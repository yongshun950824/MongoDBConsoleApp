using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace MongoDBConsoleApp
{
    public static class EnumExtensions
    {
        public static string GetEnumMemberValue<TEnum>(this TEnum @enum)
            where TEnum : struct, Enum
        {
            string enumMemberValue = @enum.GetType()
                .GetMember(@enum.ToString())
                .FirstOrDefault()?
                .GetCustomAttributes<EnumMemberAttribute>(false)
                .FirstOrDefault()?
                .Value;

            if (enumMemberValue == null)
                return @enum.ToString();
                //throw new ArgumentException($"Enum {@enum.GetType().Name} with member {@enum} not applies the {nameof(EnumMemberAttribute)} attribute.");

            return enumMemberValue;
        }

        public static TEnum EnumMemberValueToEnum<TEnum>(this string value)
            where TEnum : struct, Enum
        {
            foreach (var field in typeof(TEnum).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                    typeof(EnumMemberAttribute)) is EnumMemberAttribute attribute)
                {
                    if (attribute.Value == value)
                        return (TEnum)field.GetValue(null);
                }

                if (field.Name == value)
                    return (TEnum)field.GetValue(null);
            }

            throw new ArgumentException($"{value} is not found.");
        }
    }
}
