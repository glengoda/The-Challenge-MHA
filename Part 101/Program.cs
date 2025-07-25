using System;
using System.Collections.Generic;
using System.IO;

class LavaIslandHiking
{
    static void Main(string[] args)
    {
        // Read the topographic map from file
        int[][] map = ReadMapFromFile(@"C:\Users\tshem\source\repos\Test\Part 101\bin\Debug\net8.0\data1.txt");
        int totalScore = 0;

        // Traverse the entire map to find trailheads (height == 0)
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] == 0)
                {
                    totalScore += GetTrailheadScore(map, i, j);
                }
            }
        }

        Console.WriteLine("Total trailhead score: " + totalScore);
    }

    // Reads the topographic map from a text file
    static int[][] ReadMapFromFile(string filename)
    {
        var lines = File.ReadAllLines(filename);
        int[][] map = new int[lines.Length][];

        for (int i = 0; i < lines.Length; i++)
        {
            map[i] = new int[lines[i].Length];
            for (int j = 0; j < lines[i].Length; j++)
            {
                map[i][j] = lines[i][j] - '0'; // Convert char to int
            }
        }

        return map;
    }

    // Performs BFS from a trailhead to find reachable 9s
    static int GetTrailheadScore(int[][] map, int startX, int startY)
    {
        int rows = map.Length;
        int cols = map[0].Length;

        var visited = new HashSet<(int, int)>();
        var reachedNines = new HashSet<(int, int)>();
        var queue = new Queue<(int x, int y, int height)>();
        queue.Enqueue((startX, startY, 0));

        while (queue.Count > 0)
        {
            var (x, y, height) = queue.Dequeue();

            if (visited.Contains((x, y)))
                continue;
            visited.Add((x, y));

            if (map[x][y] != height)
                continue;

            if (map[x][y] == 9)
            {
                reachedNines.Add((x, y));
                continue;
            }

            // Check 4 directions: up, down, left, right
            foreach (var (nx, ny) in GetNeighbors(x, y, rows, cols))
            {
                if (map[nx][ny] == map[x][y] + 1)
                {
                    queue.Enqueue((nx, ny, map[nx][ny]));
                }
            }
        }

        return reachedNines.Count;
    }

    // Gets valid non-diagonal neighbors within bounds
    static List<(int, int)> GetNeighbors(int x, int y, int rows, int cols)
    {
        var directions = new (int, int)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
        var neighbors = new List<(int, int)>();

        foreach (var (dx, dy) in directions)
        {
            int nx = x + dx;
            int ny = y + dy;
            if (nx >= 0 && nx < rows && ny >= 0 && ny < cols)
                neighbors.Add((nx, ny));
        }

        return neighbors;
    }
}

