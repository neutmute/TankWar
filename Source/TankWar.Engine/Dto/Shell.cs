using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.Engine
{
    public class Shell : Sprite
    {
        public TurretSetting LaunchState { get; set; }

        public int LaunchTime { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, LaunchState=[{1}]", base.ToString(), LaunchState);
        }
    }
}
