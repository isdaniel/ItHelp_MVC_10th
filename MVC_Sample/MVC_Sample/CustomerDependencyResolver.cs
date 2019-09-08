using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Autofac;

namespace MVC_Sample
{
    public class CustomerDependencyResolver : IDependencyResolver
    {
        private readonly ILifetimeScope _container;

        public CustomerDependencyResolver(ILifetimeScope container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof (container));

            _container = container;
        }

        public object GetService(Type serviceType)
        {
            return _container.ResolveOptional(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return (IEnumerable<object>) _container.ResolveOptional(typeof (IEnumerable<>).MakeGenericType(serviceType));
        }
    }
}