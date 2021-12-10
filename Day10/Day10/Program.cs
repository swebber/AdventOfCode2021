string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day10\Day10\day10-data.txt";

var openChars = new[] { '(', '[', '{', '<' };
var closeChars = new[] { ')', ']', '}', '>' };
var points = new[] { 3, 57, 1197, 25137 };

int totalScore = 0;

foreach (var item in File.ReadLines(fileName))
{
    var stack = new Stack<char>();
    foreach (var ch in item)
    {
        if (openChars.Contains(ch)) stack.Push(ch);

        if (closeChars.Contains(ch))
        {
            int x = Array.IndexOf(closeChars, ch);
            char openChar = openChars[x];
            if (stack.Peek() != openChar)
            {
                int score = points[x];
                totalScore += score;
                x = Array.IndexOf(openChars, stack.Peek());
                Console.WriteLine($"{item} - Expected {closeChars[x]}, but found {ch} instead. Points: {score}");
                break;
            }
            stack.Pop();
        }
    }
}

Console.WriteLine();
Console.WriteLine($"Total score: {totalScore}");