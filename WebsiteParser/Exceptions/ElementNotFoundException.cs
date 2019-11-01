using System;
using System.Collections.Generic;
using System.Text;

namespace WebsiteParser.Exceptions
{
    public class ElementNotFoundException : Exception
    {
        public ElementNotFoundException(string selector) : base($"Could not find element using: {selector}")
        {
            Selector = selector;
        }

        public ElementNotFoundException(string message, string selector) : base(message)
        {
            Selector = selector;
        }

        public string Selector { get; }
    }
}
