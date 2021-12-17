string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day15\Day15\day15-data.txt";

int width = 0;
int height = 0;
InitHeightAndWidth();

var field = new Point[height,width];
LoadField();
ExtendField();

//DumpField();

bool updated = true;
while (updated)
{
    updated = WalkField();
}

Console.WriteLine(field[height - 1, width - 1].Depth);

void DumpField()
{
    for (int row = 0; row < height; row++)
    {
        for (int col = 0; col < width; col++)
        {
            Console.Write(field[row, col].Value);
        }
        Console.WriteLine();
    }
}

void ExtendField()
{
    int efWidth = width / 5;
    int efHeight = height / 5;

    for (int x = 1; x < 5; x++)
    {
        int row1 = 0;
        int row2 = efHeight;
        int col1 = efWidth * (x - 1);
        int col2 = col1 + efWidth;
        int rightOffset = efWidth;
        ExtendRight(row1, col1, row2, col2, rightOffset);
    }

    for (int x = 1; x < 5; x++)
    {
        int row1 = efHeight * (x - 1);
        int row2 = row1 + efHeight;
        int col1 = 0;
        int col2 = width;
        int downOffset = efHeight;
        ExtendDown(row1, col1, row2, col2, downOffset);
    }
}

void ExtendRight(int row1, int col1, int row2, int col2, int offset)
{
    for (int row = row1; row < row2; row++)
    {
        for (int col = col1; col < col2; col++)
        {
            int value = field[row, col].Value + 1;
            if (value > 9) value = 1;
            field[row, col + offset] = new Point(value, long.MaxValue);
        }
    }
}

void ExtendDown(int row1, int col1, int row2, int col2, int offset)
{
    for (int row = row1; row < row2; row++)
    {
        for (int col = col1; col < col2; col++)
        {
            int value = field[row, col].Value + 1;
            if (value > 9) value = 1;
            field[row + offset, col] = new Point(value, long.MaxValue);
        }
    }
}

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

    height *= 5;
    width *= 5;
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