using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankWar.Engine.Objects;

namespace TankWar.Engine
{
    public class Shell : Sprite
    {
        public int LaunchTime { get; set; }

        public Tank Origin { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, LaunchState=[{1}]", base.ToString(), Origin);
        }
    }
}
