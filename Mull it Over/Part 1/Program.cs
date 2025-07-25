using System.Text.RegularExpressions;

var input = await File.ReadAllTextAsync(@".txt");
//Create a Regex object to match strings of the form "mul(x,y)"
// where x and y are integers between 0 and 999 (1 to 3 digits)
var regex = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)");
var matches = regex.Matches(input);

int total = 0;
foreach (Match match in matches)
{
    int x = int.Parse(match.Groups[1].Value);
    int y = int.Parse(match.Groups[2].Value);
    total += x * y;
}

Console.WriteLine($"Total: {total}");