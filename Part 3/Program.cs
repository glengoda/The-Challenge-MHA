using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        var solver = new PuzzleSolver();
        int result = await solver.Solve(@"data.txt");
        Console.WriteLine($"Total: {result}");
        Console.ReadKey(); // Keeps console open so you can see the output
    }
}

public class PuzzleSolver
{
    public async Task<int> Solve(string filePath)
    {
        int total = 0;
        bool enabled = true;

        // Read all content from the file
        string inputText = await File.ReadAllTextAsync(filePath);

        // Regex pattern to match do(), don't(), or mul(x,y)
        var regex = new Regex(@"(do\(\))|(don't\(\))|(mul\((\d{1,3}),(\d{1,3})\))");
        var matches = regex.Matches(inputText);

        foreach (Match match in matches)
        {
            if (match.Value == "do()")
            {
                enabled = true;
            }
            else if (match.Value == "don't()")
            {
                enabled = false;
            }
            else if (match.Groups[3].Success && enabled) // mul(x,y) and currently enabled
            {
                int x = int.Parse(match.Groups[4].Value);
                int y = int.Parse(match.Groups[5].Value);
                total += x * y;
            }
        }

        return total;
    }
}
