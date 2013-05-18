using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.Engine.Dto
{
    public class Area
    {
        public Point TopLeft { get; private set; }

        public Point BottomRight { get; private set; }

        public Area()
        {
                
        }

        public Area(int x1, int y1, int x2, int y2)
        {
            TopLeft = new Point(x1, y1);
            BottomRight = new Point(x2, y2);
        }

        public override string ToString()
        {
            return String.Format("TL={0}, BR={1}", TopLeft, BottomRight);
        }
    }
}
