string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day15\Day15\day15-data.txt";

int width = 0;
int height = 0;
InitHeightAndWidth();

var field = new Point[height,width];
LoadField();

bool updated = true;
while (updated)
{
    updated = WalkField();
}

Console.WriteLine(field[height - 1, width - 1].Depth);

bool WalkField()
{
    bool updated = false;

    for (int row = 0; row < height; row++)
    {
        //Console.Write($"{row}: ");
        for (int col = 0; col < width; col++)
        {
            //Console.Write(".");
            if (row == 0 && col == 0) continue;
            if (WalkPoint(row, col)) updated = true;
        }
        //Console.WriteLine();
    }

    return updated;
}

bool WalkPoint(int row, int col)
{
    bool updated = false;

    // row - 1, col
    if (row - 1 >= 0)
    {
        if (field[row - 1, col].Depth != long.MaxValue)
        {
            if (field[row - 1, col].Depth + field[row, col].Value < field[row, col].Depth)
            {
                field[row, col].Depth = field[row - 1, col].Depth + field[row, col].Value;
                updated = true;
            }
        }
    }

    // row, col + 1
    if (col + 1 < width)
    {
        if (field[row, col + 1].Depth != long.MaxValue)
        {
            if (field[row, col + 1].Depth + field[row, col].Value < field[row, col].Depth)
            {
                field[row, col].Depth = field[row, col + 1].Depth + field[row, col].Value;
                updated = true;
            }
        }
    }

    // row + 1, col
    if (row + 1 < height)
    {
        if (field[row + 1, col].Depth != long.MaxValue)
        {
            if (field[row + 1, col].Depth + field[row, col].Value < field[row, col].Depth)
            {
                field[row, col].Depth = field[row + 1, col].Depth + field[row, col].Value;
                updated = true;
            }
        }
    }

    // row, col - 1
    if (col - 1 >= 0)
    {
        if (field[row, col - 1].Depth != long.MaxValue)
        {
            if (field[row, col - 1].Depth + field[row, col].Value < field[row, col].Depth)
            {
                field[row, col].Depth = field[row, col - 1].Depth + field[row, col].Value;
                updated = true;
            }
        }
    }

    return updated;
}

void LoadField()
{
    int row = 0;
    foreach (var line in File.ReadLines(fileName))
    {
        int col = 0;
        foreach (var ch in line)
        {
            field[row, col] = new Point(int.Parse(ch.ToString()), long.MaxValue);
            ++col;
        }
        ++row;
    }

    field[0, 0].Depth = 0L;
}

void InitHeightAndWidth()
{
    foreach (var line in File.ReadLines(fileName))
    {
        ++height;
        if (line.Length > width) width = line.Length;
    }
}

internal class Point
{
    public long Depth { get; set; }
    public int Value { get; set; }

    public Point(int value, long depth = 0L)
    {
        Depth = depth;
        Value = value;
    }
}