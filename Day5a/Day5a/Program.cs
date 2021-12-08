using Day5a;

string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day5a\Day5a\day5-data.txt";

string[]? lines = File.ReadAllLines(fileName);
var ventLines = new List<VentLine>();

int rowSize = int.MinValue;
int colSize = int.MinValue;

foreach (var line in lines)
{
    var ventLine = new VentLine(line);
    if (ventLine.IsLineValid())
    {
        ventLines.Add(ventLine);
        int row = ventLine.MaxRow();
        if (row > rowSize) rowSize = row;
        int col = ventLine.MaxColumn();
        if (col > colSize) colSize = col;
    }
}

var ventGrid = new VentGrid(rowSize, colSize);
ventGrid.AddRange(ventLines);

Console.WriteLine($"Overlap count: {ventGrid.OverlapCount()}");
//ventGrid.Dump();
