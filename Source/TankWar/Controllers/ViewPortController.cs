using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using TankWar.Engine;
using TankWar.Engine.Dto;
using TankWar.Models;

namespace TankWar.Controllers
{
    public class ViewPortController : Controller
    {

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            var viewModel = new ViewPortModel();
            viewModel.ViewSize = Game.Instance.Screen;
            return View(viewModel);
        }

        public ActionResult Admin()
        {
            var game = Game.Instance;
            var model = new ViewPortAdminModel
                {
                    CountdownSeconds = game.CountDownSeconds
                    ,GameLoopIntervalMilliseconds = game.GameLoopIntervalMilliseconds
                };
            return View(model);
        }

        [HttpPost]
        public ActionResult ResetGameState()
        {
            Game.Instance.Init();
            return RedirectToAction("Admin");
        }

        [HttpPost]
        public ActionResult SetGameParams(ViewPortAdminModel model)
        {
            if (ModelState.IsValid)
            {
                Game.Instance.CountDownSeconds = model.CountdownSeconds;
                Game.Instance.GameLoopIntervalMilliseconds = model.GameLoopIntervalMilliseconds;
                Log.Info("Changed game settings: {0}", model);

            } 
            return RedirectToAction("Admin");
        }
    }
}
