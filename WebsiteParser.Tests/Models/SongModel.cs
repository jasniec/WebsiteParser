using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteParser.Attributes;
using WebsiteParser.Attributes.StartAttributes;

namespace WebsiteParser.Tests.Models
{
    [ListSelector(".table_lyrics tbody", ChildSelector = ".even, .odd")]
    class SongModel
    {
        [Selector(".wrapWords")]
        public string Name { get; set; }
    }
}
