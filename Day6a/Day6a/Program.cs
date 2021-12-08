int days = 80;
string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day6a\Day6a\day6-data.txt";

var listOfAges = File.ReadAllLines(fileName);
var oldFish = listOfAges[0].Split(',').Select(int.Parse).ToList();

//DumpAges(oldFish);

for (int day = 0; day < days; day++)
{
    var newFish = new List<int>();

    for (int i = 0; i < oldFish.Count; i++)
    {
        if (--oldFish[i] < 0)
        {
            oldFish[i] = 6;
            newFish.Add(8);
        }
    }

    oldFish.AddRange(newFish);

    //DumpAges(oldFish, day);
}

Console.WriteLine($"Number of fish on day {days}: {oldFish.Count}");

void DumpAges(List<int> ages, int? day = null)
{
    if (day == null)
        Console.Write("Initial state:    ");
    else
        Console.Write($"After {(++day).ToString(),5} days: ");

    string sep = "";
    foreach (var age in ages)
    {
        Console.Write($"{sep}{age}");
        sep = ",";
    }

    Console.WriteLine();
}