using System;
using System.Diagnostics;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

class Program
{
    // Define 8 directions: right, down, left, up, and diagonals
    static readonly int[][] directions = new int[][]
    {
        new int[] { 0, 1 },   // right
        new int[] { 1, 0 },   // down
        new int[] { 0, -1 },  // left
        new int[] { -1, 0 },  // up
        new int[] { 1, 1 },   // down-right
        new int[] { 1, -1 },  // down-left
        new int[] { -1, -1 }, // up-left
        new int[] { -1, 1 }   // up-right
    };

    static void Main()
    {
        string filePath = @"data.txt";

        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found.");
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        int rows = lines.Length;
        int cols = lines[0].Length;

        // Load the grid
        char[,] grid = new char[rows, cols];
        for (int r = 0; r < rows; r++)
        {
            string line = lines[r].Trim();
            for (int c = 0; c < cols; c++)
            {
                grid[r, c] = line[c];
            }
        }

        string word = "XMAS";
        int count = 0;

        // Check all positions and directions
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                foreach (var dir in directions)
                {
                    if (CheckWord(grid, r, c, dir[0], dir[1], word))
                    {
                        count++;
                    }
                }
            }
        }

        Console.WriteLine($"\nTotal '{word}' found: {count}");
    }

    //the method to check word
    static bool CheckWord(char[,] grid, int r, int c, int dr, int dc, string word)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        for (int i = 0; i < word.Length; i++)
        {
            int nr = r + dr * i;
            int nc = c + dc * i;

            if (nr < 0 || nc < 0 || nr >= rows || nc >= cols)
                return false;

            if (grid[nr, nc] != word[i])
                return false;
        }

        return true;
    }
}
