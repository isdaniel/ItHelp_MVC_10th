using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MVC_Sample.ValueProvider;
using FormValueProvider = MVC_Sample.ValueProvider.FormValueProvider;
using QueryStringValueProvider = MVC_Sample.ValueProvider.QueryStringValueProvider;

namespace MVC_Sample
{
    public class CustomerActionInvoker : IActionInvoker
    {

        public bool InvokeAction(ControllerContext controllerContext, string actionName)
        {
            //取得執行Action方法
            MethodInfo method = controllerContext.Controller
                .GetType()
                .GetMethods()
                .First(m => string.Compare(actionName, m.Name, StringComparison.OrdinalIgnoreCase) == 0);

            //取得Action使用的參數,並利用反射將值填充
            var parameters = method.GetParameters().Select(parameter =>
                BindModel(controllerContext, parameter.Name, parameter.ParameterType));

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
                }
                return Activator.CreateInstance(modelType);
            }

            return SimpleModelBinding(controllerContext, modelType);
        }

        private object SimpleModelBinding(ControllerContext controllerContext, Type modelType)
        {
            object modelInstance = Activator.CreateInstance(modelType);
            foreach (PropertyInfo property in modelType.GetProperties())
            {
                //針對基本型別或string型別給值
                if (!property.CanWrite || IsSimpleType(property))
                {
                    object propertyValue;
                    if (GetValueTypeInstance(controllerContext, property.Name, property.PropertyType, out propertyValue))
                    {
                        property.SetValue(modelInstance, propertyValue);
                    }
                }
            }

            return modelInstance;
        }

        private static bool IsSimpleType(PropertyInfo property)
        {
            return property.PropertyType == typeof(string) ||property.PropertyType.IsValueType;
        }

        private bool GetValueTypeInstance(ControllerContext controllerContext, string modelName, Type modelType, out object value)
        {
            List<ValueProviderBase> _valueProvider = new List<ValueProviderBase>()
            {
                new FormValueProvider(controllerContext),
                new QueryStringValueProvider(controllerContext)
            };

            foreach (var valueProvider in _valueProvider)
            {
                value = valueProvider.GetValue(modelName, modelType);
                if (value != null)
                    return true;
            }

            value = null;
            return false;
        }
    }
}