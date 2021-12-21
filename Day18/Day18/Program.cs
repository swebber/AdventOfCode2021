using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day18\Day18\test2-data.txt";

LinkedList<Item> number = new();

int i = 0;
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

DumpNumber(number);
CountSinglesAndPairs();

void CountSinglesAndPairs()
{
    int singles = 0;
    int pairs = 0;

    for (var node = number.First; node != null; node = node.Next)
    {
        switch (node.Value.Type)
        {
            case ItemType.Unknown:
            case ItemType.Open:
            case ItemType.Close:
            case ItemType.Separator:
            case ItemType.Single:
                ++singles;
                break;
            case ItemType.Pair:
                ++pairs;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    Console.WriteLine($"Singles: {singles}, Pairs: {pairs}");
}

void FixBrokenPairs()
{
    for (var node = number.First; node != null; node = node.Next)
    {
        if (node.Next == null || node.Next.Next == null || node.Next.Next.Next == null ||
            node.Next.Next.Next.Next == null) break;

        var n1 = node.Next;
        if (n1.Value.Type != ItemType.Single) continue;
        var n2 = n1.Next;
        if (n2.Value.Type != ItemType.Separator) continue;
        var n3 = n2.Next;
        if (n3.Value.Type != ItemType.Single) continue;
        var n4 = n3.Next;
        if (n4.Value.Type != ItemType.Close) continue;

        n1.Value.Type = ItemType.Pair;
        n1.Value.Right = n3.Value.Left;

        number.Remove(n2);
        number.Remove(n3);
    }
}

//long magnitude = Magnitude();
//Console.WriteLine($"Magnitude: {magnitude}");


//long Magnitude()
//{
//    for (var node = number.First; node != null; node = node.Next)
//    {
//        if (node.Value.Type == ItemType.Pair)
//        {
//            // convert to single, magnitute = left * 3 + right * 2
//            node.Value.Type = ItemType.Single;
//            node.Value.Left = (node.Value.Left * 3) + (node.Value.Right * 2);
//            node.Value.Right = int.MinValue;

//            // remove left and right nodes
//            number.Remove(node.Previous);
//            number.Remove(node.Next);
//        }
//    }

//    bool done = false;
//    while (!done)
//    {
//        done = true;
//        for (var node = number.First; node != null; node = node.Next)
//        {
//            if (node.Value.Type != ItemType.Open) continue;

//            // node => open
//            LinkedListNode<Item> r1 = null; // r1 => single
//            LinkedListNode<Item> r2 = null; // r2 => separator
//            LinkedListNode<Item> r3 = null; // r3 => single
//            LinkedListNode<Item> r4 = null; // r4 => close

//            if (node.Next == null) continue;

//            r1 = node.Next;
//            if (r1.Value.Type != ItemType.Single || r1.Next == null) continue;

//            r2 = r1.Next;
//            if (r2.Value.Type != ItemType.Separator || r2.Next == null) continue;

//            r3 = r2.Next;
//            if (r3.Value.Type != ItemType.Single || r3.Next == null) continue;

//            r4 = r3.Next;
//            if (r4.Value.Type != ItemType.Close || r4.Next == null) continue;

//            done = false;

//            r1.Value.Left = (r1.Value.Left * 3) + (r3.Value.Left * 2);

//            number.Remove(r4);
//            number.Remove(r3);
//            number.Remove(r2);
//            number.Remove(node);

//            node = r1;
//        }
//    }

//    return 0L;
//}

void Reduce()
{
    bool processExplode = true;
    bool processSplit = false;

    while (true)
    {
        if (processExplode && Explode2())
        {
            FixBrokenPairs();
            continue;
        }

        while (Split2())
        {
            FixBrokenPairs();
            continue;
        }
        //bool splitting = Split2();
        //FixBrokenPairs();
        //if (splitting) ++count;

        break;
    }
}

bool Split2()
{
    for (var node = number.First; node != null; node = node.Next)
    {
        switch (node.Value.Type)
        {
            case ItemType.Open:
            case ItemType.Close:
            case ItemType.Separator:
            case ItemType.Single:
                if (node.Value.Left > 9)
                {
                    Split(node);
                    return true;
                }
                break;
            case ItemType.Pair:
                if (node.Value.Left > 9)
                {
                    SplitLeft(node);
                    return true;
                }
                else if (node.Value.Right > 9)
                {
                    SplitRight(node);
                    return true;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    return false;
}

bool Explode2()
{
    int depth = 0;

    for (var node = number.First; node != null; node = node.Next)
    {
        switch (node.Value.Type)
        {
            case ItemType.Open:
                depth++;
                break;
            case ItemType.Close:
                depth--;
                break;
            case ItemType.Separator:
            case ItemType.Single:
                break;
            case ItemType.Pair:
                if (depth >= 5)
                {
                    Explode(node);
                    return true;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    return false;
}

void SplitLeft(LinkedListNode<Item> node)
{
    int left = node.Value.Left / 2;
    int right = (left * 2 == node.Value.Left) ? left : left + 1;

    // [ xx , yy ] ==> [ [ left , right ] , yy }
    number.AddBefore(node, new Item(ItemType.Open));
    number.AddBefore(node, new Item(ItemType.Pair) { Left = left, Right = right });
    number.AddBefore(node, new Item(ItemType.Close));
    number.AddBefore(node, new Item(ItemType.Separator));
    number.AddBefore(node, new Item(ItemType.Single) { Left = node.Value.Right });

    number.Remove(node);
}

void SplitRight(LinkedListNode<Item> node)
{
    int left = node.Value.Right / 2;
    int right = (left * 2 == node.Value.Right) ? left : left + 1;

    // [ xx , yy ] ==> [ xx , [ left , right ]]
    number.AddBefore(node, new Item(ItemType.Single) { Left = node.Value.Left });
    number.AddBefore(node, new Item(ItemType.Separator));
    number.AddBefore(node, new Item(ItemType.Open));
    number.AddBefore(node, new Item(ItemType.Pair) { Left = left, Right = right });
    number.AddBefore(node, new Item(ItemType.Close));

    number.Remove(node);
}

void Split(LinkedListNode<Item> node)
{
    int left = node.Value.Left / 2;
    int right = (left * 2 == node.Value.Left) ? left : left + 1;

    // [ left , right ]
    number.AddBefore(node, new Item(ItemType.Open));
    number.AddBefore(node, new Item(ItemType.Pair) { Left = left, Right = right });
    number.AddBefore(node, new Item(ItemType.Close));

    number.Remove(node);
}

void Explode(LinkedListNode<Item> node)
{
    int left = node.Value.Left;
    int right = node.Value.Right;

    // update regular number to the left
    var leftNode = node;
    while (leftNode.Previous != null)
    {
        leftNode = leftNode.Previous;
        if (leftNode.Value.Type == ItemType.Single)
        {
            leftNode.Value.Left += left;
            break;
        } 
        if (leftNode.Value.Type == ItemType.Pair)
        {
            leftNode.Value.Right += left;
            break;
        }
    }

    // update regular number to the right
    var rightNode = node;
    while (rightNode.Next != null)
    {
        rightNode = rightNode.Next;
        if (rightNode.Value.Type is ItemType.Single or ItemType.Pair)
        {
            rightNode.Value.Left += right;
            break;
        }
    }

    // replace pair with 0 (zero) [ left,right ]
    number.Remove(node.Previous);
    number.Remove(node.Next);

    node.Value.Type = ItemType.Single; ;
    node.Value.Left = 0;
    node.Value.Right = int.MinValue;
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
    string foo = "";
    for (int y = x; y < value.Length; y++)
    {
        char ch = value[y];
        if (ch == ']' || ch == '[') break;
        foo += ch.ToString();
    }

    Item item;
    if (foo[^1] == ',')
    {
        // number part ends in a comma, it's a single number value at the start of a pair
        item = new Item(ItemType.Single) { Left = int.Parse(foo[..^1]) };
        x += foo.Length - 1;
        return item;
    }

    if (!foo.Contains(','))
    {
        // number part ends in closing ]
        item = new Item(ItemType.Single) { Left = int.Parse(foo) };
        x += foo.Length;
        return item;
    }

    // number part is a pair of numbers
    var pair = foo.Split(",");
    item = new Item(ItemType.Pair)
    {
        Left = int.Parse(pair[0]),
        Right = int.Parse(pair[1])
    };
    x += foo.Length;

    return item;
}

void DumpNumber(LinkedList<Item> results)
{
    if (results.Count == 0) return;

    var item = results.First;
    while (item != null)
    {
        switch (item.Value.Type)
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
            case ItemType.Single:
                Console.Write(item.Value.Left);
                break;
            case ItemType.Pair:
                Console.Write($"{item.Value.Left},{item.Value.Right}");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        item = item.Next;
    }

    Console.WriteLine();
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

class Item
{
    public ItemType Type { get; set; } = ItemType.Unknown;
    public int Left { get; set; } = int.MinValue;
    public int Right { get; set; } = int.MinValue;

    public long Magnitude
    {
        get
        {
            switch (Type)
            {
                case ItemType.Unknown:
                case ItemType.Open:
                case ItemType.Close:
                case ItemType.Separator:
                    return 0L;
                case ItemType.Single:
                    return (long)Left;
                case ItemType.Pair:
                    return ((long)Left * 3L) + ((long)Right * 2L);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public Item(ItemType type)
    {
        Type = type;
    }
}

enum ItemType
{
    Unknown,
    Open,
    Close,
    Separator,
    Single,
    Pair
}