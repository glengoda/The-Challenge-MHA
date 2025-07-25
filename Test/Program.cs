
class Program
{
    static void Main()
    {

        //declaring the list
        var left = new List<int>();
        var right = new List<int>();

        //reading from a file
        var lines = File.ReadAllLines(@"data.txt");

        //This loop goes through each line from the lines array
        foreach (var line in lines)
        {
            
            var part = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (part.Length > 0 && int.TryParse(part[0], out var number))
            {
                left.Add(number);
            }

            if (part.Length > 1 && int.TryParse(part[1], out number))
            {
                right.Add(number);
            }
        }
        //sorting my list
        left.Sort();
        right.Sort();

        //adding the total
        var totalDistance = 0;
        for (int i = 0; i < left.Count; i++)
        {
            totalDistance += Math.Abs(left[i] - right[i]);
        }

        Console.WriteLine($"Total Distance: {totalDistance}");
    }
}
