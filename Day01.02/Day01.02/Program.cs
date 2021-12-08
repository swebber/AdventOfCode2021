var depthData = File.ReadAllLines(@"C:\Users\WebberS\source\repos\AdventOfCode2021\Day01\Day01\depth-data.txt");
int lastWindow = int.MaxValue;
int count = 0;
for (int i = 2; i < depthData.Length; i++)
{
    int currentWindow = 
        int.Parse(depthData[i - 2]) +
        int.Parse(depthData[i - 1]) +
        int.Parse(depthData[i]);

    if (currentWindow > lastWindow) ++count;
    lastWindow = currentWindow;
}
Console.WriteLine(count);