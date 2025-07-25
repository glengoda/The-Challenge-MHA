using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        var inputPath = @"data.txt";

        string[] tokens = File.ReadAllText(inputPath)
                               .Trim()
                               .Split(new char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);

        // Dictionary: stone value count of that stone
        Dictionary<long, long> stoneCounts = new Dictionary<long, long>();

        foreach (var token in tokens)
        {
            if (long.TryParse(token, out long value))
            {
                if (!stoneCounts.ContainsKey(value))
                    stoneCounts[value] = 0;
                stoneCounts[value]++;
            }
            else
            {
                Console.WriteLine($"Warning: Skipping invalid input token '{token}'");
            }
        }

        int blinks = 75;

        for (int blink = 0; blink < blinks; blink++)
        {
            Dictionary<long, long> nextCounts = new Dictionary<long, long>();

            foreach (var kvp in stoneCounts)
            {
                long stone = kvp.Key;
                long count = kvp.Value;

                if (stone == 0)
                {
                    // Rule 1: 0 -> 1
                    AddStone(nextCounts, 1, count);
                }
                else if (stone.ToString().Length % 2 == 0)
                {
                    // Rule 2: even number of digits -> split into two
                    string s = stone.ToString();
                    int half = s.Length / 2;

                    string leftStr = s.Substring(0, half).TrimStart('0');
                    string rightStr = s.Substring(half).TrimStart('0');

                    long left = string.IsNullOrEmpty(leftStr) ? 0 : long.Parse(leftStr);
                    long right = string.IsNullOrEmpty(rightStr) ? 0 : long.Parse(rightStr);

                    AddStone(nextCounts, left, count);
                    AddStone(nextCounts, right, count);
                }
                else
                {
                    // Rule 3: multiply by 2024
                    AddStone(nextCounts, stone * 2024L, count);
                }
            }

            stoneCounts = nextCounts;

            Console.WriteLine($"After blink {blink + 1}: {TotalStones(stoneCounts)} stones");
        }

        Console.WriteLine("\n Final result after 75 blinks:");
        Console.WriteLine($"Total stones: {TotalStones(stoneCounts)}");
    }

    static void AddStone(Dictionary<long, long> dict, long stone, long count)
    {
        if (!dict.ContainsKey(stone))
            dict[stone] = 0;
        dict[stone] += count;
    }

    static long TotalStones(Dictionary<long, long> dict)
    {
        long total = 0;
        foreach (var count in dict.Values)
        {
            total += count;
        }
        return total;
    }
}

