using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TankWar.Engine;

namespace TankWar.Models
{
    public class PointModelBinder : IModelBinder
    {
        //public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        //{
        //    var valueProvider = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        //    if (valueProvider == null)
        //    {
        //        return new Point(0,0);
        //    }

        //    var stringValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).AttemptedValue;
        //    var regExMatch = Regex.Match(stringValue, @"(\d+),(\d+)");
            
        //    if (regExMatch.Success)
        //    {
        //        int x = Convert.ToInt32(regExMatch.Groups[1].Value);
        //        int y = Convert.ToInt32(regExMatch.Groups[2].Value);
        //        var point = new Point(x, y);
        //        return point;
        //    }
        //    return new Point(0, 0);
        //}


        public bool CanBind(Type modelType)
        {
            throw new NotImplementedException();
        }

        public object Bind(Nancy.NancyContext context, Type modelType, object instance, BindingConfig configuration, params string[] blackList)
        {
            throw new NotImplementedException();
        }
    }
}