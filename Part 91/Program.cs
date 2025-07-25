using System;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        // Read input line from file
        string input = File.ReadAllText(@"data.txt").Trim();

        // Parse input into disk array
        var disk = ParseDiskMap(input);
        CompactDisk(disk);
        long checksum = CalculateChecksum(disk);

        Console.WriteLine($"Checksum: {checksum}");
    }
    //the ParseDiskMap method
    static char[] ParseDiskMap(string input)
    {
        int index = 0;
        int fileId = 0;
        var diskList = new StringBuilder();

        while (index < input.Length)
        {
            // File length
            int fileLength = input[index] - '0';
            index++;

            // Add file blocks with current fileId
            for (int i = 0; i < fileLength; i++)
                diskList.Append((char)('0' + fileId));

            fileId++;

            if (index >= input.Length)
                break;

            // Free space length
            int freeLength = input[index] - '0';
            index++;

            // Add free space blocks '.'
            for (int i = 0; i < freeLength; i++)
                diskList.Append('.');
        }

        return diskList.ToString().ToCharArray();
    }
    //the method CompactDisk
    static void CompactDisk(char[] disk)
    {
        while (true)
        {
            int leftmostFree = Array.IndexOf(disk, '.');
            if (leftmostFree == -1)
                break; // No free space left

            // Check if all free spaces are at the end (no '.' before files)
            bool allFreeAtEnd = true;
            for (int i = 0; i < disk.Length; i++)
            {
                if (disk[i] == '.' && i < disk.Length - 1 && disk[i + 1] != '.')
                {
                    allFreeAtEnd = false;
                    break;
                }
            }
            if (allFreeAtEnd) break;

            // Find rightmost file block that is to the right of leftmostFree
            int rightmostFilePos = -1;
            for (int i = disk.Length - 1; i > leftmostFree; i--)
            {
                if (disk[i] != '.')
                {
                    rightmostFilePos = i;
                    break;
                }
            }

            if (rightmostFilePos == -1)
                break; // No file blocks to move

            // Move the file block from rightmostFilePos to leftmostFree
            disk[leftmostFree] = disk[rightmostFilePos];
            disk[rightmostFilePos] = '.';
        }
    }

    //method the CalculateChecksum
    static long CalculateChecksum(char[] disk)
    {
        long sum = 0;
        for (int i = 0; i < disk.Length; i++)
        {
            if (disk[i] != '.')
            {
                int fileId = disk[i] - '0';
                sum += i * (long)fileId;
            }
        }
        return sum;
    }
}
