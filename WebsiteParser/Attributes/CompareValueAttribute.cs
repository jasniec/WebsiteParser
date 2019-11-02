using System;
using System.Collections.Generic;
using System.Text;
using WebsiteParser.Attributes.Abstract;

namespace WebsiteParser.Attributes
{
    /// <summary>
    /// Compares if received value equals (or not) expected value
    /// </summary>
    public class CompareValueAttribute : IParserAttribute
    {
        /// <summary>
        /// Default constructor of CompareValueAttribute
        /// </summary>
        /// <param name="expected">Expected result to compare</param>
        /// <param name="equals">should equals or not equals</param>
        public CompareValueAttribute(string expected, bool equals)
        {
            _expected = expected;
            _equals = equals;
        }

        readonly string _expected;
        readonly bool _equals;

        public object GetValue(object input)
        {
            return ((string)input).Equals(_expected) && _equals;
        }
    }
}
