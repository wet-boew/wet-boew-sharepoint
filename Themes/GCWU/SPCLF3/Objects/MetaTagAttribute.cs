using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPCLF3.Objects
{
    public class MetaTagAttribute
    {

        public string Name { get; set; }
        public string Content { get; set; }


        public MetaTagAttribute(string name, string content)
        {
            this.Name = name;
            this.Content = content;
        }

        /// <summary>
        /// Generates the HTML output of the attribute.
        /// </summary>
        /// <returns>An HTML attribute.</returns>
        public string GenerateAttribute()
        {
            return String.Format("{0}=\"{1}\"", this.Name, this.Content);
        }

    }
}
