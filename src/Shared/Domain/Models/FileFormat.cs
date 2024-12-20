namespace Domain.Models
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class StringValueAttribute : Attribute
    { 
        public string Value { get; }
        public StringValueAttribute(string value) => Value = value;
    }
    public enum FileFormat : int 
    {
        [StringValue(".csv")]
        csv,

        [StringValue(".json")]
        json,
    }

    public static class EnumExtensions 
    {
        public static string GetStringValue(this Enum value) 
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            if (memberInfo.Length > 0)
            {
                var attributes = memberInfo[0].GetCustomAttributes(typeof (StringValueAttribute), false);
                if (attributes.Length > 0)
                {
                    return ((StringValueAttribute)attributes[0]).Value;
                }
            }

            // If no attribute found
            return value.ToString();
        }
    }
}
