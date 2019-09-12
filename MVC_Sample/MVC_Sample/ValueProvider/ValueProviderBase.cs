using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;

namespace MVC_Sample.ValueProvider
{
    public abstract class ValueProviderBase
    {
        protected ControllerContext _controllerContext;

        public ValueProviderBase(ControllerContext controllerContext)
        {
            _controllerContext = controllerContext;
        }

        protected abstract NameValueCollection nameValueCollection { get; }

        public object GetValue(string modelName,Type modelType)
        {
            string key = nameValueCollection.AllKeys.FirstOrDefault(x => string.Compare(x, modelName, StringComparison.OrdinalIgnoreCase) == 0);

            if (key != null)
            {
                return Convert.ChangeType(nameValueCollection[key], modelType);
            }

            return null;
        }
    }
}