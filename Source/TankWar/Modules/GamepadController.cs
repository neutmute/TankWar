using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;

namespace TankWar.Controllers
{
    public class GamepadController : NancyModule
    {
        public GamepadController()
        {
            Get["/"] = parameters => "Hello World";
        }
    }
}
