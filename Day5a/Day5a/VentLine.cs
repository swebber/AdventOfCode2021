using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day5a
{
    internal class VentLine
    {
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }

        public VentLine(string lineData)
        {
            var coordinatePair = lineData.Split(" -> ");
            var pair0 = coordinatePair[0].Split(',');
            var pair1 = coordinatePair[1].Split(',');

            X1 = int.Parse(pair0[0]);
            Y1 = int.Parse(pair0[1]);
            X2 = int.Parse(pair1[0]);
            Y2 = int.Parse(pair1[1]);
        }

        public int MaxRow()
        {
            int maxValue = int.MinValue;

            if (X1 > maxValue) maxValue = X1;
            if (X2 > maxValue) maxValue = X2;

            return maxValue;
        }

        public int MaxColumn()
        {
            int maxValue = int.MinValue;

            if (Y1 > maxValue) maxValue = Y1;
            if (Y2 > maxValue) maxValue = Y2;

            return maxValue;
        }

        public bool IsLineValid()
        {
            if (X1 == X2 || Y1 == Y2)
                return true;

            int xDistance = Math.Abs(X1 - X2);
            int yDistance = Math.Abs(Y1 - Y2);
            if (xDistance == yDistance)
                return true;

            return false;
        }
    }
}
