using System;
using System.Collections.Generic;
using System.Text;

namespace WebsiteParser.Exceptions
{
    public class TooManyStartAtributesException : Exception
    {
        public TooManyStartAtributesException() : base("There can only be one IStartAttribute")
        {

        }
    }
}
