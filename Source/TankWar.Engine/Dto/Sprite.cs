using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.Engine
{
    public class Sprite
    {
        public int Id { get; set; }

        public Point Point { get; set; }

        /// <summary>
        /// Remove from viewport
        /// </summary>
        public bool IsDead { get; set; }

        public Sprite()
        {
            Point = new Point();
        }
    }
}
