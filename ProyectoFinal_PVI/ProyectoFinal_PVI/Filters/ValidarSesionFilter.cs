using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoFinal_PVI.Models;

namespace ProyectoFinal_PVI.Filters
{
    public class ValidarSesionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var sesion = (LoginModel)HttpContext.Current.Session["nuevaSesion"];
            if (sesion == null)
            {
                filterContext.HttpContext.Response.Redirect("~/Login/Index");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}


