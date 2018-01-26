using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RepidShare.Admin
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
               name: "ControllerAndAction",
               routeTemplate: "api/{controller}/{action}"
           );
        }
    }
}
