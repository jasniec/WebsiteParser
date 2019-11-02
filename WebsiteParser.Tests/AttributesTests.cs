using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using WebsiteParser.Attributes;
using WebsiteParser.Attributes.Enums;
using WebsiteParser.Attributes.StartAttributes;
using WebsiteParser.Tests.Models;
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
            attr.SkipIfNotFound = true;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml("");
            attr.GetValue(doc.DocumentNode, out bool canParse);

            Assert.AreEqual(false, canParse);
        }

        [TestMethod]
        public void SelectorAttributeEmptyValuesTest()
        {
            SelectorAttribute attr = new SelectorAttribute(".class");
            attr.EmptyValues = new string[] { "content" };

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml("<div class='class'>content</div>");
            attr.GetValue(doc.DocumentNode, out bool canParse);

            Assert.AreEqual(false, canParse);
        }

        [TestMethod]
        public void WebsiteParserList()
        {
            WebsiteParserList wpl = new WebsiteParserList();
            wpl.SetPropertyInfo(typeof(AlbumModel).GetProperties().First());

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(Resources.SongContent);

            var result = ((IEnumerable<SongModel>)wpl.GetValue(document.DocumentNode, out bool canParse)).ToList();


            Assert.AreEqual(true, canParse);
            Assert.AreEqual(8, result.Count());
        }

        [TestMethod]
        public void WebsiteParserModel()
        {
            WebsiteParserModel wpm = new WebsiteParserModel();
            wpm.SetPropertyInfo(typeof(AlbumModel).GetProperties().ElementAt(1));

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(Resources.SongContent);

            var result = (MetadataModel)wpm.GetValue(document.DocumentNode, out bool canParse);

            var expected = new MetadataModel()
            { 
                LastModifiedBy = "Viral",
                AddedBy = "Added by: (Unknown user)",
                LastModifiedDate = new DateTime(2018, 12, 22, 1, 6, 55)
            };


            Assert.AreEqual(true, canParse);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CompareValueAttributeTest()
        {
            CompareValueAttribute cva = new CompareValueAttribute("expected value", true);

            bool result = (bool)cva.GetValue("expected value");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CompareValueAttributeNegativeTest()
        {
            CompareValueAttribute cva = new CompareValueAttribute("expected value", true);

            bool result = (bool)cva.GetValue("not expected value");

            Assert.IsFalse(result);
        }

    }
}
