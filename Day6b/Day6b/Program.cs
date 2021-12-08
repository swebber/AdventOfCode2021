int days = 256;
string fileName = @"C:\Users\WebberS\source\repos\Advent of Code\Day6b\Day6b\day6-data.txt";

var listOfAges = File.ReadAllLines(fileName);
var oldFish = listOfAges[0].Split(',').Select(int.Parse).ToList();

ulong[] fishTimer = new ulong[9];
foreach (var age in oldFish)
{
    ++fishTimer[age];
}

for (int day = 0; day < days; day++)
{
    ulong newFishCount = fishTimer[0];
    fishTimer[0] = fishTimer[1];
    fishTimer[1] = fishTimer[2];
    fishTimer[2] = fishTimer[3];
    fishTimer[3] = fishTimer[4];
    fishTimer[4] = fishTimer[5];
    fishTimer[5] = fishTimer[6];
    fishTimer[6] = newFishCount + fishTimer[7];
    fishTimer[7] = fishTimer[8];
    fishTimer[8] = newFishCount;
}

ulong totalFish = fishTimer[0] + fishTimer[1] + fishTimer[2] + fishTimer[3] + fishTimer[4] + fishTimer[5] + fishTimer[6] + fishTimer[7] + fishTimer[8];

Console.WriteLine($"Days: {days}, Fish {totalFish}");
