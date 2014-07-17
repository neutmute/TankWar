using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;

namespace TankWar.Controllers
{
    public class HomeController : NancyModule
    {

        public HomeController()
        {
            Get["/"] = parameters => View["Index.cshtml"];
            Get["/preso"] = parameters => View["Preso.cshtml"];
        }

    }
}
