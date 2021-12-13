using System.Text.RegularExpressions;

string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day12\Day12\day12-data.txt";

Dictionary<string, Node> network = new();
List<string> paths = new();
string path = "";

network.Add("start", new Node("start"));

foreach (var line in File.ReadLines(fileName))
{
    var nodeName = line.Split('-');
    AddNode(nodeName[0]);
    AddNode(nodeName[1]);
    Connect(nodeName[0], nodeName[1]);
}

WalkNetwork("start", path);

void WalkNetwork(string nodeName, string path)
{
    if (nodeName == "end")
    {
        path += $",{nodeName}";
        paths.Add(path);
        return;
    }

    path += (nodeName == "start") ? nodeName : $",{nodeName}";
    foreach (var node in network[nodeName].Connection)
    {
        if (node.Name == "start") continue;

        if (LargeCave(node.Name))
        {
            WalkNetwork(node.Name, path);
            continue;
        }

        // at this point we are working with a small cave

        if (!PathContainsDoubleSmallCave(path))
        {
            WalkNetwork(node.Name, path);
            continue;
        }

        // the path already contains a double small cave, so no more.

        if (!path.Contains(node.Name))
        {
            WalkNetwork(node.Name, path);
        }
    }
}

//foreach (var item in paths)
//{
//    Console.WriteLine(item);
//}

Console.WriteLine($"There are {paths.Count} paths through this example cave system.");

bool PathContainsDoubleSmallCave(string path)
{
    var nodes = path.Split(',');
    foreach (var node in nodes)
    {
        if (!LargeCave(node))
        {
            int count = Regex.Matches(path, node).Count;
            if (count > 1) return true;
        }
    }
    return false;
}

bool LargeCave(string nodeName)
{
    return Char.IsUpper(nodeName[0]);
}

void AddNode(string nodeName)
{
    if (!network.ContainsKey(nodeName))
        network.Add(nodeName, new Node(nodeName));
}

void Connect(string from, string to)
{
    network[from].Connection.Add(network[to]);
    network[to].Connection.Add(network[from]);
}

class Node
{
    public string Name { get; set; }
    public List<Node> Connection { get; set; } = new();

    public Node(string name)
    {
        Name = name;
    }
}
