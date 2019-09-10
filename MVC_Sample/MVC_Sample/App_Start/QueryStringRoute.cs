using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC_Sample
{
    public class QueryStringRoute : RouteBase
    {
        public string Url { get; set; }

        private bool Match(NameValueCollection queryString, out IDictionary<string, string> variables)
        {
            variables = new Dictionary<string, string>();

            var para = Url.Split('&');
            if (!para.All(x=>queryString.AllKeys.Contains(x)))
                return false;

            variables = para.ToDictionary(x => x, y => queryString[y]);

            return true;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            IDictionary<string, string> value;
       
            if (Match(httpContext.Request.QueryString,out value))
            {
                RouteData routeData = new RouteData(this, new MvcRouteHandler());

                foreach (var dict in value)
                    routeData.Values.Add(dict.Key,dict.Value);

                return routeData;
            }

            return null;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return null;
        }
    }
}