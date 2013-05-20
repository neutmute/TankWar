using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TankWar.Engine;
using TankWar.Engine.Dto;
using TankWar.Models;

namespace TankWar.Controllers
{
    public class ViewPortController : Controller
    {
        //
        // GET: /ViewPort/

        public ActionResult Index()
        {
            var viewModel = new ViewPortModel();
            viewModel.ViewSize = Game.Instance.Screen;
            return View(viewModel);
        }

        public ActionResult Admin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetGameState()
        {
            Game.Instance.Init();
            return RedirectToAction("Admin");
        }
    }
}
