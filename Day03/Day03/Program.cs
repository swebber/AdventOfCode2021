using System.Formats.Asn1;
using System.Runtime.CompilerServices;

string fileName = @"C:\Users\WebberS\source\repos\Advent of Code\Day03\Day03\diagnostic-report.txt";

string[]? o2Results = File.ReadAllLines(fileName);
int length = o2Results.Length;

for (int i = 0; i < length; i++)
{
    var highCount = 0;
    var lowCount = 0;

    foreach (var item in o2Results)
    {
        highCount += item[i] == '1' ? 1 : 0;
        lowCount += item[i] == '0' ? 1 : 0;
    }

    char checkBit = highCount >= lowCount ? '1' : '0';
    o2Results = o2Results.Where(it => it[i] == checkBit).ToArray();

    if (o2Results == null) throw new ArgumentOutOfRangeException();
    if (o2Results.Length == 1) break;
}

uint o2Result = ToBinary(o2Results[0]);
Console.WriteLine($"Oxygen Generator Rating: {o2Results[0]} {o2Result}");

string[]? co2Results = File.ReadAllLines(fileName);

for (int i = 0; i < length; i++)
{
    var highCount = 0;
    var lowCount = 0;

    foreach (var item in co2Results)
    {
        highCount += item[i] == '1' ? 1 : 0;
        lowCount += item[i] == '0' ? 1 : 0;
    }

    char checkBit = lowCount <= highCount ? '0' : '1';
    co2Results = co2Results.Where(it => it[i] == checkBit).ToArray();

    if (co2Results == null) throw new ArgumentOutOfRangeException();
    if (co2Results.Length == 1) break;
}

uint co2Result = ToBinary(co2Results[0]);
Console.WriteLine($"CO2 Scrubber Rating: {co2Results[0]} {co2Result}");

Console.WriteLine($"life support rating: {o2Result * co2Result}");

static uint ToBinary(string value)
{
    uint result = 0;
    foreach (var bit in value)
    {
        result = (result << 1) + (uint)int.Parse(bit.ToString());
    }

    return result;
}