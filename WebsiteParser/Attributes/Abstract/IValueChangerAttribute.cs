
using System;
using System.Collections.Generic;
using System.Text;

namespace WebsiteParser.Attributes.Abstract
{
    interface IValueChangerAttribute
    {
        object GetValue(object input);
    }
}
