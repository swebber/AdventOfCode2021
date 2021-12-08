using Day4a;

string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day4a\Day4a\day4-data.txt";

var bingoData = File.ReadAllLines(fileName);
var moves = bingoData[0].Split(',');
var boardCount = (bingoData.Length - 1) / 6;

var dataRow = 1;
var boards = new List<Board>();
for (int i = 0; i < boardCount; i++)
{
    boards.Add(new Board(bingoData, dataRow));
    dataRow += 6;
}

foreach (var move in moves)
{
    foreach (var board in boards)
    {
        var score = board.Play(move);
        if (score != null)
        {
            if (--boardCount == 0)
            {
                Console.WriteLine($"Score: {score}");
                board.Dump();
                return;
            }
        }
    }
}

foreach (var board in boards)
{
    board.Dump();
    Console.WriteLine();
}