using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;

namespace TankWar.Controllers
{
    public class Home : NancyModule
    {

        public Home()
        {
            Get["/"] = parameters => View["Index.cshtml"];
            Get["/preso"] = parameters => View["Preso.cshtml"];
        }

    }
}
