using System;
using System.Collections.Generic;
using System.Text;

namespace WebsiteParser.Exceptions
{
    public class RegexParseException : Exception
    {
        public RegexParseException(string regex) : base("Regex match went wrong")
        {
            Regex = regex;
        }

        public string Regex { get; }
    }
}
