using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

class ClawMachine
{
    public (long x, long y) A, B, Prize;
}

class Program
{
    static void Main()
    {
        var machines = ParseMachines(@"C:\Users\tshem\source\repos\Test\34\bin\Debug\net8.0\data.txt");

        long totalTokens = 0;
        int wonCount = 0;

        foreach (var machine in machines)
        {
            var result = Solve(machine);
            if (result != null)
            {
                wonCount++;
                totalTokens += result.Value;
            }
        }

        Console.WriteLine($"Total prizes won: {wonCount}");
        Console.WriteLine($"Minimum total tokens spent: {totalTokens}");
    }

    // Reads machine definitions from file
    static List<ClawMachine> ParseMachines(string filename)
    {
        var lines = File.ReadAllLines(filename);
        var machines = new List<ClawMachine>();

        for (int i = 0; i < lines.Length; i += 4)
        {
            var aMatch = Regex.Match(lines[i], @"X\+(\d+), Y\+(\d+)");
            var bMatch = Regex.Match(lines[i + 1], @"X\+(\d+), Y\+(\d+)");
            var prizeMatch = Regex.Match(lines[i + 2], @"X=(\d+), Y=(\d+)");

            var machine = new ClawMachine
            {
                A = (long.Parse(aMatch.Groups[1].Value), long.Parse(aMatch.Groups[2].Value)),
                B = (long.Parse(bMatch.Groups[1].Value), long.Parse(bMatch.Groups[2].Value)),
                Prize = (long.Parse(prizeMatch.Groups[1].Value), long.Parse(prizeMatch.Groups[2].Value))
            };

            machines.Add(machine);
        }

        return machines;
    }

    // Solves the machine for minimum cost if possible
    static long? Solve(ClawMachine machine)
    {
        var (aX, aY) = machine.A;
        var (bX, bY) = machine.B;
        var (px, py) = machine.Prize;

        if (!TrySolve2D(aX, bX, px, aY, bY, py, out long aPresses, out long bPresses))
            return null;

        return aPresses * 3 + bPresses;
    }

    // Extended Euclidean Algorithm: solves ax + by = gcd(a, b)
    static long GCD(long a, long b, out long x, out long y)
    {
        x = 1; y = 0;
        long x1 = 0, y1 = 1;
        while (b != 0)
        {
            long q = a / b;
            (a, b) = (b, a % b);
            (x, x1) = (x1, x - q * x1);
            (y, y1) = (y1, y - q * y1);
        }
        return a;
    }

    // Solves linear Diophantine equation a*x + b*y = c
    static bool SolveLinear(long a, long b, long c, out long x, out long y)
    {
        long g = GCD(a, b, out x, out y);
        if (c % g != 0)
        {
            x = y = 0;
            return false;
        }

        long scale = c / g;
        x *= scale;
        y *= scale;
        return true;
    }

    // Attempts to find valid non-negative integer solutions (a, b)
    // such that aX*a + bX*b = pX and aY*a + bY*b = pY
    static bool TrySolve2D(long aX, long bX, long pX, long aY, long bY, long pY,
        out long bestA, out long bestB)
    {
        bestA = bestB = 0;

        // Step 1: Solve aX * x + bX * y = pX
        if (!SolveLinear(aX, bX, pX, out long x0, out long y0))
            return false;

        long g = GCD(aX, bX, out _, out _);
        long dx = bX / g;
        long dy = -aX / g;

        long bestCost = long.MaxValue;
        bool found = false;

        // Try a range of t values to find best solution
        // Adjust bounds as needed for performance
        for (long t = -100000; t <= 100000; t++)
        {
            long a = x0 + dx * t;
            long b = y0 + dy * t;

            if (a < 0 || b < 0)
                continue;

            // Check if the second equation is satisfied
            if (aY * a + bY * b != pY)
                continue;

            long cost = 3 * a + b;
            if (cost < bestCost)
            {
                bestCost = cost;
                bestA = a;
                bestB = b;
                found = true;
            }
        }

        return found;
    }
}
