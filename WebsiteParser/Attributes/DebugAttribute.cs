using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace WebsiteParser.Attributes
{
    /// <summary>
    /// It prints actual state of parsed value in a output window
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class DebugAttribute : Attribute
    {
        /// <summary>
        /// If there should be class name shown
        /// </summary>
        public bool ShowClass { get; set; } = true;
        /// <summary>
        /// If there sould be type shown
        /// </summary>
        public bool ShowType { get; set; } = true;

        internal void LogValue(string propName, string propClass, object value)
        {
            StringBuilder sb = new StringBuilder();

            if (ShowClass)
            {
                sb.Append("[Clas]: ");
                sb.Append(propClass);
                sb.Append(" ");
            }

            sb.Append("[Prop]: ");
            sb.Append(propName);

            if (ShowType)
            {
                sb.Append(" [Type]: ");
                sb.Append(value.GetType().Name);
            }
            sb.Append(" [Value]: ");
            sb.Append(value.ToString());

            Debug.WriteLine(sb.ToString());
        }

    }
}
