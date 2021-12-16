string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day14\Day14\day14-data.txt";

bool template = true;
string polymerTemplate = "";
var pairInsertion = new Dictionary<string, string>();

LoadData();

for (int i = 1; i <= 10; i++)
{
    polymerTemplate = ProcessInsertion();
    Console.WriteLine($"After step {i}: {polymerTemplate.Length}");
}

int maxCount = int.MinValue;
int minCount = int.MaxValue;

char maxChar = ' ';
char minChar = ' ';

PerformCount();

Console.WriteLine($"({minChar}, {minCount}), ({maxChar}, {maxCount}) produces {maxCount - minCount}");

void PerformCount()
{
    var distinctCharacters = polymerTemplate.Distinct().ToArray();
    foreach (var c in distinctCharacters)
    {
        int count = polymerTemplate.Count(it => it == c);
        if (count > maxCount)
        {
            maxCount = count;
            maxChar = c;
        }

        if (count < minCount)
        {
            minCount = count;
            minChar = c;
        }
    }
}

string ProcessInsertion()
{
    string input = polymerTemplate;
    string output = "";

    for (int i = 1; i < input.Length; i++)
    {
        output += input[i - 1];

        string key = input.Substring(i - 1, 2);
        if (pairInsertion.ContainsKey(key))
        {
            output += pairInsertion[key];
        }
    }

    return output += input.Last();
}

void LoadData()
{
    foreach (var line in File.ReadLines(fileName))
    {
        if (template)
        {
            polymerTemplate = line;
            template = false;
            continue;
        }

        if (line == "") continue;

        pairInsertion.Add(line.Substring(0, 2), line.Substring(6));
    }
}