using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using TankWar.Engine.Dto;

namespace TankWar.Engine.Objects
{
    public class CollisionHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public void Detect(Shell shell, Tank tank)
        {
            var hit = IsHit(shell.Point, tank.Target);
            if (hit)
            {
                Log.Info("'{0}' hit '{1}'", shell.Origin.Name, tank.Name);
                tank.HitBy = shell.Origin;
                tank.IsHit = true;
                shell.IsDead = true;
            }
        }
        private  bool IsHit(Point point, Area area)
        {
            bool withinX = point.X >= area.TopLeft.X && point.X <= area.BottomRight.X;
            bool withinY = point.Y <= area.TopLeft.Y && point.Y >= area.BottomRight.Y;
            return withinX && withinY;
        }
    }
}
