string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day18a\Day18a\day18-data.txt";

LinkedList<Item> number = new();
Part2();

void Part2()
{
    int max = int.MinValue;

    var lines = File.ReadAllLines(fileName);
    foreach (var line1 in lines)
    {
        foreach (var line2 in lines)
        {
            number.Clear();
            number = Parse(line1);
            Add(Parse(line2));
            Reduce();
            Magnitude();
            if (number.First.Value.Number > max) max = number.First.Value.Number;

            number.Clear();
            number = Parse(line2);
            Add(Parse(line1));
            Reduce();
            Magnitude();
            if (number.First.Value.Number > max) max = number.First.Value.Number;
        }
    }

    Console.WriteLine(max);
}

void Part1()
{
    foreach (var line in File.ReadLines(fileName))
    {
        if (number.Count == 0)
        {
            number = Parse(line);
        }
        else
        {
            Add(Parse(line));
        }

        Reduce();
    }

    //DumpNumber(number);

    Magnitude();
    DumpNumber(number);
}

void Magnitude()
{
    while (ThereAreStillPairs())
    {
        for (var node = number.First; node != null; node = node.Next)
        {
            if (NodeIsStartOfPair(node))
            {
                var left = node;
                var right = node.Next.Next;

                // in this case "node" points at the first number in the pair
                int total = (left.Value.Number * 3) + (right.Value.Number * 2);

                number.Remove(node.Previous); // remove the leading [
                node.Value.Number = total; // update the magnitude
                number.Remove(node.Next); // remove the , separator
                number.Remove(node.Next); // remove the second number in the pair
                number.Remove(node.Next); // remove the training ]

                break;
            }
        }
    }
}

void Reduce()
{
    bool numberWasReduced = true;
    while (numberWasReduced)
    {
        numberWasReduced = false;

        var node = NumberNeedsToExplode();
        if (node != null)
        {
            Explode(node);
            numberWasReduced = true;
            continue;
        }

        node = NumberNeedsToSplit();
        if (node != null)
        {
            Split(node);
            numberWasReduced = true;
        }
    }
}

void Split(LinkedListNode<Item> node)
{
    // node is pointed at the number that needs to be split
    int left = node.Value.Number / 2;
    int right = (left * 2 == node.Value.Number) ? left : left + 1;

    // node gets replaced with [ left , right ]
    number.AddBefore(node, new Item(ItemType.Open));
    number.AddBefore(node, new Item(ItemType.Number, left));
    number.AddBefore(node, new Item(ItemType.Separator));
    node.Value.Number = right;
    number.AddAfter(node, new Item(ItemType.Close));
}

void Explode(LinkedListNode<Item> node)
{
    // node is pointed at the opening [ of a pair
    int left = node.Next.Value.Number;
    int right = node.Next.Next.Next.Value.Number;

    var leftNode = node.Previous;
    while (leftNode.Previous != null)
    {
        if (leftNode.Value.Type == ItemType.Number)
        {
            //the pair's left value is added to the first regular number to the left of the exploding pair (if any)
            leftNode.Value.Number += left;
            break;
        }
        leftNode = leftNode.Previous;
    }

    var rightNode = node.Next.Next.Next.Next;
    while (rightNode.Next != null)
    {
        if (rightNode.Value.Type == ItemType.Number)
        {
            //the pair's right value is added to the first regular number to the right of the exploding pair (if any)
            rightNode.Value.Number += right;
            break;
        }
        rightNode = rightNode.Next;
    }

    // the entire exploding pair is replaced with the regular number 0
    var n = node.Next;
    n.Value.Number = 0;

    number.Remove(n.Previous); // remove the leading [
    number.Remove(n.Next);     // remove the , separator
    number.Remove(n.Next);     // remove the second number in the par
    number.Remove(n.Next);     // remove the training ]
}

LinkedListNode<Item> NumberNeedsToSplit()
{
    for (var node = number.First; node != null; node = node.Next)
    {
        if (node.Value.Type == ItemType.Number && node.Value.Number >= 10)
        {
            return node;
        }
    }

    return null;
}

LinkedListNode<Item> NumberNeedsToExplode()
{
    int depth = 0;
    for (var node = number.First; node != null; node = node.Next)
    {
        switch (node.Value.Type)
        {
            case ItemType.Unknown:
            case ItemType.Separator:
                break;
            case ItemType.Open:
                ++depth;
                break;
            case ItemType.Close:
                --depth;
                break;
            case ItemType.Number:
                if (depth > 4 && NodeIsStartOfPair(node))
                {
                    return node.Previous;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    return null;
}

bool NodeIsStartOfPair(LinkedListNode<Item> node)
{
    // to be a pair, we need the pattern xx,yy
    if (node.Value.Type != ItemType.Number) return false;

    var n = node.Next;
    if (n == null || n.Value.Type != ItemType.Separator) return false;

    n = n.Next;
    if (n == null || n.Value.Type != ItemType.Number) return false;

    return true;
}

bool ThereAreStillPairs()
{
    for (var node = number.First; node != null; node = node.Next)
    {
        if (node.Value.Type == ItemType.Separator) return true;
    }

    return false;
}

void Add(LinkedList<Item> items)
{
    number.AddFirst(new Item(ItemType.Open));
    number.AddLast(new Item(ItemType.Separator));

    foreach (var item in items)
    {
        number.AddLast(item);
    }

    number.AddLast(new Item(ItemType.Close));
}

LinkedList<Item> Parse(string value)
{
    LinkedList<Item> results = new();

    int x = 0;
    while (x < value.Length)
    {
        char ch = value[x];
        switch (ch)
        {
            case '[':
                results.AddLast(new Item(ItemType.Open));
                x++;
                break;
            case ']':
                results.AddLast(new Item(ItemType.Close));
                x++;
                break;
            case ',':
                results.AddLast(new Item(ItemType.Separator));
                x++;
                break;
            default:
                results.AddLast(ParseNumber(value, ref x));
                break;
        }
    }

    return results;
}

Item ParseNumber(string value, ref int x)
{
    string num = "";
    for (int y = x; y < value.Length; y++)
    {
        char ch = value[y];
        if (ch == ']' || ch == '[' || ch == ',') break;
        num += ch.ToString();
    }

    x += num.Length;

    return new Item(ItemType.Number, int.Parse(num));
}

void DumpNumber(LinkedList<Item> results)
{
    if (results.Count == 0) return;

    for (var node = number.First; node != null; node = node.Next)
    {
        switch (node.Value.Type)
        {
            case ItemType.Open:
                Console.Write("[");
                break;
            case ItemType.Close:
                Console.Write("]");
                break;
            case ItemType.Separator:
                Console.Write(",");
                break;
            case ItemType.Number:
                Console.Write(node.Value.Number);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    Console.WriteLine();
}

class Item
{
    public ItemType Type { get; set; } = ItemType.Unknown;
    public int Number { get; set; }

    public Item(ItemType type, int number = 0)
    {
        Type = type;
        Number = number;
    }
}

enum ItemType
{
    Unknown,
    Open,
    Close,
    Separator,
    Number
}