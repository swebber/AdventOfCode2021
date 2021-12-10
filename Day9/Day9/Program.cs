using System.Drawing;

string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day9\Day9\day9-data.txt";

var depthData = new List<int[]>();
var basins = new List<Point>();
var basinSizes = new List<int>();

var lines = File.ReadLines(fileName);
foreach (var line in lines)
{
    depthData.Add(line.Select(s => int.Parse(s.ToString())).ToArray());
}

int count;
int dataHeight = depthData.Count;
int dataWidth = depthData[0].Length;

for (int row = 0; row < depthData.Count; row++)
{
    for (int col = 0; col < depthData[row].Length; col++)
    {
        if (IsPointLow(row, col)) basins.Add(new Point(col, row));
    }
}

var basinPoints = new List<Point>();

foreach (var basin in basins)
{
    count = 0;
    basinPoints.Clear();
    WalkBasin(basin);
    basinSizes.Add(basinPoints.Count);
}

int max = 1;

for (int i = 0; i < 3; i++)
{
    // https://stackoverflow.com/a/1143552
    int index =
        basinSizes
            .Select((value, index) => new { Value = value, Index = index })
            .Aggregate((a, b) => (a.Value > b.Value) ? a : b)
            .Index;

    // or since i have no idea what the above does...
    index = basinSizes.IndexOf(basinSizes.Max());

    max *= basinSizes[index];
    basinSizes.RemoveAt(index);
}

Console.WriteLine($"Product of three largest basins: {max}");

void WalkBasin(Point location)
{
    if (basinPoints.Contains(location)) return;

    // How much do I trust my recursive code?
    if (++count > 1000) throw new Exception();

    int depth = GetDepthAtPoint(location);
    if (depth < 9) basinPoints.Add(location);

    int row = location.Y;
    int col = location.X;

    // look right - row, col + 1
    var point = new Point(col + 1, row);
    depth = GetDepthAtPoint(point);
    if (depth < 9) WalkBasin(point);

    // look down - row + 1, col
    point = new Point(col, row + 1);
    depth = GetDepthAtPoint(point);
    if (depth < 9) WalkBasin(point);

    // look left - row, col - 1
    point = new Point(col - 1, row);
    depth = GetDepthAtPoint(point);
    if (depth < 9) WalkBasin(point);

    // look up - row - 1, col
    point = new Point(col, row - 1);
    depth = GetDepthAtPoint(point);
    if (depth < 9) WalkBasin(point);
}

int GetDepthAtPoint(Point point)
{
    return GetDepth(point.Y, point.X);
}

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