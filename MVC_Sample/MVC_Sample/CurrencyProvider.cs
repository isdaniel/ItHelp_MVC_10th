using System.Web;
using System.Web.Routing;

namespace MVC_Sample
{
    public class CurrencyProvider : ICurrency
    {
        public string GetCurrencySymbol()
        {
            HttpContextBase contextWrapper = new HttpContextWrapper(HttpContext.Current);

            string culture = RouteTable.Routes.GetRouteData(contextWrapper)?.Values["culture"] as string;

            return GetSymbol(culture);
        }

        private string GetSymbol(string culture)
        {
            switch (culture)
            {
                case "en":
                    return "$";
                case "eu":
                    return "£";
                default:
                    return "$";
            }
        }
    }
}