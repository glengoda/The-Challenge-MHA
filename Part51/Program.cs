using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        string[] lines = File.ReadAllLines(@"data.txt");
        int height = lines.Length;
        int width = lines[0].Length;

        // Dictionary to group antenna positions by frequency
        Dictionary<char, List<(int x, int y)>> antennasByFreq = new();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                char ch = lines[y][x];
                if (char.IsLetterOrDigit(ch))
                {
                    if (!antennasByFreq.ContainsKey(ch))
                        antennasByFreq[ch] = new List<(int x, int y)>();
                    antennasByFreq[ch].Add((x, y));
                }
            }
        }

        // HashSet to store unique antinode positions
        HashSet<(int x, int y)> antinodes = new();

        foreach (var entry in antennasByFreq)
        {
            var positions = entry.Value;
            int count = positions.Count;

            // Compare every pair of antennas with the same frequency
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    if (i == j) continue;

                    var a = positions[i];
                    var b = positions[j];

                    // Compute vector from a to b
                    int dx = b.x - a.x;
                    int dy = b.y - a.y;

                    // Compute the third point such that a is the midpoint between b and the antinode
                    int antinodeX = a.x - dx;
                    int antinodeY = a.y - dy;

                    // Check bounds
                    if (antinodeX >= 0 && antinodeX < width && antinodeY >= 0 && antinodeY < height)
                    {
                        antinodes.Add((antinodeX, antinodeY));
                    }
                }
            }
        }

        Console.WriteLine($"Total unique antinodes: {antinodes.Count}");
    }
}
