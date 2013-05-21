using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TankWar.Engine.Dto;

namespace TankWar.Models
{
    public class ViewPortAdminModel
    {
        [DisplayName("Game loop Interval (ms)")]
        [Range(0, 10000.00, ErrorMessage = "This could take a while...")]
        public int GameLoopIntervalMilliseconds { get; set; }

        [DisplayName("Countdown (seconds)")]
        [Range(0, 300.00, ErrorMessage = "You want to wait that long, really?")]
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