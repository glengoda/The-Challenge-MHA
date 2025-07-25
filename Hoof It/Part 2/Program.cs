using System;
using System.Collections.Generic;
using System.IO;

class LavaTrailRatings
{
    static void Main(string[] args)
    {
        int[][] map = ReadMapFromFile(@"data.txt");
        int totalRating = 0;

        // Cache for memoization to speed up repeated path checks
        var memo = new Dictionary<(int, int), int>[10];

        for (int h = 0; h < 10; h++)
            memo[h] = new Dictionary<(int, int), int>();

        // Find all trailheads (height == 0)
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] == 0)
                {
                    totalRating += CountTrails(map, i, j, 0, memo);
                }
            }
        }

        Console.WriteLine("Total trailhead rating: " + totalRating);
    }

    // Reads topographic map from file into a 2D int array
    static int[][] ReadMapFromFile(string filename)
    {
        var lines = File.ReadAllLines(filename);
        int[][] map = new int[lines.Length][];

        for (int i = 0; i < lines.Length; i++)
        {
            map[i] = new int[lines[i].Length];
            for (int j = 0; j < lines[i].Length; j++)
                map[i][j] = lines[i][j] - '0';
        }

        return map;
    }

    // Count all valid trails recursively using DFS + memoization
    static int CountTrails(int[][] map, int x, int y, int currentHeight, Dictionary<(int, int), int>[] memo)
    {
        int rows = map.Length;
        int cols = map[0].Length;

        // If we're not at the expected height, return 0
        if (map[x][y] != currentHeight)
            return 0;

        // If height is 9, this is a valid path ending
        if (currentHeight == 9)
            return 1;

        // Check cache
        if (memo[currentHeight].ContainsKey((x, y)))
            return memo[currentHeight][(x, y)];

        int total = 0;

        // Check all 4 directions
        foreach (var (nx, ny) in GetNeighbors(x, y, rows, cols))
        {
            if (map[nx][ny] == currentHeight + 1)
            {
                total += CountTrails(map, nx, ny, currentHeight + 1, memo);
            }
        }

        memo[currentHeight][(x, y)] = total;
        return total;
    }

    // Get valid non-diagonal neighbors
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
