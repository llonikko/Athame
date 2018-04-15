using System.Collections.Generic;
using System.Linq;

namespace Athame.Core.Utils
{
    public static class Dictify
    {
        public static Dictionary<string, object> ObjectToDictionary(object o)
        {
            var t = o.GetType();
            var properties = t.GetProperties();
            return properties.Where(property => property.GetGetMethod() != null)
                .ToDictionary(property => property.Name, property => property.GetValue(o));
        }
    }
}
