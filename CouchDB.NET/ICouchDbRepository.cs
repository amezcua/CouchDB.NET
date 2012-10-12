using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CouchDb.Net
{
    interface ICouchDbRepository
    {
        IEnumerable<dynamic> GetAll(string designDocument, string viewName);
        dynamic GetById(string id);
        void SaveWithId(string id, dynamic model);
        void Delete(string id);
    }
}
