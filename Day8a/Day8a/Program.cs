using System.ComponentModel;
using System.Security.Cryptography;
using Microsoft.VisualBasic;

int[] digits = new int[] { 6, 2, 5, 5, 4, 5, 6, 3, 7, 6 };
string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day8a\Day8a\day8-data.txt";

var displayLines = File.ReadAllLines(fileName);
int totalOutput = 0;

foreach (var displayLine in displayLines)
{
    var segments = displayLine.Split(" | ");
    var signalPatterns = segments[0].Split(" ").ToList();
    var outputValues = segments[1].Split(" ").ToList();

    var results = InitResults(signalPatterns);

    Patterns(signalPatterns, out string top, out string right, out string upperLeft, out string lowerLeft);

    Fives(signalPatterns, top, upperLeft, lowerLeft, results);
    Sixes(signalPatterns, top, right, upperLeft, lowerLeft, results);

    string answer = "";
    foreach (var output in outputValues)
    {
        for (int i = 0; i < results.Length; i++)
        {
            if (output.Length == results[i].Length && Contains(output, results[i]))
            {
                answer += i.ToString();
                break;
            }
        }
    }

    //foreach (var output in outputValues)
    //{
    //    Console.Write($"{output} ");
    //}

    totalOutput += int.Parse(answer);
    //Console.WriteLine($": {totalOutput}");

    /*
     * 5 digit numbers == 2,3,5
       6 digit numbers == 0,6,9
       
       acedgfb:8 
       cdfbe:5 
       gcdfa:2
       fbcad:3
       dab:7 
       cefabd:9 
       cdfgeb:6
       eafb:4 
       cagedb:0 
       ab:1 
       |
       cdfeb:5 fcadb:3 cdfeb:5 cdbaf:3
       
       0:dd
       1:ef      2:ab
       3:ef
       4:cg      5:ab
       6:cg
     */
}

Console.WriteLine($"Output total: {totalOutput}");

void Sixes(List<string> signalPatterns, string top, string right, string upperLeft, string lowerLeft, string[] results)
{
    var sixes = new Dictionary<int, List<string>>
    {
        { 0, signalPatterns.Where(it => it.Length == 6).ToList() },
        { 6, signalPatterns.Where(it => it.Length == 6).ToList() },
        { 9, signalPatterns.Where(it => it.Length == 6).ToList() }
    };

    var patterns = signalPatterns.Where(it => it.Length == 6).ToList();
    foreach (var pattern in patterns)
    {
        bool valid = Contains(pattern, top) && Contains(pattern, lowerLeft) && Contains(pattern, right);
        if (!valid)
            sixes[0].Remove(pattern);

        valid = Contains(pattern, top) && Contains(pattern, upperLeft) && Contains(pattern, lowerLeft);
        if (!valid)
            sixes[6].Remove(pattern);

        valid = Contains(pattern, top) && Contains(pattern, upperLeft) && Contains(pattern, right);
        if (!valid)
            sixes[9].Remove(pattern);
    }

    if (sixes[0].Count == 1)
    {
        sixes[6].Remove(sixes[0][0]);
        sixes[9].Remove(sixes[0][0]);
    }
    if (sixes[6].Count == 1)
    {
        sixes[0].Remove(sixes[6][0]);
        sixes[9].Remove(sixes[6][0]);
    }
    if (sixes[9].Count == 1)
    {
        sixes[0].Remove(sixes[9][0]);
        sixes[6].Remove(sixes[9][0]);
    }

    if (sixes[0].Count != 1 || sixes[6].Count != 1 || sixes[9].Count != 1)
        throw new ArgumentOutOfRangeException();

    results[0] = sixes[0][0];
    results[6] = sixes[6][0];
    results[9] = sixes[9][0];
}

void Fives(List<string> signalPatterns, string top, string upperLeft, string lowerLeft, string[] results)
{
    var fives = new Dictionary<int, List<string>>
    {
        { 2, signalPatterns.Where(it => it.Length == 5).ToList() },
        { 3, signalPatterns.Where(it => it.Length == 5).ToList() },
        { 5, signalPatterns.Where(it => it.Length == 5).ToList() }
    };

    var patterns = signalPatterns.Where(it => it.Length == 5).ToList();
    foreach (var pattern in patterns)
    {
        bool valid = Contains(pattern, top) && Contains(pattern, lowerLeft);
        if (!valid)
            fives[2].Remove(pattern);

        valid = Contains(pattern, top) && Contains(pattern, upperLeft);
        if (!valid)
            fives[5].Remove(pattern);
    }

    if (fives[2].Count == 1)
    {
        fives[3].Remove(fives[2][0]);
        fives[5].Remove(fives[2][0]);
    }
    if (fives[3].Count == 1)
    {
        fives[2].Remove(fives[3][0]);
        fives[5].Remove(fives[3][0]);
    }
    if (fives[5].Count == 1)
    {
        fives[2].Remove(fives[5][0]);
        fives[3].Remove(fives[5][0]);
    }

    if (fives[2].Count != 1 || fives[3].Count != 1 || fives[5].Count != 1)
        throw new ArgumentOutOfRangeException();

    results[2] = fives[2][0];
    results[3] = fives[3][0];
    results[5] = fives[5][0];
}

string[] InitResults(List<string> signalPatterns)
{
    var results = new string[10];

    results[1] = signalPatterns.Single(it => it.Length == 2);
    results[4] = signalPatterns.Single(it => it.Length == 4);
    results[7] = signalPatterns.Single(it => it.Length == 3);
    results[8] = signalPatterns.Single(it => it.Length == 7);
    
    return results;
}

bool Contains(string signalPattern, string value)
{
    foreach (var ch in value)
    {
        if (!signalPattern.Contains(ch))
            return false;
    }

    return true;
}

void Patterns(List<string> signalPatterns, out string top, out string right, out string upperLeft, out string lowerLeft)
{
    right = signalPatterns.Single(it => it.Length == 2);

    top = signalPatterns.Single(it => it.Length == 3);
    top = right.Aggregate(top, (current, ch) => current.Replace(ch.ToString(), ""));

    upperLeft = signalPatterns.Single(it => it.Length == 4);
    upperLeft = right.Aggregate(upperLeft, (current, ch) => current.Replace(ch.ToString(), ""));

    lowerLeft = signalPatterns.Single(it => it.Length == 7);
    lowerLeft = right.Aggregate(lowerLeft, (current, ch) => current.Replace(ch.ToString(), ""));
    lowerLeft = top.Aggregate(lowerLeft, (current, ch) => current.Replace(ch.ToString(), ""));
    lowerLeft = upperLeft.Aggregate(lowerLeft, (current, ch) => current.Replace(ch.ToString(), ""));
}
