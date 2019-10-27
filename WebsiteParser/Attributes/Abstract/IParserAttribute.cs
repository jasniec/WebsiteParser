using System;
using System.Collections.Generic;
using System.Text;

namespace WebsiteParser.Attributes.Abstract
{
    /// <summary>
    /// Implementing this interface on an attribute makes it visible for parser.
    /// </summary>
    public interface IParserAttribute
    {
        /// <summary>
        /// Changes input value and/or its type.
        /// </summary>
        /// <param name="input">Input received from an attribute above</param>
        /// <returns>New value of variable</returns>
        object GetValue(object input);
    }
}
