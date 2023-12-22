using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;

namespace TubelessServices
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            //https://stackoverflow.com/questions/57788435/setting-valid-produce-and-consume-media-types-in-swagger-for-mvc5-in-net-framew
            //https://blog.kloud.com.au/2017/08/04/swashbuckle-pro-tips-for-aspnet-web-api-part-1/
            //var mediaType = new MediaTypeHeaderValue("application/json");
            //var formatter = new JsonMediaTypeFormatter();
            //formatter.SupportedMediaTypes.Clear();
            //formatter.SupportedMediaTypes.Add(mediaType);
            //config.Formatters.Clear();
            //config.Formatters.Add(formatter);

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}
