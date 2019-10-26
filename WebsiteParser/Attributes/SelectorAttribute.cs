using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebsiteParser.Exceptions;

namespace WebsiteParser.Attributes
{
    /// <summary>
    /// Mandatory attribute for property which you want to be parsed. It gets text or attribute's value of selected node.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SelectorAttribute : Attribute
    {
        public SelectorAttribute(string selector)
        {
            Selector = selector;
        }

        /// <summary>
        /// Name of attribute whose value will be gathered instead of text.
        /// </summary>
        public string Attribute { get; set; }
        public string Selector { get; }

        public string GetValue(HtmlNode node)
        {
            string value;

            HtmlNode valueNode = node.QuerySelector(Selector);

            if (valueNode == null)
                throw new ElementNotFoundException($"Could not find element using: {Selector}", Selector);
            else if (!string.IsNullOrEmpty(Attribute) && !valueNode.Attributes.Any(a => a.Name == Attribute))
                throw new ElementNotFoundException($"Element {valueNode.Name} doesn't have attribute: {Attribute}", Selector);

            value = string.IsNullOrEmpty(Attribute) ? valueNode.InnerText : valueNode.Attributes[Attribute].Value;
            return value;
        }
    }
}
