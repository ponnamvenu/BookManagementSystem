using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptArchitect.Utils
{
    public class ColumnAttribute:Attribute
    {
        public string Name { get; set; }

        public bool AllowNull { get; set; } = false;

        public string DataType { get; set; }

        public bool Exclude { get; set; } //true means this is not a column
    }
}
