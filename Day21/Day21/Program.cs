using Microsoft.VisualBasic;

string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day21\Day21\day21-data.txt";

int lastRoll = 0;
int rollCount = 0;
int losingPlayer = 0;

List<Player> player = new();

LoadPlayers();

int x = 0;
while (true)
{
    int r1 = RollDeterministicDice();
    int r2 = RollDeterministicDice();
    int r3 = RollDeterministicDice();

    Console.Write($"Player {player[x].Name} rolls {r1}+{r2}+{r3} ");
    player[x].Move(r1 + r2 + r3);

    if (player[x].Score >= 1000)
    {
        losingPlayer = (x + 1) % 2;
        break;
    }
    x = ++x % 2;
}

Console.WriteLine($"{rollCount} * {player[losingPlayer].Score} = {rollCount * player[losingPlayer].Score}");

int RollDeterministicDice()
{
    ++rollCount;
    return (lastRoll++ % 100) + 1;
}

void LoadPlayers()
{
    player.Clear();

    foreach (var line in File.ReadLines(fileName))
    {
        var sp = line.Replace("Player ", "");
        sp = sp.Replace(" starting position: ", ",");
        var playerInfo = sp.Split(',');
        player.Add(new Player(playerInfo[0], int.Parse(playerInfo[1])));
    }
}

class Player
{
    public string Name { get; set; }
    public int BoardPosition { get; set; }
    public long Score { get; set; } = 0L;

    public Player(string name, int startingPosition)
    {
        Name = name;
        BoardPosition = startingPosition;
    }

    public void Move(int spaces)
    {
        BoardPosition += spaces;
        while (BoardPosition > 10) BoardPosition -= 10;
        Score += BoardPosition;
        Console.WriteLine($"and moves to space {BoardPosition} for a total score to {Score}.");
    }
}