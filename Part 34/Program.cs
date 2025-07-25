using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static int[] dx = { 0, 1, 0, -1 }; // Up, Right, Down, Left
    static int[] dy = { -1, 0, 1, 0 };

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
                if ("^>v<".Contains(grid[y, x]))
                {
                    guardPos = (x, y);
                    dir = "^>v<".IndexOf(grid[y, x]);
                }
            }
        }

        int loopCount = 0;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                if (grid[y, x] == '.' && (x != guardPos.x || y != guardPos.y))
                {
                    // place obstacle temporarily
                    grid[y, x] = '#'; 
                    if (CausesLoop(grid, guardPos, dir, rows, cols))
                        loopCount++;
                    // remove obstacle
                    grid[y, x] = '.';
                }
            }
        }

        Console.WriteLine($"Number of positions causing loop: {loopCount}");
    }

    //The causesLoop
    static bool CausesLoop(char[,] grid, (int x, int y) startPos, int startDir, int rows, int cols)
    {
        var visited = new HashSet<(int x, int y, int dir)>();
        var pos = startPos;
        int dir = startDir;

        while (true)
        {
            // If we have visited this state, loop detected
            if (visited.Contains((pos.x, pos.y, dir)))
                return true;

            visited.Add((pos.x, pos.y, dir));

            int nx = pos.x + dx[dir];
            int ny = pos.y + dy[dir];

            if (nx < 0 || ny < 0 || nx >= cols || ny >= rows)
                return false;

            if (grid[ny, nx] == '#')
            {
                // turn right if blocked
                dir = (dir + 1) % 4;
                continue;
            }

            pos = (nx, ny);
        }
    }
}
