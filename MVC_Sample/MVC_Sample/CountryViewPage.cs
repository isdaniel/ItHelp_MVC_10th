using System.Web.Mvc;

namespace MVC_Sample
{
    public abstract class CountryViewPage<TModel> : WebViewPage<TModel>
    {
        public CountryViewPage()
        {
            Currency = DependencyResolver.Current.GetService<ICurrency>();
        }
        public ICurrency Currency { get;  }
    }
}