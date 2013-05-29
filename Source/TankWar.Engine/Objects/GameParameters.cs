using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankWar.Engine.Dto;

namespace TankWar.Engine.Objects
{
    public class GameParameters
    {
        [DisplayName("Game loop Interval (ms)")]
        [Range(0, 10000.00, ErrorMessage = "This could take a while...")]
        public int GameLoopIntervalMilliseconds { get; set; }

        [DisplayName("Countdown (seconds)")]
        [Range(0, 300.00, ErrorMessage = "You want to wait that long, really?")]
        public int CountdownSeconds { get; set; }

        [DisplayName("Maximum Game Time (minutes)")]
        public int MaximumGameTimeMinutes { get; set; }

        [DisplayName("ViewPort Size")]
        [UIHint("String")]
        public Point ViewPortSize { get; set; }

        public GameParameters()
        {
            ViewPortSize = new Point(800, 400);
            CountdownSeconds = 1;
            MaximumGameTimeMinutes = 2;
            GameLoopIntervalMilliseconds = 25;
        }

        public override string ToString()
        {
            return string.Format(
                "CountdownSeconds={0}, GameLoopIntervalMilliseconds={1}, MaximumGameTimeMinutes={3}, Screen={2}"
                , CountdownSeconds
                , GameLoopIntervalMilliseconds
                , ViewPortSize
                , MaximumGameTimeMinutes);
        }
    }
}
