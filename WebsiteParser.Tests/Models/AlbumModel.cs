using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteParser.Attributes.StartAttributes;

namespace WebsiteParser.Tests.Models
{
    class AlbumModel
    {
        [WebsiteParserList]
        public IEnumerable<SongModel> Songs { get; set; }

        [WebsiteParserModel]
        public MetadataModel MetaData { get; set; }
    }
}
