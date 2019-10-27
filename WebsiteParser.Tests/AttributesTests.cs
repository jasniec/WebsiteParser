using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteParser.Attributes;
using WebsiteParser.Attributes.Enums;
using WebsiteParser.Tests.Properties;

namespace WebsiteParser.Tests
{
    [TestClass]
    public class AttributesTests
    {
        [TestMethod]
        public void ReplaceAttributeTextTest()
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(Resources.Metal);
            string input = document.QuerySelector(".band_comment").InnerText;

            RemoveAttribute remove = new RemoveAttribute("Read more", RemoverValueType.Text);
            string output = (string)remove.GetValue(input);

            string expected = "Played their final show of their very last tour entitled \"The End\" in Birmingham, England on February 4th, 2017. The band initially retired from large scale touring after the show, but later announced that they broke up.  Black Sabbath are generally considered both the first heavy metal and doom metal band. Originally they were called Polka Tulk (featuring a saxophonist and slide guitarist in ...";

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void ReplaceAttributeRegexTest()
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(Resources.Metal);
            string input = document.QuerySelector(".band_comment").InnerText;

            RemoveAttribute remove = new RemoveAttribute("Read more", RemoverValueType.Regex);
            string output = (string)remove.GetValue(input);

            string expected = "Played their final show of their very last tour entitled \"The End\" in Birmingham, England on February 4th, 2017. The band initially retired from large scale touring after the show, but later announced that they broke up.  Black Sabbath are generally considered both the first heavy metal and doom metal band. Originally they were called Polka Tulk (featuring a saxophonist and slide guitarist in ...";

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void FormatAttributeTest()
        {
            FormatAttribute attr = new FormatAttribute("pre-{0}-post");

            string actual = (string)attr.GetValue("input");
            string expected = "pre-input-post";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SelectorAttributeEmptyTest()
        {
            SelectorAttribute attr = new SelectorAttribute(".not-existing-class");
            attr.NotParseWhenNotFound = true;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml("");
            attr.GetContent(doc.DocumentNode, out bool canParse);

            Assert.AreEqual(false, canParse);
        }
    }
}
