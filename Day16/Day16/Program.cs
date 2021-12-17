using System.Text;

string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day16\Day16\day16-data.txt";

int index = 0;
string hexString = File.ReadAllText(fileName);
string binaryString = string.Join(string.Empty,
    hexString.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

//Console.WriteLine(hexString);
//Console.WriteLine(binaryString);

long results = WalkPacket();
Console.WriteLine($"Results: {results}");

long WalkPacket()
{
    // version
    int version = ReadBinary(3, "Version");

    // packet type
    int packetType = ReadBinary(3, "Packet type");

    if (packetType == 4)
    {
        return ReadLiteral();
    }

    List<long> packetValues = new();
    int lengthTypeId = ReadLengthType();
    if (lengthTypeId == 0)
    {
        int length = LengthOfPackets();
        
        int end = index + length;
        while (index < end)
        {
            packetValues.Add(WalkPacket());
        }
    }
    else
    {
        int packetCount = ReadByCount();
        for (int i = 0; i < packetCount; i++)
        {
            packetValues.Add(WalkPacket());
        }
    }

    long result = 1;
    return packetType switch
    {
        0 => packetValues.Sum(),
        1 => packetValues.Aggregate(result, (current, value) => current * value),
        2 => packetValues.Min(),
        3 => packetValues.Max(),
        5 => packetValues[0] > packetValues[1] ? 1L : 0L,
        6 => packetValues[0] < packetValues[1] ? 1L : 0L,
        _ => packetValues[0] == packetValues[1] ? 1L : 0L
    };
}

int ReadByCount()
{
    int numberOfPackets = ReadBinary(11, "Number of packets");
    return numberOfPackets;
}

int LengthOfPackets()
{
    int totalLength = ReadBinary(15, "Length of packets");
    return totalLength;
}

int ReadLengthType()
{
    int value = Convert.ToInt32(binaryString.Substring(index, 1), 2);
    index += 1;

    return value;
}

long ReadLiteral()
{
    bool done = false;
    StringBuilder literal = new();

    while (!done)
    {
        string value = binaryString.Substring(index, 5);
        literal.Append(value.Substring(1));
        done = value[0] == '0';
        index += 5;
    }

    long literalValue = Convert.ToInt64(literal.ToString(), 2);
    return literalValue;
}

int ReadBinary(int length, string option)
{
    int value = Convert.ToInt32(binaryString.Substring(index, length), 2);
    index += length;

    return value;
}