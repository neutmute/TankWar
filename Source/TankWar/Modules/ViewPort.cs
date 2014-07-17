using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;
using TankWar.Engine;
using TankWar.Engine.Dto;
using TankWar.Engine.Objects;
using TankWar.Models;
using Nancy;

namespace TankWar.Controllers
{
    public class ViewPort : NancyModule
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();


        public ViewPort()
        {
            var viewModel = new ViewPortModel();
            viewModel.ViewSize = Game.Instance.GameParameters.ViewPortSize;

            Get["/"] = parameters => View["Index.cshtml", viewModel];
            Get["/Admin"] = parameters => View["Admin.cshtml", Game.Instance.GameParameters];
            Post["/ResetGameState"] = parameters => 
            {
                Game.Instance.Init();
                return Response.AsRedirect("/Admin");
            };
            Post["/ForceStart"] = parameters =>
            {
                Game.Instance.Start();
                return Response.AsRedirect("/Admin");
            };
            Post["/GameOver"] = parameters =>
            {
                Game.Instance.Stop();
                return Response.AsRedirect("/Admin");
            };
            Post["/Admin"] = model =>
            {
                //if (ModelState.IsValid)
                {
                    Game.Instance.GameParameters = model;
                    Log.Info("Changed game settings: {0}", model);
                    return Response.AsRedirect("/Admin");
                }
                return View["Admin.cshtml", model];
            };
        }

    }
}
