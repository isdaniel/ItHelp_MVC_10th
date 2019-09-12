using System.Collections.Specialized;
using System.Web.Mvc;

namespace MVC_Sample.ValueProvider
{
    public class FormValueProvider : ValueProviderBase
    {
        public FormValueProvider(ControllerContext controllerContext) : base(controllerContext)
        {
        }

        protected override NameValueCollection nameValueCollection => _controllerContext.RequestContext.HttpContext.Request.Form;
    }
}