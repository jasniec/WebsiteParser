using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace WebsiteParser.Attributes.Abstract
{
    public abstract class PropertyAwareAttribute : Attribute
    {
        public string PropertyName { get; private set; }
        public Type PropertyType { get; private set; }

        internal void SetPropertyInfo(PropertyInfo property)
        {
            PropertyName = property.Name;
            PropertyType = property.PropertyType;
        }

    }
}
