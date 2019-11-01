using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using WebsiteParser.Attributes.Abstract;
using WebsiteParser.Exceptions;

namespace WebsiteParser.Attributes.StartAttributes
{
    /// <summary>
    /// Parses fragment of current page into current property using another parsing class
    /// </summary>
    public class WebsiteParserModel : PropertyAwareAttribute, IStartAttribute
    {
        /// <summary>
        /// Found element will be root node
        /// </summary>
        public string Selector { get; set; }

        /// <summary>
        /// If selector won't be found, skip current property
        /// </summary>
        public bool SkipIfNotFound { get; set; }

        public object GetValue(HtmlNode rootNode, out bool canParse)
        {
            canParse = true;

            if (!string.IsNullOrEmpty(Selector))
            {
                rootNode = rootNode.QuerySelector(Selector);

                if (rootNode == null)
                {
                    if (!SkipIfNotFound)
                        throw new ElementNotFoundException(Selector);

                    canParse = false;
                }

            }

            return WebContentParser.Parse(PropertyType, rootNode.InnerHtml);
        }
    }
}
