using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;

namespace TankWar.Controllers
{
    public class Gamepad : NancyModule
    {
        public Gamepad() : base("/gamepad")
        {
            Get["/"] = parameters => View["Index.cshtml"];
        }
    }
}
