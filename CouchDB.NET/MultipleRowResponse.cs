using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CouchDb.Net
{
    public class MultipleRowResponse<T>
    {
        public int total_rows { get; set; }
        public int offset { get; set; }
        public ResponseRow<T>[] rows { get; set; }
    }
}
