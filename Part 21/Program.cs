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

        int sumOfMiddlePages = 0;

        foreach (var update in updates)
        {
            if (!IsValid(update, rules))
            {
                var sortedUpdate = TopologicalSort(update, rules);
                if (sortedUpdate.Count > 0)
                {
                    int midIndex = sortedUpdate.Count / 2;
                    sumOfMiddlePages += sortedUpdate[midIndex];
                }
            }
        }

        Console.WriteLine($"Sum of middle pages from corrected invalid updates: {sumOfMiddlePages}");
    }

    static bool IsValid(List<int> update, List<(int Before, int After)> rules)
    {
        Dictionary<int, int> position = new();
        for (int i = 0; i < update.Count; i++)
            position[update[i]] = i;

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

    static List<int> TopologicalSort(List<int> update, List<(int Before, int After)> rules)
    {
        // Only use rules relevant to this update
        var updateSet = new HashSet<int>(update);
        var adj = new Dictionary<int, List<int>>();
        var indegree = new Dictionary<int, int>();

        foreach (var page in update)
        {
            adj[page] = new List<int>();
            indegree[page] = 0;
        }

        foreach (var (before, after) in rules)
        {
            if (updateSet.Contains(before) && updateSet.Contains(after))
            {
                adj[before].Add(after);
                indegree[after]++;
            }
        }

        // Kahn’s algorithm for topological sorting of a Directed Acyclic Graph (DAG).
        var queue = new Queue<int>(indegree.Where(kv => kv.Value == 0).Select(kv => kv.Key));
        var result = new List<int>();

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            result.Add(node);

            foreach (var neighbor in adj[node])
            {
                indegree[neighbor]--;
                if (indegree[neighbor] == 0)
                    queue.Enqueue(neighbor);
            }
        }

        // Check if we were able to sort all nodes
        if (result.Count != update.Count)
            return new List<int>(); // cycle detected or sorting failed

        return result;
    }
}

