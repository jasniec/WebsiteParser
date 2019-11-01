using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebsiteParser.Attributes.Abstract
{
    interface IStartAttribute
    {
        object GetValue(HtmlNode rootNode, out bool canParse);
    }
}
