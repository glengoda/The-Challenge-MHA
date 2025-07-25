using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        var map = File.ReadAllLines(@"data.txt");
        int rows = map.Length;
        int cols = map[0].Length;

        char[,] grid = new char[rows, cols];
        (int x, int y) guardPos = (0, 0);
        int dir = 0; 

        // Load grid and find guard
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                grid[y, x] = map[y][x];
                if (" ^>v<".Contains(grid[y, x]))
                {
                    guardPos = (x, y);
                    dir = "^>v<".IndexOf(grid[y, x]);
                }
            }
        }

        // Up, Right, Down, Left
        int[] dx = { 0, 1, 0, -1 };
        int[] dy = { -1, 0, 1, 0 };

        HashSet<(int x, int y)> visited = new();
        visited.Add(guardPos);

        while (true)
        {
            int nx = guardPos.x + dx[dir];
            int ny = guardPos.y + dy[dir];

            // Check if outside map
            if (nx < 0 || ny < 0 || nx >= cols || ny >= rows)
                break;

            // If there's an obstacle, turn right
            if (grid[ny, nx] == '#')
            {
                dir = (dir + 1) % 4;
                continue;
            }

            // Move forward
            guardPos = (nx, ny);
            visited.Add(guardPos);
        }

        Console.WriteLine($"Guard visited {visited.Count} distinct positions.");
    }
}
