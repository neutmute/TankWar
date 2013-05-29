using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using TankWar.Engine.Dto;

namespace TankWar.Engine.Objects
{
    public delegate void NotifyHitMethod(Player playerHit, Shell hitBy);

    public class CollisionHandler
    {
        private readonly NotifyHitMethod _notifyHitMethod;

        public CollisionHandler(NotifyHitMethod notifyHitMethod)
        {
            _notifyHitMethod = notifyHitMethod;
          
        }

        public void Detect(Shell shell, Tank tank)
        {
            var hit = IsHit(shell.Point, tank.Target);
            if (hit)
            {              
                tank.IsHit = true;
                shell.IsDead = true;

                _notifyHitMethod(tank.Owner, shell);
            }
        }
        private  bool IsHit(Point point, Area area)
        {
            const int margin = 3;
            bool withinX = point.X >= (area.TopLeft.X+margin) && point.X <= (area.BottomRight.X - margin);
            bool withinY = point.Y <= (area.TopLeft.Y -margin)&& point.Y >= (area.BottomRight.Y + margin);
            return withinX && withinY;
        }
    }
}
