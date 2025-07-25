using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        var lines = File.ReadAllLines(@"data.txt");

        List<(int Before, int After)> rules = new();
        List<List<int>> updates = new();
        bool parsingRules = true;

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                parsingRules = false;
                continue;
            }

            if (parsingRules)
            {
                var parts = line.Split('|');
                rules.Add((int.Parse(parts[0]), int.Parse(parts[1])));
            }
            else
            {
                updates.Add(line.Split(',').Select(int.Parse).ToList());
            }
        }

        int sum = 0;

        foreach (var update in updates)
        {
            if (IsValid(update, rules))
            {
                int midIndex = update.Count / 2;
                Console.WriteLine($"Valid: {string.Join(",", update)} → +{update[midIndex]}");
                sum += update[midIndex];
            }
            else
            {
                Console.WriteLine($"Invalid: {string.Join(",", update)}");
            }
        }

        Console.WriteLine($"\nTotal sum of middle pages from valid updates: {sum}");
    }
    //checks if a given update sequence respects all the ordering rules specified as (Before, After) pairs
    static bool IsValid(List<int> update, List<(int Before, int After)> rules)
    {
        Dictionary<int, int> position = new();
        for (int i = 0; i < update.Count; i++)
        {
            position[update[i]] = i;
        }

        foreach (var rule in rules)
        {
            if (position.ContainsKey(rule.Before) && position.ContainsKey(rule.After))
            {
                if (position[rule.Before] >= position[rule.After])
                    return false;
            }
        }

        return true;
    }
}
