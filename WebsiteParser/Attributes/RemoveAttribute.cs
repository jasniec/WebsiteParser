using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using WebsiteParser.Attributes.Enums;

namespace WebsiteParser.Attributes
{
    public class RemoveAttribute : Attribute
    {
        public RemoveAttribute(string value, RemoverValueType type)
        {
            _value = value;
            _type = type;
        }

        string _value;
        RemoverValueType _type;

        public object GetValue(object input)
        {
            string output = (string)input;

            switch (_type)
            {
                case RemoverValueType.Text:
                    output = output.Replace(_value, "");
                    break;
                case RemoverValueType.Regex:
                    output = Regex.Replace(output, _value, "");
                    break;
            }

            return output.Trim();
        }

    }
}
