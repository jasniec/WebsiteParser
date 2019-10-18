using System;
using System.Text.RegularExpressions;
using WebParser.Converters.Abstract;

namespace WebParser.Converters
{
    /// <summary>
    /// Default regex converters. Input have to be <see cref="string"/>, output is <see cref="string"/>.
    /// </summary>
    public class RegexConverter : IConverter
    {
        public RegexConverter(string regex)
        {
            _regex = regex;
        }

        string _regex;

        public object Convert(object input)
        {
            if (!(input is string))
                throw new Exception("Input value have to be string");

            var match = Regex.Match((string)input, _regex);

            if (!match.Success)
                throw new Exception("regex parse went wrong");

            return match.Groups[1].Value;
        }
    }
}
