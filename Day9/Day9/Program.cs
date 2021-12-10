using System.Text;

string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day9\Day9\day9-data.txt";

var depthData = new List<int[]>();

var lines = File.ReadLines(fileName);
foreach (var line in lines)
{
    depthData.Add(line.Select(s => int.Parse(s.ToString())).ToArray());
}

int dataHeight = depthData.Count;
int dataWidth = depthData[0].Length;

var total = 0;
for (int row = 0; row < depthData.Count; row++)
{
    for (int col = 0; col < depthData[row].Length; col++)
    {
        var depth = GetDepth(row, col);
        if (IsPointLow(row, col)) total += depth + 1;
    }
}

Console.WriteLine($"Sum of risk from low points: {total}");

int GetDepth(int row, int col)
{
    if (row < 0 || col < 0 || row >= dataHeight || col >= dataWidth) return int.MaxValue;
    return depthData[row][col];
}

bool IsPointLow(int row, int col)
{
    var depth = GetDepth(row, col);

    if (depth >= GetDepth(row, col + 1)) return false;
    if (depth >= GetDepth(row + 1, col + 1)) return false;
    if (depth >= GetDepth(row + 1, col)) return false;
    if (depth >= GetDepth(row + 1, col - 1)) return false;
    if (depth >= GetDepth(row, col - 1)) return false;
    if (depth >= GetDepth(row - 1, col - 1)) return false;
    if (depth >= GetDepth(row - 1, col)) return false;
    if (depth >= GetDepth(row - 1 , col + 1)) return false;
    
    return true;
}