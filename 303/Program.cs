using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Read all lines from input.txt (change path as needed)
        string[] lines = File.ReadAllLines(@"data.txt");
        int rows = lines.Length;
        int cols = lines[0].Trim().Length;

        char[][] map = new char[rows][];
        bool[][] visited = new bool[rows][];

        for (int i = 0; i < rows; i++)
        {
            map[i] = lines[i].Trim().ToCharArray();
            visited[i] = new bool[cols];
        }

        int totalPrice = 0;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (!visited[r][c])
                {
                    var (area, sides, type) = FloodFill(map, visited, r, c);
                    int price = area * sides;
                    Console.WriteLine($"Region '{type}' → Area: {area}, Sides: {sides}, Price: {price}");
                    totalPrice += price;
                }
            }
        }

        Console.WriteLine($"\nTotal Price: {totalPrice}");
    }

    static (int area, int sides, char type) FloodFill(char[][] map, bool[][] visited, int startR, int startC)
    {
        int[] dr = { -1, 1, 0, 0 };
        int[] dc = { 0, 0, -1, 1 };

        int rows = map.Length;
        int cols = map[0].Length;

        char regionType = map[startR][startC];

        Queue<(int, int)> queue = new();
        queue.Enqueue((startR, startC));
        visited[startR][startC] = true;

        int area = 0;
        int sides = 0;

        while (queue.Count > 0)
        {
            var (r, c) = queue.Dequeue();
            area++;

            // Check all 4 directions around current cell
            for (int d = 0; d < 4; d++)
            {
                int nr = r + dr[d];
                int nc = c + dc[d];

                // If neighbor out of bounds or different type => increment sides
                if (nr < 0 || nc < 0 || nr >= rows || nc >= cols || map[nr][nc] != regionType)
                {
                    sides++;
                }
                else if (!visited[nr][nc])
                {
                    visited[nr][nc] = true; // Mark visited immediately on enqueue
                    queue.Enqueue((nr, nc));
                }
            }
        }

        return (area, sides, regionType);
    }
}
