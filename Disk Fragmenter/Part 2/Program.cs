using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

class Program
{
    static void Main()
    {
        string input = File.ReadAllText(@"data.txt").Trim();
        var disk = ParseDiskMap(input, out Dictionary<int, (int Start, int Length)> fileMap);

        CompactWholeFiles(disk, fileMap);

        long checksum = CalculateChecksum(disk);
        Console.WriteLine($"Checksum: {checksum}");
    }

    // Parse the input into a disk array and file map
    static char[] ParseDiskMap(string input, out Dictionary<int, (int Start, int Length)> fileMap)
    {
        fileMap = new Dictionary<int, (int, int)>();
        var diskList = new List<char>();
        int fileId = 0;
        int i = 0;

        while (i < input.Length)
        {
            int fileLength = input[i++] - '0';
            int fileStart = diskList.Count;
            for (int j = 0; j < fileLength; j++)
                diskList.Add((char)('0' + fileId));
            fileMap[fileId] = (fileStart, fileLength);
            fileId++;

            if (i >= input.Length) break;

            int freeLength = input[i++] - '0';
            for (int j = 0; j < freeLength; j++)
                diskList.Add('.');
        }

        return diskList.ToArray();
    }

    // Compact whole files leftward into the first fitting free space
    static void CompactWholeFiles(char[] disk, Dictionary<int, (int Start, int Length)> fileMap)
    {
        int maxFileId = fileMap.Count - 1;

        for (int fileId = maxFileId; fileId >= 0; fileId--)
        {
            var (start, length) = fileMap[fileId];

            // Find all free spans to the left of this file
            for (int i = 0; i <= start - length; i++)
            {
                bool spaceAvailable = true;
                for (int j = 0; j < length; j++)
                {
                    if (disk[i + j] != '.')
                    {
                        spaceAvailable = false;
                        break;
                    }
                }

                if (spaceAvailable)
                {
                    // Move the file
                    for (int j = 0; j < length; j++)
                    {
                        disk[i + j] = (char)('0' + fileId);
                        disk[start + j] = '.';
                    }

                    // Update fileMap in case needed later (not strictly required here)
                    fileMap[fileId] = (i, length);
                    break; // File moved, continue to next
                }
            }
        }
    }

    // Checksum: sum of (position * fileId) for all non-'.' blocks
    static long CalculateChecksum(char[] disk)
    {
        long sum = 0;
        for (int i = 0; i < disk.Length; i++)
        {
            if (disk[i] != '.')
            {
                int fileId = disk[i] - '0';
                sum += i * fileId;
            }
        }
        return sum;
    }
}
