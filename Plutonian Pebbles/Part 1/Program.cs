using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        // Update this path to match your actual input file location
        var inputPath = @"data.txt";

        // Read and parse initial stone values
        string[] tokens = File.ReadAllText(inputPath)
                               .Trim()
                               .Split(new char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);

        List<long> stones = new List<long>();

        foreach (var token in tokens)
        {
            if (long.TryParse(token, out long value))
            {
                stones.Add(value);
            }
            else
            {//debug purpose
                Console.WriteLine($"Warning: Skipping invalid input token '{token}'");
            }
        }

        int blinks = 25;

        for (int blink = 0; blink < blinks; blink++)
        {
            List<long> nextStones = new List<long>();

            foreach (long stone in stones)
            {
                if (stone == 0)
                {
                    nextStones.Add(1);
                }
                else if (stone.ToString().Length % 2 == 0)
                {
                    string s = stone.ToString();
                    int half = s.Length / 2;

                    string leftStr = s.Substring(0, half).TrimStart('0');
                    string rightStr = s.Substring(half).TrimStart('0');

                    long left = 0;
                    long right = 0;

                    if (!long.TryParse(leftStr, out left)) left = 0;
                    if (!long.TryParse(rightStr, out right)) right = 0;

                    nextStones.Add(left);
                    nextStones.Add(right);
                }
                else
                {
                    // Use 2024L to make sure it does long arithmetic
                    nextStones.Add(stone * 2024L);
                }
            }

            stones = nextStones;

           
        }

        Console.WriteLine("Number of stones after 25 blinks: " + stones.Count);
    }
}
