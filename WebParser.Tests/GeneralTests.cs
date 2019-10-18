using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebParser.Attributes;

namespace WebParser.Tests
{
    [TestClass]
    public class GeneralTests
    {
        [TestMethod]
        public void ExampleWebsiteTest()
        {
            var result = WebContentParser.Parse<BandResult>(WebParser.Tests.Properties.Resources.metal_archives);
        }
    }
}
