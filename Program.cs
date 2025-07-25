using System;
using System.IO;

class Program
{
    static void Main()
    {
        var lines = File.ReadAllLines(@"data.txt");
        long totalSum = 0;

        foreach (var line in lines)
        {
            var parts = line.Split(':');
            if (parts.Length != 2) continue;

            long target = long.Parse(parts[0].Trim());
            var numbersStr = parts[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            long[] numbers = Array.ConvertAll(numbersStr, long.Parse);

            bool isMatch = CanFormTarget(numbers, target);

            Console.WriteLine($"{line} -> {(isMatch ? "MATCH ✅" : "no match ❌")}");
            if (isMatch)
                totalSum += target;
        }

        Console.WriteLine($"\nTotal calibration result: {totalSum}");
    }

    // Try every possible combination of +, *, and || between numbers
    static bool CanFormTarget(long[] numbers, long target)
    {
        int opCount = numbers.Length - 1;
        int totalCombinations = (int)Math.Pow(3, opCount); // 3 options per gap

        for (int i = 0; i < totalCombinations; i++)
        {
            int[] ops = GetOperatorCombo(i, opCount); // 0 = +, 1 = *, 2 = ||
            long result = EvaluateExpression(numbers, ops);

            if (result == target)
                return true;
        }

        return false;
    }

    // Generate operator combinations as base-3 digits
    static int[] GetOperatorCombo(int index, int length)
    {
        int[] result = new int[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = index % 3;
            index /= 3;
        }
        return result;
    }

    // Evaluate a number sequence and its operator sequence, left-to-right
    static long EvaluateExpression(long[] numbers, int[] ops)
    {
        long current = numbers[0];

        for (int i = 0; i < ops.Length; i++)
        {
            long next = numbers[i + 1];
            switch (ops[i])
            {
                case 0: // +
                    current = current + next;
                    break;
                case 1: // *
                    current = current * next;
                    break;
                case 2: // ||
                    current = Concatenate(current, next);
                    break;
            }
        }

        return current;
    }

    // Concatenate two numbers digit-wise
    static long Concatenate(long a, long b)
    {
        long multiplier = 1;
        while (b >= multiplier)
            multiplier *= 10;
        return a * multiplier + b;
    }
}
