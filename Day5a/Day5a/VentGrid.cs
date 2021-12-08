using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day5a
{
    internal class VentGrid
    {
        private int _rowSize;
        private int _colSize;
        private int[,] _grid;

        public VentGrid(int rowSize, int colSize)
        {
            _rowSize = rowSize;
            _colSize = colSize;
            _grid = new int[colSize + 1, rowSize + 1];
        }

        public void Dump()
        {
            for (int row = 0; row <= _rowSize; row++)
            {
                for (int col = 0; col <= _colSize; col++)
                {
                    Console.Write($"{_grid[row, col]} ");
                }

                Console.WriteLine();
            }
        }

        public int OverlapCount()
        {
            int overlapCount = 0;

            for (int row = 0; row <= _rowSize; row++)
            {
                for (int col = 0; col <= _colSize; col++)
                {
                    if (_grid[col, row] > 1)
                        overlapCount++;
                }
            }

            return overlapCount;
        }

        private void AddColumn(VentLine line)
        {
            var x = line.X1;
            var y = line.Y1;
            var offset = line.Y1 < line.Y2 ? 1 : -1;

            while (y != line.Y2 + offset)
            {
                ++_grid[y, x];
                y += offset;
            }
        }

        private void AddRow(VentLine line)
        {
            var x = line.X1;
            var y = line.Y1;
            var offset = line.X1 < line.X2 ? 1 : -1;

            while (x != line.X2 + offset)
            {
                ++_grid[y, x];
                x += offset;
            }
        }

        private void AddDiagonal(VentLine line)
        {
            var x = line.X1;
            var y = line.Y1;
            var xOffset = line.X1 < line.X2 ? 1 : -1;
            var yOffset = line.Y1 < line.Y2 ? 1 : -1;

            // this had better the the same for both x and y
            var distance = Math.Abs(line.X1 - line.X2) + 1;

            while (x != line.X2 + xOffset)
            {
                ++_grid[y, x];
                x += xOffset;
                y += yOffset;
            }
        }

        public void AddRange(List<VentLine> ventLines)
        {
            foreach (var line in ventLines)
            {
                // single point
                if (line.X1 == line.X2 && line.Y1 == line.Y2)
                {
                    ++_grid[line.X1, line.Y1];
                    continue;;
                }

                // vertical column
                if (line.X1 == line.X2 && line.Y1 != line.Y2)
                {
                    AddColumn(line);
                    continue;
                }

                // horizontal row
                if (line.X1 != line.X2 && line.Y1 == line.Y2)
                {
                    AddRow(line);
                    continue;
                }

                AddDiagonal(line);
            }
        }
    }
}
