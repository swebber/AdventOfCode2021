var depthData = File.ReadAllLines(@"C:\Users\WebberS\source\repos\Advent of Code\Day01\Day01\depth-data.txt");
int lastDepth = int.MaxValue;
int count = 0;
foreach (var depth in depthData)
{
    int currentDepth = int.Parse(depth);
    if (currentDepth > lastDepth) ++count;
    lastDepth = currentDepth;
}
Console.WriteLine(count);