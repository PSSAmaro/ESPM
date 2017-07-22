using System.Web.Http;
using System.Web.Mvc;

namespace ESPM.Areas.Gestao
{
    public class GestaoAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Gestao";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
                name: "APIGestao",
                routeTemplate: "Gestao/api/{controller}/{action}/{id}",
                defaults: new { action = "Index", id = RouteParameter.Optional }
            );

            context.MapRoute(
                "Gestao_default",
                "Gestao/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}