using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;

namespace ESPM
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Remover todos os tipos exceto JSON
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());

            // Web API routes
            config.MapHttpAttributeRoutes();

            // A API pública é definida no próprio controlador
            config.Routes.MapHttpRoute(
                name: "APIGestao",
                routeTemplate: "Gestao/api/{controller}/{action}/{id}",
                defaults: new { action = "Todos", id = RouteParameter.Optional }
            );
        }
    }
}
