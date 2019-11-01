using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebsiteParser.Attributes.Abstract;
using WebsiteParser.Exceptions;

namespace WebsiteParser.Attributes.StartAttributes
{
    /// <summary>
    /// Mandatory attribute for property which you want to be parsed. It gets text or attribute's value of selected node.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SelectorAttribute : Attribute, IStartAttribute
    {
        public SelectorAttribute(string selector)
        {
            Selector = selector;
        }

        /// <summary>
        /// Name of attribute whose value will be gathered instead of text.
        /// </summary>
        public string Attribute { get; set; }
        /// <summary>
        /// Don't parse and don't throw error when selector doesn't exist. Recommended only for optional values.
        /// </summary>
        public bool SkipIfNotFound { get; set; }
        /// <summary>
        /// If markup / attribute value will be one of these it will be considered as empty (skipped with no exception)
        /// </summary>
        public string[] EmptyValues { get; set; }
        public string Selector { get; }

        public object GetValue(HtmlNode node, out bool canParse)
        {
            canParse = true;
            string value;

            HtmlNode valueNode = node.QuerySelector(Selector);

            if (valueNode == null)
            {
                canParse = false;

                if (SkipIfNotFound)
                    return null;
                else
                    throw new ElementNotFoundException(Selector);
            }
            else if (!string.IsNullOrEmpty(Attribute) && !valueNode.Attributes.Any(a => a.Name == Attribute))
            {
                if (SkipIfNotFound)
                    return null;
                else
                    throw new ElementNotFoundException($"Element {valueNode.Name} doesn't have attribute: {Attribute}", Selector);
            }

            value = string.IsNullOrEmpty(Attribute) ? valueNode.InnerText : valueNode.Attributes[Attribute].Value;
            value = value.Trim();

            if (EmptyValues != null && EmptyValues.Contains(value))
            {
                canParse = false;
                return null;
            }

            return value;
        }
    }
}
