using System.Data;
using System.Security.Cryptography.X509Certificates;

string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day11\Day11\day11-data.txt";

var octopuses = new List<int[]>();
foreach (var line in File.ReadLines(fileName))
{
    octopuses.Add(line.Select(s => int.Parse(s.ToString())).ToArray());
}

int totalFlashes = 0;

//Console.WriteLine("Before any steps:");
//DumpBoard();
//Console.ReadKey();

int step = 0;
bool done = false;
while (!done)
{
    ++step;

    IncreaseEnergyLevel();
    CheckOctopuses();
    
    if (PowerLevelZero())
    {
        done = true;
        Console.WriteLine($"What is the first step during which all octopuses flash? {step}");
        break;
    }

    //Console.WriteLine($"After step: {i + 1}");
    //DumpBoard();
    //Console.ReadLine();
}

//Console.WriteLine($"There have been a total of {totalFlashes} flashes.");

bool PowerLevelZero()
{
    foreach (var row in octopuses)
    {
        foreach (var powerLevel in row)
        {
            if (powerLevel != 0) return false;
        }
    }
    return true;
}

void CheckOctopuses()
{
    int flashCount = int.MinValue;
    while (flashCount != 0)
    {
        flashCount = 0;
        for (int row = 0; row < octopuses.Count; row++)
        {
            for (int col = 0; col < octopuses[row].Length; col++)
            {
                bool flashed = CheckOctopus(row, col);
                if (flashed)
                {
                    ++flashCount;
                    ++totalFlashes;
                }
            }
        }
    }
}

bool CheckOctopus(int row, int col)
{
    if (WeAreOffTheGrid(row, col)) return false;

    // skip the check if the octopus isn't powered up
    if (octopuses[row][col] <= 9) return false;

    octopuses[row][col] = 0;

    // increment surrounding
    IncrementOctopus(row, col + 1);
    IncrementOctopus(row + 1, col + 1);
    IncrementOctopus(row + 1, col);
    IncrementOctopus(row + 1, col - 1);
    IncrementOctopus(row, col - 1);
    IncrementOctopus(row - 1, col - 1);
    IncrementOctopus(row - 1, col);
    IncrementOctopus(row - 1, col + 1);

    return true;
}

void IncrementOctopus(int row, int col)
{
    if (WeAreOffTheGrid(row, col)) return;
    if (octopuses[row][col] > 0)
        ++octopuses[row][col];
}

bool WeAreOffTheGrid(int row, int col)
{
    if (row < 0 || row >= octopuses.Count) return true;
    if (col < 0 || col >= octopuses[row].Length) return true;
    return false;
}

void IncreaseEnergyLevel()
{
    foreach (var row in octopuses)
    {
        for (int col = 0; col < row.Length; col++)
        {
            ++row[col];
        }
    }
}

void DumpBoard()
{
    for (int x = 0; x < octopuses.Count; x++)
    {
        for (int y = 0; y < octopuses[x].Length; y++)
        {
            bool setColor = octopuses[x][y] == 0;
            if (setColor) Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{octopuses[x][y]}");
            if (setColor) Console.ResetColor();
        }

        Console.WriteLine();
    }

    Console.WriteLine();
}