using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankWar.Engine.Dto;

namespace TankWar.Engine
{
    public class Tank : Sprite
    {
        public TurretSetting Setting { get; set; }

        public const int Height = 40;
        public const int Width = 40;

        /// <summary>
        /// Where the tank can get hit
        /// </summary>
        public Area Target { get; set; }

        public string Name { get; set; }

        public override Point Point
        {
            get { return base.Point; }
            set 
            { 
                base.Point = value;
                Target = new Area(value.X, value.Y, value.X + Width, value.Y+Height);
            }
        }

        /// <summary>
        /// Initiate explosion
        /// </summary>
        public bool IsHit { get; set; }

        public bool IsFiring { get; set; }

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
