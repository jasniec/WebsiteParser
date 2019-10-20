using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteParser;
using WebsiteParser.Attributes;

namespace WebParser.Tests
{
    [TestClass]
    public class GeneralTests
    {
        [TestMethod]
        public void Run()
        {
            var result = WebContentParser.Parse<Metal>(WebsiteParser.Tests.Properties.Resources.Metal);
        }
    }

    class Metal
    {
        [Selector(@"#band_stats dl:first-child dd:nth-child(2)")]
        public string Sth { get; set; }
    }
}
