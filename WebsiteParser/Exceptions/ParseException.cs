using System;
using System.Collections.Generic;
using System.Text;

namespace WebsiteParser.Exceptions
{
    /// <summary>
    /// General parse exception
    /// </summary>
    public class ParseException : Exception
    {
        public ParseException(string propertyName, string className) : base($"An error occurred in {className}.{propertyName} during parse process.")
        {
            PropertyName = propertyName;
            ClassName = className;
        }

        public ParseException(string propertyName, string className, Exception innerException) : base($"An error occurred in {className}.{propertyName} during parse process. See inner message for more details", innerException)
        {
            PropertyName = propertyName;
            ClassName = className;
        }

        /// <summary>
        /// Property's name where error occurred
        /// </summary>
        public string PropertyName { get; }
        /// <summary>
        /// Class's name where error occurred
        /// </summary>
        public string ClassName { get; set; }
    }
}
