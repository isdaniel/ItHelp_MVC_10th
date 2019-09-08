using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC_Sample
{
    public class CustomerControllerActivator : IControllerActivator
    {
        public IController Create(RequestContext requestContext, Type controllerType)
        {
            return (IController) DependencyResolver.Current.GetService(controllerType);
        }
    }
}