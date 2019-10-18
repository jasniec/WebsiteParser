using System;
using System.Collections.Generic;
using System.Text;
using WebParser.Converters.Abstract;

namespace WebParser.Converters
{
    /// <summary>
    /// Default enum converter. Parses <see cref="string"/> into <see cref="T"/> enum.
    /// </summary>
    /// <typeparam name="T">Enum type</typeparam>
    public class EnumConverter<T> : IConverter where T : Enum
    {
        public object Convert(object input)
        {
            if (!(input is string))
                throw new Exception("Input value have to be string");

            return Enum.Parse(typeof(T), (string)input);
        }
    }
}
