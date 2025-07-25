using System;
using System.Collections.Generic;
using System.IO;

class FenceCalculator
{
    static int rows, cols;
    static char[,] map;
    static bool[,] visited;

    // Movement directions: up, right, down, left
    static readonly int[] dx = { -1, 0, 1, 0 };
    static readonly int[] dy = { 0, 1, 0, -1 };

    static void Main()
    {
        string path = @"C:\Path\To\Your\data.txt";  // 🔁 Replace with your actual file path!

        if (!File.Exists(path))
        {
            Console.WriteLine("❌ File not found. Please check the path.");
            return;
        }

        string[] allLines = File.ReadAllLines(path);

        // 🌐 Split input into maps by blank lines
        List<List<string>> maps = new List<List<string>>();
        List<string> currentMap = new List<string>();

        foreach (var line in allLines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                if (currentMap.Count > 0)
                {
                    maps.Add(new List<string>(currentMap));
                    currentMap.Clear();
                }
            }
            else
            {
                currentMap.Add(line.TrimEnd());
            }
        }

        // Add last map if it exists
        if (currentMap.Count > 0)
            maps.Add(currentMap);

        long grandTotal = 0;

        // 🔄 Process each map
        for (int mapIndex = 0; mapIndex < maps.Count; mapIndex++)
        {
            var mapLines = maps[mapIndex];

            Console.WriteLine($"\n📦 Map #{mapIndex + 1} being processed:");
            foreach (var line in mapLines) Console.WriteLine(line);

            rows = mapLines.Count;
            cols = mapLines[0].Length;

            // Validate uniform row length
            foreach (string line in mapLines)
            {
                if (line.Length != cols)
                {
                    Console.WriteLine("❌ Error: Inconsistent row lengths in map.");
                    return;
                }
            }

            // Initialize grid and visited tracker
            map = new char[rows, cols];
            visited = new bool[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    map[i, j] = mapLines[i][j];

            int regionNum = 0;
            long mapTotal = 0;

            // 🌿 Traverse and explore each region
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (!visited[i, j])
                    {
                        regionNum++;
                        var (plantType, area, sides) = ExploreRegion(i, j, map[i, j]);
                        long price = area * sides;
                        mapTotal += price;
                        Console.WriteLine($"Region #{regionNum} (type '{plantType}') ␦ Area: {area}, Sides: {sides}, Price: {price}");
                    }
                }
            }

            Console.WriteLine($"✅ Total price of fencing all regions in this map: {mapTotal}");
            grandTotal += mapTotal;
        }

        Console.WriteLine($"\n🎯 Grand total for all maps: {grandTotal}");
    }

    // 🧭 Explore one region using BFS
    static (char type, int area, int sides) ExploreRegion(int startX, int startY, char plantType)
    {
        int area = 0;
        int sides = 0;

        Queue<(int, int)> queue = new Queue<(int, int)>();
        queue.Enqueue((startX, startY));
        visited[startX, startY] = true;

        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();
            area++;

            // Look in all 4 directions
            for (int d = 0; d < 4; d++)
            {
                int nx = x + dx[d];
                int ny = y + dy[d];

                // If it's out of bounds or a different plant, it’s a fence side
                if (nx < 0 || ny < 0 || nx >= rows || ny >= cols || map[nx, ny] != plantType)
                {
                    sides++;
                }
                else if (!visited[nx, ny])
                {
                    visited[nx, ny] = true;
                    queue.Enqueue((nx, ny));
                }
            }
        }

        return (plantType, area, sides);
    }
}
