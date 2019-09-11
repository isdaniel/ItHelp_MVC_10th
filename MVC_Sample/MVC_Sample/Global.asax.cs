using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using MVC_Sample.Services;

namespace MVC_Sample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ControllerBuilder.Current.SetControllerFactory(
                new DefaultControllerFactory(new CustomerControllerActivator()));
            AutofacRegister();
        }

        private static void AutofacRegister()
        {
            ContainerBuilder builder = new ContainerBuilder();
            //注入typeof(MvcApplication).Assembly 中所有繼承IController物件.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<MemberService>().As<IMemberService>();
            //因為IActionInvoker會先從解析器找尋實作IActionInvoker類別,如果沒有使用DefaultActionInvoker
            builder.RegisterType<CustomerActionInvoker>().As<IActionInvoker>();

            //替換成自己的DependencyResolver
            DependencyResolver.SetResolver(new CustomerDependencyResolver(builder.Build()));
        }
    }
}

