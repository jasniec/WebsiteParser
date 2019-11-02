using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using WebsiteParser.Attributes.Abstract;
using WebsiteParser.Exceptions;

namespace WebsiteParser.Attributes.StartAttributes
{
    /// <summary>
    /// Parses fragment of current page into current property as <see cref="System.Collections.IEnumerable"/> using another parsing class
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class WebsiteParserList : PropertyAwareAttribute, IStartAttribute
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
            if (!PropertyType.GetInterfaces().Contains(typeof(IEnumerable)) || !PropertyType.IsGenericType)
                throw new Exception("Property have to be a generic ienumerable");

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

            Type itemType = PropertyType.GetGenericArguments()[0];

            return CastIenumerableToGeneric(WebContentParser.ParseList(itemType, rootNode.InnerHtml), itemType);
        }

        object CastIenumerableToGeneric(IEnumerable items, Type type)
        {
            var typeOfList = typeof(List<>).MakeGenericType(type);
            var list = (IList)Activator.CreateInstance(typeOfList);

            IEnumerator enumerator = items.GetEnumerator();
            while (enumerator.MoveNext())
                list.Add(enumerator.Current);

            return list;
        }
    }
}
