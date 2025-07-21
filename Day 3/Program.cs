using System;
using System.IO;

class Program
{
    static void Main()
    {
        //declare
        int safeCount = 0;
        
        string filePath = @"C:\Users\tshem\source\repos\Test\Day 3\bin\Debug\net8.0\data3.txt";

        foreach (var line in File.ReadLines(filePath))
        {
            //skip empty lines or lines that contain only whitespace
            //clean & parse each line into numbers, check if safe with calling the method
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            try
            {
                var levels = Array.ConvertAll(parts, int.Parse);

                if (IsSafe(levels))
                {
                    Console.WriteLine($"Safe:     {string.Join(" ", levels)}");
                    safeCount++;
                }
                else
                {
                    Console.WriteLine($"Not Safe: {string.Join(" ", levels)}");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine($"Invalid line (skipped): {line}");
            }
        }

        Console.WriteLine($"\nNumber of safe reports: {safeCount}");
    }

    //the method for playing the rules
    static bool IsSafe(int[] levels)
    {
        if (levels.Length < 2)
            return false;

        int? direction = null; // 1 = increasing, -1 = decreasing

        for (int i = 1; i < levels.Length; i++)
        {
            int diff = levels[i] - levels[i - 1];

            if (diff == 0 || Math.Abs(diff) > 3)
                return false; // Invalid step

            int currentDirection = diff > 0 ? 1 : -1;

            if (direction == null)
            {
                direction = currentDirection;
            }
            else if (direction != currentDirection)
            {
                return false; // Changed direction mid-sequence
            }
        }

        return true;
    }
}
