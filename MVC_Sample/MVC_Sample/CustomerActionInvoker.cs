using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace MVC_Sample
{
    public class CustomerActionInvoker : IActionInvoker
    {

        public CustomerActionInvoker()
        {

        }

        public bool InvokeAction(ControllerContext controllerContext, string actionName)
        {
            MethodInfo method = controllerContext.Controller.GetType().GetMethods().First(m => string.Compare(actionName, m.Name, StringComparison.OrdinalIgnoreCase) == 0);
            List<object> parameters = new List<object>();
            foreach (ParameterInfo parameter in method.GetParameters())
            {
                parameters.Add(BindModel(controllerContext, parameter.Name, parameter.ParameterType));
            }
            ActionResult actionResult = method.Invoke(controllerContext.Controller, parameters.ToArray()) as ActionResult;

            actionResult.ExecuteResult(controllerContext);

            return true;
        }

        private object BindModel(ControllerContext controllerContext,
            string modelName, Type modelType)
        {
            if (modelType.IsValueType || typeof(string) == modelType)
            {
                object instance;
                if (GetValueTypeInstance(controllerContext, modelName, modelType, out instance))
                {
                    return instance;
                };
                return Activator.CreateInstance(modelType);
            }

            return SimpleModelBinding(controllerContext, modelType);
        }

        private object SimpleModelBinding(ControllerContext controllerContext, Type modelType)
        {
            object modelInstance = Activator.CreateInstance(modelType);
            foreach (PropertyInfo property in modelType.GetProperties())
            {
                //只針對simple model binding做動作.
                if (!property.CanWrite || (!property.PropertyType.IsValueType && property.PropertyType != typeof(string)))
                {
                    continue;
                }

                object propertyValue;
                if (GetValueTypeInstance(controllerContext, property.Name, property.PropertyType, out propertyValue))
                {
                    property.SetValue(modelInstance, propertyValue);
                }
            }

            return modelInstance;
        }

        private bool GetValueTypeInstance(ControllerContext controllerContext, string modelName, Type modelType, out object value)
        {
            var form = controllerContext.RequestContext.HttpContext.Request.Form;
            var queryString = controllerContext.RequestContext.HttpContext.Request.QueryString;

            string key = form.AllKeys.FirstOrDefault(x => string.Compare(x, modelName, StringComparison.OrdinalIgnoreCase) == 0);
            if (key != null)
            {
                value = Convert.ChangeType(form[key], modelType);
                return true;
            }

            string queryKey = queryString.AllKeys.FirstOrDefault(x => string.Compare(x, modelName, StringComparison.OrdinalIgnoreCase) == 0);
            if (queryKey != null)
            {
                value = Convert.ChangeType(queryString[queryKey], modelType);
                return true;
            }

            value = null;
            return false;
        }
    }
}