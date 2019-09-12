using System.Collections.Specialized;
using System.Web.Mvc;

namespace MVC_Sample.ValueProvider
{
    public class QueryStringValueProvider : ValueProviderBase
    {
        public QueryStringValueProvider(ControllerContext controllerContext) : base(controllerContext)
        {
        }

        protected override NameValueCollection nameValueCollection => _controllerContext.RequestContext.HttpContext.Request.QueryString;
    }
}