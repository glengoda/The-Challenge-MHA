class Program
{
    static void Main()
    {

        //declaring the list
        var left = new List<int>();
        var right = new List<int>();

        var count2 = new Dictionary<int, int>();

        //reading from a file
        var lines = File.ReadAllLines(@"C:\Users\tshem\source\repos\Test\Part 2\bin\Debug\net8.0\data2.txt");

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

                if(count2.ContainsKey(number))
                {
                    count2[number]++;
                }
                else
                {
                    count2[number] = 1;
                }
            }
        }

        
        //eg for each number (3) if exist in count2 then 3 * (times show ) m add the total
        var total = 0;
        for (int i = 0; i < left.Count; i++)
        {
            if (count2.ContainsKey(left[i]))
            {
                total += left[i] * count2[left[i]];
            }
        }

        Console.WriteLine($"Total Distance: {total}");
    }
}