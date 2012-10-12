using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CouchDb.Net
{
    public class ResponseRow<T>
    {
        public string id { get; set; }
        public string key { get; set; }
        public T value { get; set; }
    }
}
