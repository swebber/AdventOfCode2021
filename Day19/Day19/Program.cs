using System.ComponentModel.DataAnnotations;

string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day19\Day19\day19-data.txt";
string input = File.ReadAllText(fileName);

int numberOfBeacons = PartOne(input);
Console.WriteLine($"Part 1, Number of beacons: {numberOfBeacons}");

int largestDistance = PartTwo(input);
Console.WriteLine($"The largest Manhattan distaince is: {largestDistance}");

int PartOne(string input) =>
    LocateScanners(input)
        .SelectMany(scanner => scanner.GetBeaconsInWorld())
        .Distinct()
        .Count();

int PartTwo(string input)
{
    var scanners = LocateScanners(input);
    return (
            from sA in scanners
            from sB in scanners
            where sA != sB
            select Math.Abs(sA.Center.X - sB.Center.X) +
                   Math.Abs(sA.Center.Y - sB.Center.Y) +
                   Math.Abs(sA.Center.Z - sB.Center.Z)
    ).Max();
}

HashSet<Scanner> LocateScanners(string input)
{
    HashSet<Scanner> scanners = new(Parse(input));
    HashSet<Scanner> locateScanners = new();
    Queue<Scanner> q = new();

    // when a scanner is located, it gets into the queue so we can check its neighbors

    locateScanners.Add(scanners.First());
    q.Enqueue(scanners.First());

    while (q.Any())
    {
        var scannerA = q.Dequeue();
        foreach (var scannerB in scanners.ToArray())
        {
            var maybeLocatedScanner = TryToLocate(scannerA, scannerB);
            if (maybeLocatedScanner != null)
            {
                locateScanners.Add(maybeLocatedScanner);
                q.Enqueue(maybeLocatedScanner);

                scanners.Remove(scannerB);
            }
        }
    }

    return locateScanners;
}

Scanner TryToLocate(Scanner scannerA, Scanner scannerB)
{
    var beaconsInA = scannerA.GetBeaconsInWorld().ToArray();

    foreach (var (beaconInA, beaconInB) in PotentialMatchingBeacons(scannerA, scannerB))
    {
        // now try to find the orientation for B
        var rotatedB = scannerB;
        for (int rotation = 0; rotation < 24; rotation++, rotatedB = rotatedB.Rotate())
        {
            // moving the rotated scanner so that beaconA and beaconB overlaps
            // are there 12 matches
            var beaconInRotatedB = rotatedB.Transform(beaconInB);

            var locatedB = rotatedB.Translate(new Coord(
                beaconInA.X - beaconInRotatedB.X,
                beaconInA.Y - beaconInRotatedB.Y,
                beaconInA.Z - beaconInRotatedB.Z
            ));

            if (locatedB.GetBeaconsInWorld().Intersect(beaconsInA).Count() >= 12)
            {
                return locatedB;
            }
        }
    }

    // no luck
    return null;
}

IEnumerable<(Coord beaconInA, Coord beaconInB)> PotentialMatchingBeacons(Scanner scannerA, Scanner scannerB)
{
    // if we had a matching beaconInA and beaconInB and moved the center of the
    // scanners to these then we would find at least 23 beacons with the same
    // coordinates

    // the only problem is that the rotation of scannerB is not fixed yet

    // we need to make our check invariant to that:

    // after the translation, we could form a set from each scanner
    // taking the absolute values of the x, y, and z coordinates of their
    // beacons and compare those

    IEnumerable<int> absCoordinates(Scanner scanner) =>
        from coord in scanner.GetBeaconsInWorld()
        from v in new[] { coord.X, coord.Y, coord.Z }
        select Math.Abs(v);

    // this is the same no patter how we rotate scannerB, so the two sets should
    // have at least 3 * 12 common values (with multiplicity)

    // we can also considerably speed up the search with the pigenhold principle
    // which says that it's enough to take all but 11 beacons from A and B
    // if there is no match amongst those, there cannot be 12 matching pairs

    IEnumerable<T> pick<T>(IEnumerable<T> ts) => ts.Take(ts.Count() - 11);

    foreach (var beaconInA in pick(scannerA.GetBeaconsInWorld()))
    {
        var absA = absCoordinates(
            scannerA.Translate(new Coord(-beaconInA.X, -beaconInA.Y, -beaconInA.Z))
        ).ToHashSet();

        foreach (var beaconInB in pick(scannerB.GetBeaconsInWorld()))
        {
            var absB = absCoordinates(
                scannerB.Translate(new Coord(-beaconInB.X, -beaconInB.Y, -beaconInB.Z))
            );

            if (absB.Count(d => absA.Contains(d)) >= 3 * 12)
            {
                yield return (beaconInA, beaconInB);
            }
        }
    }
}

Scanner[] Parse(string input) => (
    from block in input.Split("\r\n\r\n")
    let beacons =
        from line in block.Split("\r\n").Skip(1)
        let parts = line.Split(",").Select(int.Parse).ToArray()
        select new Coord(parts[0], parts[1], parts[2])
    select new Scanner(new Coord(0, 0, 0), 0, beacons.ToList())
).ToArray();

record Coord(int X, int Y, int Z);

record Scanner(Coord Center, int Rotation, List<Coord> BeaconsInLocal)
{
    public Scanner Rotate() => new Scanner(Center, Rotation + 1, BeaconsInLocal);

    public Scanner Translate(Coord t) => new Scanner(
        new Coord(Center.X + t.X, Center.Y + t.Y, Center.Z + t.Z), Rotation, BeaconsInLocal);

    public Coord Transform(Coord coord)
    {
        var (x, y, z) = coord;

        // rotate the coordinate system so that the x-axis points in the 6 possible directions
        switch (Rotation % 6)
        {
            case 0: (x, y, z) = (x, y, z); break;
            case 1: (x, y, z) = (-x, y, -z); break;
            case 2: (x, y, z) = (y, -x, z); break;
            case 3: (x, y, z) = (-y, x, z); break;
            case 4: (x, y, z) = (z, y, -x); break;
            case 5: (x, y, z) = (-z, y, x); break;
        }

        switch ((Rotation / 6) % 4)
        {
            case 0: (x, y, z) = (x, y, z); break;
            case 1: (x, y, z) = (x, -z, y); break;
            case 2: (x, y, z) = (x, -y, -z); break;
            case 3: (x, y, z) = (x, z, -y); break;

        }

        return new Coord(Center.X + x, Center.Y + y, Center.Z + z);
    }

    public IEnumerable<Coord> GetBeaconsInWorld()
    {
        return BeaconsInLocal.Select(Transform);
    }
}