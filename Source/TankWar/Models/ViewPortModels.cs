using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TankWar.Engine;
using TankWar.Engine.Dto;

namespace TankWar.Models
{
    public class ViewPortModel
    {
        public Point ViewSize { get; set; }
    }
}