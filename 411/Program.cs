using System;
using System.IO;
using System.Collections.Generic;

class RobotSimulator
{
    const int width = 101;
    const int height = 103;
    const int seconds = 100;

    static void Main()
    {
        string[] lines = File.ReadAllLines(@"data.txt");
        var positions = new List<(int x, int y)>();

        foreach (var line in lines)
        {
            // Parse line like: p=0,4 v=3,-3
            var parts = line.Split(new[] { "p=", " v=", "," }, StringSplitOptions.RemoveEmptyEntries);
            int px = int.Parse(parts[0]);
            int py = int.Parse(parts[1]);
            int vx = int.Parse(parts[2]);
            int vy = int.Parse(parts[3]);

            // New position after 100s, wrap around using modulo
            int newX = (px + vx * seconds) % width;
            int newY = (py + vy * seconds) % height;

            // Ensure positive positions
            if (newX < 0) newX += width;
            if (newY < 0) newY += height;

            positions.Add((newX, newY));
        }

        // Determine midpoints (excluding them from quadrant count)
        int midX = width / 2;
        int midY = height / 2;

        // Quadrant counters
        int topLeft = 0, topRight = 0, bottomLeft = 0, bottomRight = 0;

        foreach (var (x, y) in positions)
        {
            if (x == midX || y == midY) continue;

            if (x < midX && y < midY) topLeft++;
            else if (x > midX && y < midY) topRight++;
            else if (x < midX && y > midY) bottomLeft++;
            else if (x > midX && y > midY) bottomRight++;
        }

        int safetyFactor = topLeft * topRight * bottomLeft * bottomRight;

        Console.WriteLine($"Safety Factor: {safetyFactor}");
    }
}
