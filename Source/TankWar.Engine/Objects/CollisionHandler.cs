using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using TankWar.Engine.Dto;

namespace TankWar.Engine.Objects
{
    public delegate void NotifyPlayerMethod(Player player, PlayerStatus status);

    public class CollisionHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private NotifyPlayerMethod _notifyPlayerMethod;

        public CollisionHandler(NotifyPlayerMethod notifyPlayerMethod)
        {
            _notifyPlayerMethod = notifyPlayerMethod;
          
        }

        public void Detect(Shell shell, Tank tank)
        {
            var hit = IsHit(shell.Point, tank.Target);
            if (hit)
            {
                Log.Info("'{0}' hit '{1}'", shell.Origin.Name, tank.Name);
                
                tank.IsHit = true;
                shell.IsDead = true;

                _notifyPlayerMethod(tank.Owner, PlayerStatus.Dead);
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
