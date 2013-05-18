using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.Engine
{
    public enum Orientation
    {
        Left,
        Right
    }

    public class Tank : Sprite
    {
        public Orientation Orientation { get; set; }

        /// <summary>
        /// Initiate explosion
        /// </summary>
        public bool IsHit { get; set; }
    }
}
