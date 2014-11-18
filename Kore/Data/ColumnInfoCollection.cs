﻿using System.Collections.Generic;
using System.Linq;

namespace Kore.Data
{
    public sealed class ColumnInfoCollection : List<ColumnInfo>
    {
        public ColumnInfo this[string name]
        {
            get { return this.SingleOrDefault(x => x.ColumnName == name); }
        }

        public override string ToString()
        {
            return string.Concat("Count: ", this.Count);
        }
    }
}