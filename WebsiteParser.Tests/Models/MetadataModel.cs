using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteParser.Attributes;
using WebsiteParser.Attributes.StartAttributes;
using WebsiteParser.Converters;

namespace WebsiteParser.Tests.Models
{
    class MetadataModel
    {
        [Selector(@"#auditTrail table tr:first-child td:first-child")]
        public string AddedBy { get; set; }

        [Selector(@"#auditTrail table tr:nth-child(2) td:first-child", EmptyValues = new string[] { "Added on: N/A" })]
        [Regex(@"(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2})")]
        [Converter(typeof(DateTimeConverter))]
        public DateTime AddDate { get; set; }

        [Selector(@"#auditTrail table tr:first-child td:last-child")]
        public string LastModifiedBy { get; set; }

        [Selector(@"#auditTrail table tr:nth-child(2) td:nth-child(2)", EmptyValues = new string[] { "Added on: N/A" })]
        [Regex(@"(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2})")]
        [Converter(typeof(DateTimeConverter))]
        public DateTime LastModifiedDate { get; set; }

        public override bool Equals(object obj)
        {
            MetadataModel model = (MetadataModel)obj;

            return AddedBy == model.AddedBy
                || AddDate == model.AddDate
                || LastModifiedBy == model.LastModifiedBy
                || LastModifiedDate == model.LastModifiedDate;
        }
    }
}
