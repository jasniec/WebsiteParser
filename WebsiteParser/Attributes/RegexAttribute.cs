using System;
using System.Text.RegularExpressions;

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

        internal string Extract(string input)
        {

            var match = Regex.Match(input, _regex);

            if (!match.Success)
                throw new Exception("Regex parse went wrong");

            return match.Groups[1].Value;
        }

    }
}
