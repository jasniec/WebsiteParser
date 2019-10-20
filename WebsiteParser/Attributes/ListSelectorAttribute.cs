using System;
using System.Collections.Generic;
using System.Text;

namespace WebsiteParser.Attributes
{
    /// <summary>
    /// Declares list container. You can specify children nodes using <see cref="ChildSelector"/> property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ListSelectorAttribute : Attribute
    {
        public ListSelectorAttribute(string selector)
        {
            Selector = selector;
        }

        public string ChildSelector { get; set; }
        public string Selector { get; }
    }
}
