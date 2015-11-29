using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArtportalenApp.Helpers
{
    public class CustomAttributeHelper
    {
        public static string GetEnumDisplayValue(Enum value)
        {
            var fieldInfo = value.GetType().GetRuntimeField(value.ToString());
            var attribute = GetAttribute<DisplayAttribute>(fieldInfo);

            return (attribute != null) ? attribute.GetName() : value.ToString();
        }

        public static T GetAttribute<T>(MemberInfo member) where T : Attribute
        {
            var attributes = GetAttributes<T>(member);
            return (attributes.Length > 0) ? attributes[0] : default(T);
        }

        public static T[] GetAttributes<T>(MemberInfo member) where T : Attribute
        {
            return member.GetCustomAttributes(typeof (T), false).Cast<T>().ToArray();
        }
    }
}
