using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

class ClawMachineSolver
{
    // Represents a single claw machine class
    public class Machine
    {
        public int Ax, Ay; // Vector A coordinates
        public int Bx, By; // Vector B coordinates
        public int Px, Py; // Prize position coordinates
    }

    static void Main()
    {
        try
        {
            var machines = LoadMachines(@"data.txt");

            int totalCost = 0;
            int prizesWon = 0;

            foreach (var machine in machines)
            {
                int? minCost = FindMinimumCost(machine);

                if (minCost.HasValue)
                {
                    prizesWon++;
                    totalCost += minCost.Value;
                }
            }

            Console.WriteLine($"Prizes won: {prizesWon}");
            Console.WriteLine($"Total tokens spent: {totalCost}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred: {ex.Message}");
        }
    }

 
    //Loads machines from a text file.
    // Expects each machine's data to be 4 lines with specific formats.
   
    static List<Machine> LoadMachines(string filePath)
    {
        var machines = new List<Machine>();
        var lines = File.ReadAllLines(filePath);

        if (lines.Length % 4 != 0)
            throw new Exception("Input file format invalid: number of lines is not multiple of 4.");

        for (int i = 0; i < lines.Length; i += 4)
        {
            machines.Add(ParseMachine(lines, i));
        }

        return machines;
    }

    // Parses a machine's data from 4 lines starting at given index.
    static Machine ParseMachine(string[] lines, int startIndex)
    {
        int ParseCoordinate(string line, string pattern)
        {
            var match = Regex.Match(line.Trim(), pattern);
            if (!match.Success)
                throw new Exception($"Invalid line format: '{line}'");
            return int.Parse(match.Groups[1].Value);
        }

        return new Machine
        {
            Ax = ParseCoordinate(lines[startIndex], @"X\+(\d+)"),
            Ay = ParseCoordinate(lines[startIndex], @"Y\+(\d+)"),
            Bx = ParseCoordinate(lines[startIndex + 1], @"X\+(\d+)"),
            By = ParseCoordinate(lines[startIndex + 1], @"Y\+(\d+)"),
            Px = ParseCoordinate(lines[startIndex + 2], @"X=(\d+)"),
            Py = ParseCoordinate(lines[startIndex + 2], @"Y=(\d+)")
        };
    }

   // Find the minimum cost of tokens (a*3 + b) needed to reach the prize position by using non-negative integer multiples of vectors A and B Returns null if no combination found.
    
    static int? FindMinimumCost(Machine machine)
    {
        int? minCost = null;

        // Limit search space (adjust maxCount if needed)
        const int maxCount = 100;

        for (int countA = 0; countA <= maxCount; countA++)
        {
            for (int countB = 0; countB <= maxCount; countB++)
            {
                int x = countA * machine.Ax + countB * machine.Bx;
                int y = countA * machine.Ay + countB * machine.By;

                if (x == machine.Px && y == machine.Py)
                {
                    int cost = countA * 3 + countB;
                    if (minCost == null || cost < minCost)
                    {
                        minCost = cost;
                    }
                }
            }
        }

        return minCost;
    }
}
