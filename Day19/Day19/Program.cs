string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day19\Day19\test-data.txt";

List<Scanner> scannerList = new();
LoadBeacons();
CalculateDistancePairs();

void DistanceOverlap(int scannerId1, int scannerId2)
{
    var s1 = scannerList.First(s => s.Id == scannerId1);
    var s2 = scannerList.First(s => s.Id == scannerId2);

    foreach (var s1Pair in s1.DistancePairs)
    {
        var s2Pair = s2.DistancePairs.First(d => d.Distance == s1Pair.Distance);
        Console.WriteLine($"");

        // s1Pair.Id1 ==> scanner id / beacon id is the same as s2Pair
        // todo: trying to build a list of the overlapping beacons between scanner 1 and scanner 2
        // what data structure am I trying to use
    }
}

void CalculateDistancePairs()
{
    foreach (var scanner in scannerList)
    {
        foreach (var b1 in scanner.Beacons)
        {
            foreach (var b2 in scanner.Beacons)
            {
                if (b1.Id == b2.Id) continue;
                
                scanner.DistancePairs.Add(new BeaconPairDistance
                {
                    Id1 = b1.Id,
                    Id2 = b2.Id,
                    Distance = 
                        FindDifference(b1.X, b2.X) +
                        FindDifference(b1.Y, b2.Y) +
                        FindDifference(b1.Z, b2.Z)
                });
            }
        }
    }
}

int FindDifference(int n1, int n2)
{
    return Math.Abs(n1 - n2);
}

void LoadBeacons()
{
    Scanner scanner = null;
    int beaconId = 0;

    foreach (var line in File.ReadLines(fileName))
    {
        if (string.IsNullOrEmpty(line)) continue;

        if (line.Contains("scanner"))
        {
            string id = line.Replace("--- scanner ", "");
            id = id.Replace(" ---", "");
            scanner = new Scanner(int.Parse(id));
            scannerList.Add(scanner);
            beaconId = 0;
            continue;
        }

        string[] coords = line.Split(new char[] { ',' });
        scanner.Beacons.Add(new Beacon(beaconId++, int.Parse(coords[0]), int.Parse(coords[1]), int.Parse(coords[2])));
    }
}

class Scanner
{
    public int Id { get; set; }
    public List<Beacon> Beacons { get; set; } = new();
    public List<BeaconPairDistance> DistancePairs { get; set; } = new();

    public Scanner(int id)
    {
        Id = id;
    }
}

class BeaconPairDistance
{
    public int Id1 { get; set; }
    public int Id2 { get; set; }
    public int Distance { get; set; }
}

class Beacon
{
    public int Id { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    public Beacon(int id, int x, int y, int z)
    {
        Id = id;
        X = x;
        Y = y;
        Z = z;
    }
}