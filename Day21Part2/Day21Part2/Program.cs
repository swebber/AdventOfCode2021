var watch = System.Diagnostics.Stopwatch.StartNew();

string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day21Part2\Day21Part2\day21-data.txt";

DracPair[] dracPair =
{
    new DracPair(3,1),
    new DracPair(4,3),
    new DracPair(5,6),
    new DracPair(6,7),
    new DracPair(7,6),
    new DracPair(8,3),
    new DracPair(9,1)
};

LoadPlayers(out int playerOnePosition, out int playerTwoPosition);
long[] wins = Play(playerOnePosition, playerTwoPosition, 0, 0);

Console.WriteLine($"Player 1 wins: {wins[0]}, Player 2 wins: {wins[1]}");

watch.Start();
Console.WriteLine($"Elapsed seconds: {watch.Elapsed.TotalSeconds:F}");

long[] Play(int playerPosition, int otherPosition, long playerScore, long otherScore)
{
    if (playerScore >= 21) return new[] { 1L, 0L };
    if (otherScore >= 21) return new[] { 0L, 1L };

    long totalPlayerWins = 0L;
    long totalOtherWins = 0L;

    foreach (var pair in dracPair)
    {
        int newPosition = (playerPosition + pair.Sum) % 10;
        long newScore = playerScore + newPosition + 1;

        // pass the turn to the other player
        long[] playWins = Play(otherPosition, newPosition, otherScore, newScore);

        totalPlayerWins += playWins[1] * pair.Count;
        totalOtherWins += playWins[0] * pair.Count;
    }

    return new[] { totalPlayerWins, totalOtherWins };
}

void LoadPlayers(out int p1, out int p2)
{
    var lines = File.ReadAllLines(fileName);

    p1 = int.Parse(lines[0][(lines[0].LastIndexOf(':') + 1)..]) - 1;
    p2 = int.Parse(lines[1][(lines[1].LastIndexOf(':') + 1)..]) - 1;
}

record DracPair(int Sum, long Count)
{
    public int Sum = Sum;
    public long Count = Count;
}
