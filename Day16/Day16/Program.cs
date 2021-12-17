using System.Text;

string fileName = @"C:\Users\WebberS\source\repos\AdventOfCode2021\Day16\Day16\day16-data.txt";

int index = 0;
string hexString = File.ReadAllText(fileName);
string binaryString = string.Join(string.Empty,
    hexString.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

//Console.WriteLine(hexString);
//Console.WriteLine(binaryString);

long versionNumberTotal = 0L;

WalkPacket();

Console.WriteLine($"Version total: {versionNumberTotal}");

void WalkPacket()
{
    // version
    int version = ReadBinary(3, "Version");
    versionNumberTotal += version;

    // packet type
    int packetType = ReadBinary(3, "Packet type");

    if (packetType == 4)
    {
        ulong literal = ReadLiteral();
        return;
    }

    int lengthTypeId = ReadLengthType();
    if (lengthTypeId == 0)
    {
        int length = LengthOfPackets();

        int end = index + length;
        while (index < end)
        {
            WalkPacket();
        }
    }
    else
    {
        int packetCount = ReadByCount();
        for (int i = 0; i < packetCount; i++)
        {
            WalkPacket();
        }
    }
}

bool EndOfTransmission()
{
    if (index == binaryString.Length) return true;

    for (int x = index; x < binaryString.Length; x++)
    {
        if (binaryString[x] == '1') return false;
    }

    return true;
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

ulong ReadLiteral()
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

    ulong literalValue = Convert.ToUInt64(literal.ToString(), 2);
    return literalValue;
}

int ReadBinary(int length, string option)
{
    int value = Convert.ToInt32(binaryString.Substring(index, length), 2);
    index += length;

    return value;
}