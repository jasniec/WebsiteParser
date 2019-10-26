using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebsiteParser.Attributes;
using WebsiteParser.Exceptions;

namespace WebsiteParser
{
    /// <summary>
    /// Parses html page into model class
    /// </summary>
    public static class WebContentParser
    {
        /// <summary>
        /// Parses a page into a <see cref="T"/> object. Only properties with <see cref="SelectorAttribute"/> will be parsed
        /// </summary>
        /// <typeparam name="T">Model's type</typeparam>
        /// <param name="content">HTML string</param>
        /// <returns></returns>
        public static T Parse<T>(string content) where T : class, new()
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(content);

            return Parse<T>(document.DocumentNode);
        }

        /// <summary>
        /// Parses a page into list of <see cref="T"/> objects. <see cref="T"/> have to be decorated with <see cref="ListSelectorAttribute"/>. Only properties with <see cref="SelectorAttribute"/> will be parsed
        /// </summary>
        /// <typeparam name="T">Model's type</typeparam>
        /// <param name="content">HTML string</param>
        /// <returns></returns>
        public static IEnumerable<T> ParseList<T>(string content) where T : class, new()
        {
            ListSelectorAttribute listAttribute = typeof(T).GetCustomAttribute<ListSelectorAttribute>();
            if (listAttribute == null)
                throw new Exception("Can't parse model with no ListSelectorAttrubite to IEnumerable");

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(content);

            HtmlNode mainNode = document.DocumentNode.QuerySelector(listAttribute.Selector);
            IList<HtmlNode> listNodes;

            if (string.IsNullOrEmpty(listAttribute.ChildSelector))
                listNodes = mainNode.ChildNodes;
            else
                listNodes = mainNode.QuerySelectorAll(listAttribute.ChildSelector);

            foreach (var item in listNodes)
                yield return Parse<T>(item);
        }

        /// <summary>
        /// Parse a list item or a page
        /// </summary>
        /// <typeparam name="T">Target type</typeparam>
        /// <param name="node">Node with target's data</param>
        /// <returns></returns>
        private static T Parse<T>(HtmlNode node) where T : class, new()
        {
            T model = Activator.CreateInstance<T>();
            var props = typeof(T).GetProperties();

            foreach (var prop in props)
            {
                object value = null;

                SelectorAttribute selector = prop.GetCustomAttribute<SelectorAttribute>();
                if (selector != null)
                {
                    try
                    {
                        value = selector.GetValue(node);

                        foreach (var attrib in prop.GetCustomAttributes())
                        {
                            if (attrib is ConverterAttribute converter)
                                value = converter.ConverterInstance.Convert(value);
                            else if (attrib is RegexAttribute regex)
                                value = regex.Extract((string)value);
                            else if (attrib is DebugAttribute debug)
                                debug.LogValue(prop.Name, typeof(T).Name, value);
                        }

                        prop.SetValue(model, value);
                    }
                    catch(Exception ex)
                    {
                        throw new ParseException(prop.Name, typeof(T).Name, ex);
                    }
                }
            }

            return model;
        }

    }
}
