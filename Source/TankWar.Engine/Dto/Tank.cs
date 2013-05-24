using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankWar.Engine.Dto;
using Omu.ValueInjecter;
using TankWar.Engine.Objects;

namespace TankWar.Engine
{
    public class TankDto : Sprite
    {
        public TurretSetting Turret { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Initiate explosion
        /// </summary>
        public bool IsHit { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, turret=[{1}]", base.ToString(), Turret);
        }
    }

    public class Tank : TankDto
    {
        public const int Height = 40;
        public const int Width = 40;

        /// <summary>
        /// Where the tank can get hit
        /// </summary>
        public Area Target { 
            get
            {
                return new Area(Point.X, Point.Y, Point.X + Width, Point.Y - Height);
            }
        }

        /// <summary>
        /// Remove from viewport
        /// </summary>
        public bool IsDead { get; set; }

        public bool IsFiring { get; set; }

        public Tank HitBy { get; set; }

        public Player Owner { get; set; }

        public Tank()
        {
            Turret = new TurretSetting();
        }

        public TankDto ToDto()
        {
            var dto = new TankDto();
            dto.InjectFrom(this);
            return dto;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override bool Equals(object obj)
        {
            var objAsTank = obj as Tank;
            return objAsTank != null && objAsTank.Id == Id;
        }

    }
}
