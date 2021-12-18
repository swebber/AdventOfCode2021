using System.Drawing;
using System.Security.Cryptography.X509Certificates;

string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day17\Day17\day17-data.txt";
string input = File.ReadAllText(fileName);

var targetArea = ParseInput();

int maxHeight = int.MinValue;
int velocityCount = 0;
for (int x = -500; x < 500; x++)
{
    for (int y = -500; y < 500; y++)
    {
        var onTarget = MyShot(new Point(0, 0), x, y);
        if (onTarget)
        {
            //Console.WriteLine($"({x}, {y}), ... Max Y: {maxHeight}");
            ++velocityCount;
        }
    }
}

Console.WriteLine(velocityCount);

bool MyShot(Point pt, int xVelocity, int yVelocity)
{
    int currentMaxHeight = int.MinValue;
    while (!targetArea.MissedTarget(pt, xVelocity, yVelocity))
    {
        //Console.WriteLine($"({pt.X},{pt.Y})");

        if (pt.Y > currentMaxHeight)
            currentMaxHeight = pt.Y;

        if (targetArea.OnTarget(pt))
        {
            if (currentMaxHeight > maxHeight) maxHeight = currentMaxHeight;
            return true;
        }

        pt.X += xVelocity > 0 ? xVelocity-- : 0;
        pt.Y += yVelocity--;
    }

    return false;
}

TargetArea ParseInput()
{
    string result = input.Replace("target area: x=", "");
    result = result.Replace("..", ",");
    result = result.Replace(" y=", "");

    string[] coordinates = result.Split(new char[] { ',' });

    return new TargetArea(
        int.Parse(coordinates[0]), int.Parse(coordinates[3]),
        int.Parse(coordinates[1]), int.Parse(coordinates[2]));
}

class TargetArea
{
    private readonly Point _topLeft;
    private readonly Point _bottomRight;

    public TargetArea(int x1, int y1, int x2, int y2)
    {
        _topLeft = new Point(x1, y1);
        _bottomRight = new Point(x2, y2);
    }

    public int Top => _topLeft.Y;
    public int Right => _bottomRight.X;
    public int Bottom => _bottomRight.Y;
    public int Left => _topLeft.X;

    public bool OnTarget(Point pt)
    {
        //Console.WriteLine($"X between: {_topLeft.X} <= {pt.X} <= {_bottomRight.X}");
        //Console.WriteLine($"Y between: {_topLeft.Y} >= {pt.Y} >= {_bottomRight.Y}");

        bool result = 
            Left <= pt.X && pt.X <= Right &&
            Top >= pt.Y && pt.Y >= Bottom;

        return result;
    }

    public bool MissedTarget(Point position, int xVelocity, int yVelocity)
    {
        //Console.WriteLine($"X velocity: {xVelocity}, Y velocity {yVelocity}");

        //if (xVelocity > 0 && yVelocity > 0) return false;

        //Console.WriteLine($"X: {position.X} > {_bottomRight.X}");
        //Console.WriteLine($"Y: {position.Y} > {_bottomRight.Y}");

        bool result = 
            position.X > Right || position.Y < Bottom;

        //Console.WriteLine($"({position.X}, {position.Y})");

        return result;
    }
}