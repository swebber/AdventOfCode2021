string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day13\Day13\day13-data.txt";

List<List<string>> grid = new();
List<string> folds = new();

InitializeGrid();
LoadGridAndFolds();

foreach (var fold in folds)
{
    Fold(fold);
}

DumpGrid();

int CountDots()
{
    int count = 0;
    foreach (var row in grid)
    {
        foreach (var s in row)
        {
            if (s == "#") ++count;
        }
    }
    return count;
}

void Fold(string fold)
{
    if (fold[0] == 'y')
    {
        int row = int.Parse(fold.Replace("y=", ""));
        FoldUp(row);
    }
    else
    {
        int col = int.Parse(fold.Replace("x=", ""));
        FoldLeft(col);
    }
}

void FoldUp(int y)
{
    int rowOffset = y;
    for (int row = y + 1; row < grid.Count; row++)
    {
        --rowOffset;
        for (int col = 0; col < grid[row].Count; col++)
        {
            if (grid[row][col] == "#")
            {
                grid[rowOffset][col] = "#";
            }
        }
    }

    grid.RemoveRange(y, grid.Count - y);
}

void FoldLeft(int x)
{
    int colOffset = x;
    int colMax = grid[0].Count;

    for (int col = x + 1; col < colMax; col++)
    {
        --colOffset;
        for (int row = 0; row < grid.Count; row++)
        {
            if (grid[row][col] == "#")
            {
                grid[row][colOffset] = "#";
            }
        }
    }

    foreach (var row in grid)
    {
        row.RemoveRange(x, colMax - x);
    }
}

void InitializeGrid()
{
    int xMax = int.MinValue;
    int yMax = int.MinValue;;

    foreach (var line in File.ReadLines(fileName))
    {
        if (line == "") break;

        var pos = line.Split(',').Select<string, int>(int.Parse).ToArray();
        if (pos[0] > yMax) yMax = pos[0];
        if (pos[1] > xMax) xMax = pos[1];
    }

    ++xMax;
    ++yMax;

    for (int x = 0; x < xMax; x++)
    {
        grid.Add(new List<string>());
        for (int y = 0; y < yMax; y++)
        {
            grid[x].Add(".");
        }
    }
}

void DumpGrid()
{
    foreach (var row in grid)
    {
        foreach (var col in row)
        {
            Console.Write($"{col}");
        }

        Console.WriteLine();
    }

    Console.WriteLine();
}

void LoadGridAndFolds()
{
    bool gridInput = true;
    foreach (var line in File.ReadLines(fileName))
    {
        if (line == "")
        {
            gridInput = false;
            continue;
        }

        if (gridInput)
        {
            var pos = line.Split(',').Select<string, int>(int.Parse).ToArray();
            grid[pos[1]][pos[0]] = "#";
        }
        else
        {
            folds.Add(line.Replace("fold along ", ""));
        }
    }
}