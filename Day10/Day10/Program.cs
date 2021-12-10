string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day10\Day10\day10-data.txt";

var openChars = new[] { '(', '[', '{', '<' };
var closeChars = new[] { ')', ']', '}', '>' };
var points = new[] { 1, 2, 3, 4 };
var scores = new List<ulong>();

foreach (var line in File.ReadLines(fileName))
{
    var stack = new Stack<char>();
    bool incompleteLine = true;

    foreach (var ch in line)
    {
        if (openChars.Contains(ch)) stack.Push(ch);

        if (closeChars.Contains(ch))
        {
            int x = Array.IndexOf(closeChars, ch);
            char openChar = openChars[x];
            if (stack.Peek() != openChar)
            {
                incompleteLine = false;
                break;
            }
            stack.Pop();
        }
    }

    if (incompleteLine)
    {
        string remaining = "";
        ulong score = 0;
        while (stack.Count > 0)
        {
            char ch = stack.Pop();
            int x = Array.IndexOf(openChars, ch);
            score = (score * 5) + (ulong)points[x];
            remaining += closeChars[x];
        }

        Console.WriteLine($"{line} - Complete by adding {remaining}. Score {score}");
        scores.Add(score);
    }
}

scores = scores.OrderBy(s => s).ToList();
int y = (scores.Count / 2);

Console.WriteLine();
Console.WriteLine($"The middle score is: {scores[y]}");