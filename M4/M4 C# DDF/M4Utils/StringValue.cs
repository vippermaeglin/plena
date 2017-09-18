using System;
using System.Reflection;

namespace M4Utils
{
    public class StringValue : Attribute
    {
        private readonly string _value;

        public StringValue(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }

        public static string GetStringValue(Enum value)
        {
            Type type = value.GetType();

            FieldInfo fi = type.GetField(value.ToString());

            StringValue[] attrs = fi.GetCustomAttributes(typeof(StringValue), false) as StringValue[];

            return ((attrs != null) && (attrs.Length > 0)) ? attrs[0].Value : null;
        }

        public static object Parse(Type type, string stringValue)
        {
            return Parse(type, stringValue, false);
        }

        public static object Parse(Type type, string stringValue, bool ignoreCase)
        {
            object objectParsed = null;
            string stringValueFinded = null;

            if (!type.IsEnum)
                throw new ArgumentException(string.Format("Deve ser informado um objeto do tipo Enum. Tipo era {0}", type.ToString()));

            foreach (FieldInfo fieldInfo in type.GetFields())
            {
                StringValue[] attributeArray = fieldInfo.GetCustomAttributes(typeof(StringValue), false) as StringValue[];

                if (attributeArray.Length > 0)
                    stringValueFinded = attributeArray[0].Value;

                if (string.Compare(stringValueFinded, stringValue, ignoreCase) != 0) 
                    continue;

                objectParsed = Enum.Parse(type, fieldInfo.Name);
                break;
            }

            return objectParsed;
        }
    }
}