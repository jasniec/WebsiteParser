using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteParser;
using WebsiteParser.Attributes;
using WebsiteParser.Converters;
using WebsiteParser.Converters.Abstract;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace WebsiteParser.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        public void MetalArchivesArtist()
        {
            string html = WebsiteParser.Tests.Properties.Resources.Metal;

            var result = WebContentParser.Parse<ArtistPart>(html);

            Assert.AreEqual(Country.UnitedKingdom, result.Country);
            Assert.AreEqual(new DateTime(2002, 7, 17, 2, 35, 24), result.AddedDate);
        }
    }

    class ArtistPart
    {
        [Selector(@"#band_stats dl:first-child dd:nth-child(2)", EmptyValues = new string[] { "N/A" })]
        [Debug]
        [Converter(typeof(EnumDescriptionConverter<Country>))]
        [Debug]
        public Country Country { get; set; }

        [Selector(@"#auditTrail table tr:nth-child(2) td:first-child")]
        [Regex(@"(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2})")]
        [Converter(typeof(DateTimeConverter))]
        public DateTime AddedDate { get; set; }
    }

    enum Country
    {
        Default,
        [Description("United Kingdom")]
        UnitedKingdom
    }

    class EnumDescriptionConverter<T> : IConverter where T : Enum
    {
        public object Convert(object input)
        {
            foreach (var field in typeof(T).GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == (string)input)
                        return field.GetValue(null);
                }
                else
                {
                    if (field.Name == (string)input)
                        return field.GetValue(null);
                }
            }

            return null;
        }
    }
}
