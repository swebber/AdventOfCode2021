using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day4a
{
    internal class Board
    {
        private const int BoardSize = 5;

        private bool _completed = false;

        public Board(IReadOnlyList<string> data, int index)
        {
            for (int row = 0; row < BoardSize; row++)
            {
                int col = 0;
                var values = data[++index].Split(' ', StringSplitOptions.TrimEntries);
                foreach (var value in values)
                {
                    if (value != "")
                    {
                        _field[row, col++] = int.Parse(value);
                    }
                }
            }
        }

        public int? Play(string value)
        {
            if (_completed) return null;

            int x = int.Parse(value);
            bool played = false;

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (_field[row, col] == x)
                    {
                        _field[row, col] = 0;
                        played = true;
                    }
                }
            }

            if (played && BoardFinished())
            {
                _completed = true;
                return BoardSum() * x;
            }

            return null;
        }

        private bool BoardFinished()
        {
            for (int row = 0; row < BoardSize; row++)
            {
                int rowSum = 0;
                for (int col = 0; col < BoardSize; col++)
                {
                    rowSum += _field[row, col];
                }

                if (rowSum == 0)
                    return true;
            }

            for (int col = 0; col < BoardSize; col++)
            {
                int colSum = 0;
                for (int row = 0; row < BoardSize; row++)
                {
                    colSum += _field[row, col];
                }

                if (colSum == 0)
                    return true;
            }

            return false;
        }

        private int BoardSum()
        {
            int sum = 0;

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    sum += _field[row, col];
                }
            }

            return sum;
        }

        public void Dump()
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    Console.Write($"{_field[row, col]} ");
                }
                Console.WriteLine();
            }
        }

        private readonly int[,] _field = new int[BoardSize, BoardSize];
    }
}
