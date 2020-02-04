using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Gateway.WebAPI.Helpers
{
    public static class PropertySetter
    {
        public static void SetPropertyValue(this object parent, string propertyName, object value)
        {
            var inherType = parent.GetType();
            while (inherType != null)
            {
                PropertyInfo propToSet = inherType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                if (propToSet != null && propToSet.CanWrite)
                {
                    propToSet.SetValue(parent, value, null);
                    break;
                }

                inherType = inherType.BaseType;
            }
        }
    }
}
