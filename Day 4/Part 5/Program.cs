using System;
using System.IO;

class Program
{
    static void Main()
    {
        string[] grid = File.ReadAllLines(@"data.txt");
        int rows = grid.Length;
        int cols = grid[0].Length;
        int count = 0;

        string[] validMAS = { "MAS", "SAM" };

        for (int y = 1; y < rows - 1; y++)
        {
            for (int x = 1; x < cols - 1; x++)
            {
                char[] diag1 = { grid[y - 1][x - 1], grid[y][x], grid[y + 1][x + 1] }; // top-left to bottom-right
                char[] diag2 = { grid[y - 1][x + 1], grid[y][x], grid[y + 1][x - 1] }; // top-right to bottom-left

                string diag1Str = new string(diag1);
                string diag2Str = new string(diag2);
                //checking if array has the position 
                if (Array.Exists(validMAS, s => s == diag1Str) &&
                    Array.Exists(validMAS, s => s == diag2Str))
                {
                    count++;
                }
            }
        }

        Console.WriteLine($"Total X-MAS count: {count}");
    }
}