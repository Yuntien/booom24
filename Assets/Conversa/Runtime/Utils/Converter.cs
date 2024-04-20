using System.Globalization;
using System.Reflection;
using Conversa.Runtime.Interfaces;
using UnityEngine;

namespace Conversa.Runtime
{
    // This class is useful for connecting ports of different types
    public static class Converter
    {
        public static TReturn ConvertValue<TParam,TReturn>(TParam value)
        {
            return value switch
            {
                float floatValue when floatValue.ToString(CultureInfo.CurrentCulture) is TReturn castedValue1 => castedValue1,
                int intValue when intValue.ToString() is TReturn castedValue3 => castedValue3,
                bool boolValue when boolValue.ToString() is TReturn castedValue4 => castedValue4,
                TReturn directCastedValue => directCastedValue,
                _ => default
            };
        }

        public static T Convert<T>(IValueProperty property)
        {
            var methodInfo = typeof(Converter).GetMethod(nameof(ConvertValue), BindingFlags.Public | BindingFlags.Static);
            if (methodInfo == null)
            {
                Debug.LogError("Method converter not found");
                return default;
            }
            var genericMethodInfo = methodInfo.MakeGenericMethod(property.GetValueType(), typeof(T));
            var output = genericMethodInfo.Invoke(null, new[] { property.GetValueObject() });
            return output is T castedOutput ? castedOutput : default;
            
        }

        public static bool CanConvert(System.Type from, System.Type to) =>
            (from == to) ||
            (from == typeof(float) && to == typeof(string)) ||
            (from == typeof(bool) && to == typeof(string)) ||
            (from == typeof(int) && to == typeof(string));

    }
}