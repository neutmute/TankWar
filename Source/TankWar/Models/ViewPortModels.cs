using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TankWar.Engine.Dto;

namespace TankWar.Models
{
    public class ViewPortAdminModel
    {
        public double GameLoopIntervalMilliseconds { get; set; }

        public int CountdownSeconds { get; set; }

        public override string ToString()
        {
            return string.Format("CountdownSeconds={0}, GameLoopIntervalMilliseconds={1}", CountdownSeconds, GameLoopIntervalMilliseconds);
        }
    }

    public class ViewPortModel
    {
        public Area ViewSize { get; set; }
    }
}