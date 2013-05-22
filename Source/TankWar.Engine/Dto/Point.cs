using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.Engine
{
    public enum PointType
    {
        Cartesian,
        Screen
    }

    public class Point
    {
        public int X { get;  set; }
        public int Y { get;  set; }


        public PointType Type { get; private set; }

        public Point(int x, int y, PointType type = PointType.Cartesian)
        {
            X = x;
            Y = y;
            Type = type;
        }

        public Point()
        {
                
        }

        public override string ToString()
        {
            var prefix = Type == PointType.Cartesian ? "c" : "s";
            return String.Format("({2}:{0},{1})", X, Y, prefix);
        }
    }
}
