using System.Collections.Generic;
using System.Linq;

namespace Athame.Core.Utilities
{
    public static class Dictify
    {
        public static Dictionary<string, object> ObjectToDictionary(object o)
            => o.GetType()
                .GetProperties()
                .Where(property => property.GetGetMethod() != null)
                .ToDictionary(property => property.Name, property => property.GetValue(o));
    }
}
