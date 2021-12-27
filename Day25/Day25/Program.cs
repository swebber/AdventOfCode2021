string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day25\Day25\day25-data.txt";

var grid = File.ReadLines(fileName).Select(line => line.ToCharArray()).ToList();

int step = 0;
int height = grid.Count;
int width = grid[0].Length;

//DumpGrid("Initial state:");
while (true)
{
    step++;

    bool hasRightMove = MoveRight();
    bool hasDownMove = MoveDown();
    //DumpGrid($"After {i} step(s):");

    if (!hasRightMove && !hasDownMove) break;
}

Console.WriteLine($"Steps until gridlock: {step}");

bool MoveDown()
{
    List<(int, int)> moves = new();
    for (int col = 0; col < width; col++)
    {
        for (int row = 0; row < height; row++)
        {
            if (grid[row][col] == 'v')
            {
                int oneDown = row == height - 1 ? 0 : row + 1;
                if (grid[oneDown][col] == '.')
                {
                    moves.Add((row, col));
                }
            }
        }
    }

    foreach ((int row, int col) in moves)
    {
        int oneDown = row == height - 1 ? 0 : row + 1;
        grid[row][col] = '.';
        grid[oneDown][col] = 'v';
    }

    return moves.Any();
}

bool MoveRight()
{
    List<(int, int)> moves = new();
    for (int row = 0; row < height; row++)
    {
        for (int col = 0; col < width; col++)
        {
            if (grid[row][col] == '>')
            {
                int oneRight = col == width - 1 ? 0 : col + 1;
                if (grid[row][oneRight] == '.')
                {
                    moves.Add((row, col));
                }
            }
        }
    }

    foreach ((int row, int col) in moves)
    {
        int oneRight = col == width - 1 ? 0 : col + 1;
        grid[row][col] = '.';
        grid[row][oneRight] = '>';
    }

    return moves.Any();
}

void DumpGrid(string msg = null)
{
    if (msg != null) Console.WriteLine(msg);
    for (int row = 0; row < height; row++)
    {
        for (int col = 0; col < width; col++)
        {
            Console.Write(grid[row][col]);
        }

        Console.WriteLine(); 
    }
    Console.WriteLine();
}