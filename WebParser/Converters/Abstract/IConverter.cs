using System;
using System.Collections.Generic;
using System.Text;

namespace WebParser.Converters.Abstract
{
    public interface IConverter
    {
        object Convert(object input);
    }
}
