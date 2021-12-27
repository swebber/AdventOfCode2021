string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day22\Day22\test2-data.txt";

List<Cube> cubes = RebootSteps();

List<Cube> RebootSteps()
{
    List<Cube> cubes = new();

    foreach (var line in File.ReadLines(fileName))
    {
        bool cubeIsOn =  line.StartsWith("on");

        string[] commandPart = line[line.IndexOf(" ")..].Split(',');

        string[] part = commandPart[0].Split(new string[] {"=",".."}, StringSplitOptions.None);
        int x1 = int.Parse(part[1]);
        int x2 = int.Parse(part[2]);

        part = commandPart[1].Split(new string[] {"=",".."}, StringSplitOptions.None);
        int y1 = int.Parse(part[1]);
        int y2 = int.Parse(part[2]);

        part = commandPart[2].Split(new string[] {"=",".."}, StringSplitOptions.None);
        int z1 = int.Parse(part[1]);
        int z2 = int.Parse(part[2]);

        var cube = new Cube(x1, x2, y1, y2, z1, z2, cubeIsOn);

        if (!CubeInRange(cube)) continue;

        cubes.Add(cube);
    }

    return cubes;
}

bool CubesIntersect(Cube a, Cube b)
{
    // if A's left face is to the right of B's right face
    // then A is to the right of B
    if (a.X2 < b.X1) return false;

    // if A's right face is to the left of B's left face
    // then A is to the left of B
    if (b.X2 < a.X1) return false;

    return true;
}

bool CubeInRange(Cube cube)
{
    if (cube.X1 < -50 || cube.X1 > 50)
        return false;

    return true;
}

internal class Step
{
    public int X1 { get; set; }
    public int X2 { get; set; }
    public int Y1 { get; set; }
    public int Y2 { get; set; }
    public int Z1 { get; set; }
    public int Z2 { get; set; }
    public bool On { get; set; }
}

public class Cube
{
    public int X1 { get; }
    public int X2 { get; }
    public int Y1 { get; }
    public int Y2 { get; }
    public int Z1 { get; }
    public int Z2 { get; }
    public bool On { get; set; }

    public Cube(int x1, int x2, int y1, int y2, int z1, int z2, bool cubeIsOn)
    {
        X1 = x1;
        X2 = x2;
        Y1 = y1;
        Y2 = y2;
        Z1 = z1;
        Z2 = z2;
        On = cubeIsOn;
    }

}