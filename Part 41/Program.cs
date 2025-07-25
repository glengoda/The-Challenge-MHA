using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        // Read all lines from data file
        var lines = File.ReadAllLines(@"data.txt");
        long totalSum = 0;

        foreach (var line in lines)
        {
            // Split line by colon to separate target from numbers
            var parts = line.Split(':');
            if (parts.Length != 2) continue;

            // Parse the target value as long
            long target = long.Parse(parts[0].Trim());

            // Parse the list of numbers as long[]
            var numbersStr = parts[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            long[] numbers = Array.ConvertAll(numbersStr, long.Parse);

            // Check if any combination of +/* matches target
            if (CanFormTarget(numbers, target))
            {
                totalSum += target;
            }
        }

        Console.WriteLine($"Total calibration result: {totalSum}");
    }

    // method CanFormTarget recursive  to try all combinations of +/*
    static bool CanFormTarget(long[] numbers, long target, int idx = 1, long currentValue = long.MinValue)
    {
        if (currentValue == long.MinValue)
        {
            currentValue = numbers[0];
        }

        if (idx == numbers.Length)
        {
            return currentValue == target;
        }

     
        if (CanFormTarget(numbers, target, idx + 1, currentValue + numbers[idx]))
            return true;

     
        if (CanFormTarget(numbers, target, idx + 1, currentValue * numbers[idx]))
            return true;

        return false;
    }
}
