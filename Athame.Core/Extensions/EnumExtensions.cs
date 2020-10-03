using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Athame.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
            => value
                .GetType()
                .GetMember(value.ToString())
                .FirstOrDefault()
                ?.GetCustomAttribute<DescriptionAttribute>()
                ?.Description
            ?? value.ToString();
    }
}
