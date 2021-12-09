int simpleCount = 0;
int[] digits = new int[] { 6, 2, 5, 5, 4, 5, 6, 3, 7, 6 };
string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day8a\Day8a\day8-data.txt";

var displayLines = File.ReadAllLines(fileName);

foreach (var displayLine in displayLines)
{
    var segments = displayLine.Split(" | ");
    var signalPatterns = segments[0].Split(" ").ToList();
    var outputValues = segments[1].Split(" ").ToList();

    simpleCount += 
        outputValues
            .Select(outputValue => digits.Count(digit => digit == outputValue.Length))
            .Count(count => count == 1);
}

Console.WriteLine($"How many times to digits 1, 4, 7, or 8 appear? {simpleCount}");