string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day7a\Day7a\day7-data.txt";

var horizontalPositions = File.ReadLines(fileName).First().Split(',').Select(int.Parse).ToArray();

var maxValue = horizontalPositions.Max();
var minFuel = int.MaxValue;

for (int distinctPosition = 0; distinctPosition < maxValue + 1; distinctPosition++)
{
    int fuel = 0;
    foreach (var horizontalPosition in horizontalPositions)
    {
        int n = Math.Abs(horizontalPosition - distinctPosition);
        fuel += n * (n + 1) / 2;
        if (fuel > minFuel) break;
    }
    if (fuel < minFuel) minFuel = fuel;
}

Console.WriteLine($"Min fuel: {minFuel}");
