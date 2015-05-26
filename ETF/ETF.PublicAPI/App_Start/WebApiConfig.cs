namespace ETF.PublicAPI
{
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Dispatcher;

    using Castle.Windsor;

    using ETF.PublicAPI.Infrastructure.IoC;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config, IWindsorContainer container)
        {
            MapRoutes(config);
            RegisterControllerActivator(container);
        }

        private static void MapRoutes(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
                name: "UploadFile",
                routeTemplate: "{controller}/Upload",
                constraints:
                    new
                        {
                            httpMethod = new System.Web.Http.Routing.HttpMethodConstraint(HttpMethod.Post),
                            controller = "File"
                        },
                defaults: new { action = "Upload", });

            config.Routes.MapHttpRoute(
                name: "GetEtfWeightedIndices",
                routeTemplate: "{controller}/EtfWeightedIndex",
                constraints:
                    new
                        {
                            httpMethod = new System.Web.Http.Routing.HttpMethodConstraint(HttpMethod.Get),
                            controller = "ETF"
                        },
                defaults: new { action = "GetEtfWeightedIndices", });

            config.Routes.MapHttpRoute(
                name: "GetStockWeightedIndices",
                routeTemplate: "{controller}/StockWeightedIndex",
                constraints:
                    new
                        {
                            httpMethod = new System.Web.Http.Routing.HttpMethodConstraint(HttpMethod.Get),
                            controller = "Stock"
                        },
                defaults: new { action = "GetStockWeightedIndices", });

            config.Routes.MapHttpRoute(
                name: "GetTopStockIndices",
                routeTemplate: "{controller}/TopStockIndices",
                constraints:
                    new
                        {
                            httpMethod = new System.Web.Http.Routing.HttpMethodConstraint(HttpMethod.Get),
                            controller = "Stock"
                        },
                defaults: new { action = "GetTopStockIndices", });

            config.Routes.MapHttpRoute(
                name: "GetStockReturn",
                routeTemplate: "{controller}/StockReturn",
                constraints:
                    new
                        {
                            httpMethod = new System.Web.Http.Routing.HttpMethodConstraint(HttpMethod.Get),
                            controller = "Stock"
                        },
                defaults: new { action = "GetStockReturn", });
        }

        private static void RegisterControllerActivator(IWindsorContainer container)
        {
            GlobalConfiguration.Configuration.Services.Replace(
                typeof(IHttpControllerActivator),
                new WindsorCompositionRoot(container));
        }
    }
}
