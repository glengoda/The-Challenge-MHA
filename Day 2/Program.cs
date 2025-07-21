using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        //declare
        string filePath = @"C:\Users\tshem\source\repos\Test\Day 3\bin\Debug\net8.0\data3.txt";
        var inputLines = File.ReadAllLines(filePath);
        
        int safeCount = CountSafeReportsWithDampener(inputLines);
        Console.WriteLine($"\nNumber of safe reports: {safeCount}");
    }

    //the method to check consistent direction
    static bool IsConsistentDirection(List<int> levels)
    {
        if (levels.Count < 2)
            return false;

        int? direction = null;

        for (int i = 1; i < levels.Count; i++)
        {
            int diff = levels[i] - levels[i - 1];

            if (diff == 0 || Math.Abs(diff) > 3)
                return false;

            int currentDirection = diff > 0 ? 1 : -1;

            if (direction == null)
                direction = currentDirection;
            else if (currentDirection != direction)
                return false;
        }

        return true;
    }

    //the method to check safe with damperner
    static bool IsSafeWithDampener(List<int> levels)
    {
        if (IsConsistentDirection(levels))
            return true;

        for (int i = 0; i < levels.Count; i++)
        {
            var modified = new List<int>(levels);
            modified.RemoveAt(i);
            if (IsConsistentDirection(modified))
                return true;
        }

        return false;
    }
    //the method Count safe report with ampener
    static int CountSafeReportsWithDampener(string[] inputLines)
    {
        int safeCount = 0;

        foreach (var line in inputLines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var levels = line
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            if (IsSafeWithDampener(levels))
            {
                Console.WriteLine($"Safe:     {line}");
                safeCount++;
            }
            else
            {
                Console.WriteLine($"Not Safe: {line}");
            }
        }

        return safeCount;
    }
}
