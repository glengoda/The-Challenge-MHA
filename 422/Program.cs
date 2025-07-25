using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

class Robot  //Class Robot
{
    public int X, Y;
    public int Vx, Vy;

    public void Move() => (X, Y) = (X + Vx, Y + Vy);
    public void MoveBack() => (X, Y) = (X - Vx, Y - Vy);
}

class Program
{
    static void Main()
    {
        // Read and parse input file
        var robots = File.ReadAllLines(@"data.txt")
                         .Select(ParseRobot)
                         .ToList();

        int seconds = 0;
        long prevArea = long.MaxValue;
        int minSecond = 0;

        while (true)
        {
            seconds++;

            // Move all robots
            robots.ForEach(r => r.Move());

            // Calculate the bounding box
            int minX = robots.Min(r => r.X);
            int maxX = robots.Max(r => r.X);
            int minY = robots.Min(r => r.Y);
            int maxY = robots.Max(r => r.Y);

            long area = (long)(maxX - minX) * (maxY - minY);

            // If area starts increasing, we've passed the tightest point
            if (area > prevArea)
            {
                // Go back to previous state (tightest cluster)
                robots.ForEach(r => r.MoveBack());
                seconds--;
                Console.WriteLine($"Robots form the message after {seconds} seconds:\n");
                PrintRobots(robots);
                break;
            }

            prevArea = area;
            minSecond = seconds;
        }
    }

    static Robot ParseRobot(string line)
    {
        // Extract values from the format "p=x,y v=x,y"
        var parts = line.Split(new[] { 'p', '=', ',', 'v', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        return new Robot
        {
            X = int.Parse(parts[0]),
            Y = int.Parse(parts[1]),
            Vx = int.Parse(parts[2]),
            Vy = int.Parse(parts[3])
        };
    }

    //the method with lists of Robot
    static void PrintRobots(List<Robot> robots)
    {
        var minX = robots.Min(r => r.X);
        var maxX = robots.Max(r => r.X);
        var minY = robots.Min(r => r.Y);
        var maxY = robots.Max(r => r.Y);

        var map = new char[maxY - minY + 1, maxX - minX + 1];
        for (int y = 0; y <= maxY - minY; y++)
            for (int x = 0; x <= maxX - minX; x++)
                map[y, x] = '.';

        foreach (var r in robots)
            map[r.Y - minY, r.X - minX] = '#';

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
                Console.Write(map[y, x]);
            Console.WriteLine();
        }
    }
}
