using System;
using System.Text.RegularExpressions;
using WebsiteParser.Exceptions;

namespace WebsiteParser.Attributes
{
    /// <summary>
    /// Extracts value of first group of regex match.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RegexAttribute : Attribute
    {
        public RegexAttribute(string regex)
        {
            _regex = regex;
        }

        readonly string _regex;

        public string Extract(string input)
        {
            var match = Regex.Match(input, _regex);

            if (!match.Success)
                throw new RegexParseException(_regex);

            return match.Groups[1].Value.Trim();
        }

    }
}
