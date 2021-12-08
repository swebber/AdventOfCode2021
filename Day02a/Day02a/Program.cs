var courseData = File.ReadAllLines(@"C:\Users\WebberS\source\repos\AdventOfCode2021\Day02a\Day02a\pilot-course.txt");
int horizontalPosition = 0;
int depth = 0;
int aim = 0;
foreach (var course in courseData)
{
    var command = course.Split(' ');
    int distance = int.Parse(command[1]);
    switch (command[0])
    {
        case "forward":
            horizontalPosition += distance;
            depth += aim * distance;
            break;
        case "down":
            aim += distance;
            break;
        case "up":
            aim -= distance;
            break;
    }
}
Console.WriteLine(horizontalPosition * depth);