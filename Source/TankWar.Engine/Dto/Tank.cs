using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.Engine
{
    public class Tank : Sprite
    {
        public TurretSetting Setting { get; set; }

        /// <summary>
        /// Initiate explosion
        /// </summary>
        public bool IsHit { get; set; }

        public Tank()
        {
            Setting = new TurretSetting();
        }

        public override string ToString()
        {
            return string.Format("{0}, turret=[{1}]", base.ToString(), Setting);
        }
    }
}
