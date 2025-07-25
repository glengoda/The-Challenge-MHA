using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    // Compute gcd for reducing step vector
    static int GCD(int a, int b)
    {
        a = Math.Abs(a);
        b = Math.Abs(b);
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    static void Main()
    {
        string[] lines = File.ReadAllLines(@"data.txt");
        int rows = lines.Length;
        int cols = lines[0].Length;

        Dictionary<char, List<(int r, int c)>> antennaMap = new();

        // Parse grid and collect antenna positions by frequency
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                char ch = lines[r][c];
                if (ch != '.' && char.IsLetterOrDigit(ch))
                {
                    if (!antennaMap.ContainsKey(ch))
                        antennaMap[ch] = new List<(int, int)>();
                    antennaMap[ch].Add((r, c));
                }
            }
        }

        var antinodes = new HashSet<(int, int)>();

        foreach (var entry in antennaMap)
        {
            var positions = entry.Value;
            int count = positions.Count;

            // Only frequencies with ≥2 antennas
            if (count < 2)
                continue;

            // For each pair of antennas, find all points on the infinite line in the grid
            for (int i = 0; i < count; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    var a = positions[i];
                    var b = positions[j];

                    int dr = b.r - a.r;
                    int dc = b.c - a.c;
                    int gcd = GCD(dr, dc);

                    int stepR = dr / gcd;
                    int stepC = dc / gcd;

                    // Walk forward from a to edge of grid
                    int r = a.r;
                    int c = a.c;
                    while (r >= 0 && r < rows && c >= 0 && c < cols)
                    {
                        antinodes.Add((r, c));
                        r += stepR;
                        c += stepC;
                    }

                    // Walk backward from a to edge of grid
                    r = a.r - stepR;
                    c = a.c - stepC;
                    while (r >= 0 && r < rows && c >= 0 && c < cols)
                    {
                        antinodes.Add((r, c));
                        r -= stepR;
                        c -= stepC;
                    }
                }
            }
        }

        Console.WriteLine($"Total unique antinode positions: {antinodes.Count}");
    }
}
