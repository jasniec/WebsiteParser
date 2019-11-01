using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebsiteParser.Attributes;
using WebsiteParser.Attributes.Abstract;
using WebsiteParser.Attributes.StartAttributes;
using WebsiteParser.Exceptions;

namespace WebsiteParser
{
    /// <summary>
    /// Parses html page into model class
    /// </summary>
    public static class WebContentParser
    {
        #region Single result
        /// <summary>
        /// Parses a page into a <see cref="T"/> object. Only properties with <see cref="IStartAttribute"/> will be parsed
        /// </summary>
        /// <typeparam name="T">Model's type</typeparam>
        /// <param name="content">HTML string</param>
        /// <returns></returns>
        public static T Parse<T>(string content) where T : class, new()
        {
            return Parse(typeof(T), content) as T;
        }

        /// <summary>
        /// Parses a page into a object. Only properties with <see cref="IStartAttribute"/> will be parsed
        /// </summary>
        /// <param name="type">Class's type</param>
        /// <param name="content">HTML string</param>
        /// <returns></returns>
        public static object Parse(Type type, string content)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(content);

            return Parse(type, document.DocumentNode);
        }
        #endregion

        #region List
        /// <summary>
        /// Parses a page into list of <see cref="T"/> objects. <see cref="T"/> have to be decorated with <see cref="ListSelectorAttribute"/>. Only properties with <see cref="IStartAttribute"/> will be parsed
        /// </summary>
        /// <typeparam name="T">Model's type</typeparam>
        /// <param name="content">HTML string</param>
        /// <returns></returns>
        public static IEnumerable<T> ParseList<T>(string content) where T : class, new()
        {
            return ParseList(typeof(T), content).Cast<T>();
        }

        /// <summary>
        /// Parses a page into list of objects. Class of <see cref="Type"/> have to be decorated with <see cref="ListSelectorAttribute"/>. Only properties with <see cref="IStartAttribute"/> will be parsed
        /// </summary>
        /// <param name="type">Model's type</param>
        /// <param name="content">HTML string</param>
        /// <returns></returns>
        public static IEnumerable ParseList(Type type, string content)
        {
            ListSelectorAttribute listAttribute = type.GetCustomAttribute<ListSelectorAttribute>();
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
                yield return Parse(type, item);
        }
        #endregion

        #region Private
        /// <summary>
        /// Parse a list item or a page
        /// </summary>
        /// <typeparam name="T">Target type</typeparam>
        /// <param name="node">Node with target's data</param>
        /// <returns></returns>
        private static object Parse(Type type, HtmlNode node)
        {
            object model = Activator.CreateInstance(type);
            var props = type.GetProperties();

            foreach (var prop in props)
            {
                object value = null;
                var startAttributes = prop.GetCustomAttributes().OfType<IStartAttribute>();

                if (startAttributes?.Count() > 0)
                {
                    try
                    {
                        if (startAttributes.Count() > 1)
                            throw new TooManyStartAtributesException();

                        #region First Property
                        IStartAttribute firstAttrib = startAttributes.First();

                        if (firstAttrib is PropertyAwareAttribute firstPropAware)
                            firstPropAware.SetPropertyInfo(prop);

                        value = firstAttrib.GetValue(node, out bool canParse);
                        #endregion

                        if (canParse)
                        {
                            foreach (var attrib in prop.GetCustomAttributes().Where(i => i is IParserAttribute || i is DebugAttribute))
                            {
                                if (attrib is PropertyAwareAttribute propAware)
                                    propAware.SetPropertyInfo(prop);

                                if (attrib is DebugAttribute debug)
                                    debug.LogValue(prop.Name, type.Name, value);
                                else
                                    value = ((IParserAttribute)attrib).GetValue(value);
                            }

                            prop.SetValue(model, value);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ParseException(prop.Name, type.Name, ex);
                    }
                }
            }

            return model;
        }
        #endregion

    }
}
