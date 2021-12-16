string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day14\Day14\day14-data.txt";

string polymerTemplate = "";
Dictionary<string, string> instruction = new();
Dictionary<string, long> pairCount = new();
Dictionary<string, long> totals = new();

LoadData();
InitializeCounts();

for (int i = 1; i <= 40; i++)
{
    ProcessInstructions();
}

long maxCount = long.MinValue;
long minCount = long.MaxValue;

string maxChar = "";
string minChar = "";

PerformCount();
DumpCounts(totals);

//PerformCount();

Console.WriteLine($"({minChar}, {minCount}), ({maxChar}, {maxCount}) produces {maxCount - minCount}");

void PerformCount()
{
    foreach (var key in pairCount.Keys)
    {
        string ch = key[0].ToString();
        if (!totals.ContainsKey(ch)) totals.Add(ch, 0L);
        totals[ch] += pairCount[key];
    }

    var finalKey = polymerTemplate[^1].ToString();
    if (!totals.ContainsKey(finalKey)) totals.Add(finalKey, 0L);
    totals[finalKey] += 1L;

    foreach (var key in totals.Keys)
    {
        if (totals[key] > maxCount)
        {
            maxCount = totals[key];
            maxChar = key;
        }

        if (totals[key] < minCount)
        {
            minCount = totals[key];
            minChar = key;
        }
    }
}

void DumpCounts(Dictionary<string, long> pairs)
{
    foreach (var key in pairs.Keys)
    {
        Console.WriteLine($"{key}: {pairs[key]}");
    }
}

void ProcessInstructions()
{
    Dictionary<string, long> passCount = new();
    foreach (var key in instruction.Keys)
    {
        if (pairCount[key] == 0) continue;

        long count = pairCount[key];
        if (!passCount.ContainsKey(key)) passCount.Add(key, 0L);
        passCount[key] += count * -1;

        string[] instructionKey = new[] { $"{key[0]}{instruction[key]}", $"{instruction[key]}{key[1]}" };
        foreach (var k in instructionKey)
        {
            if (!passCount.ContainsKey(k)) passCount.Add(k, 0L);
            passCount[k] += count;
        }
    }

    foreach (var key in passCount.Keys)
    {
        pairCount[key] += passCount[key];
    }
}

void InitializeCounts()
{
    foreach (var key in instruction.Keys)
    {
        pairCount.Add(key, 0L);
    }

    for (int i = 1; i < polymerTemplate.Length; i++)
    {
        string key = $"{polymerTemplate[i - 1]}{polymerTemplate[i]}";
        ++pairCount[key];
    }
}

void LoadData()
{
    bool template = true;
    foreach (var line in File.ReadLines(fileName))
    {
        if (template)
        {
            polymerTemplate = line;
            template = false;
            continue;
        }

        if (line == "") continue;

        instruction.Add(line.Substring(0, 2), line.Substring(6));
    }
}