using System;
using System.Collections.Generic;
using System.Text;

namespace Wells.Fargo.Domain.Model
{
    class ColumnAttribute : Attribute
    {
        public string Name { get; set; }
        public Type DataType { get; set; }
    }
}
