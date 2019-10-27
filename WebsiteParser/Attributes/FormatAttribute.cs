using System;
using System.Collections.Generic;
using System.Text;
using WebsiteParser.Attributes.Abstract;

namespace WebsiteParser.Attributes
{
    public class FormatAttribute : Attribute, IParserAttribute
    {
        public FormatAttribute(string stringFormat)
        {
            _stringFormat = stringFormat;
        }

        string _stringFormat;

        public object GetValue(object input)
        {
            return string.Format(_stringFormat, input);
        }
    }
}
