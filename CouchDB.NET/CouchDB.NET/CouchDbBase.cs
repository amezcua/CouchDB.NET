using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CouchDb.Net
{
    public abstract class CouchDbBase
    {
        private string couchDbId = "";
        private string couchDbRev = "";
        private string documentType = "";
        private DateTime created = DateTime.Now;

        protected CouchDbBase()
        {
            this.documentType = this.GetType().Name;
        }

        public virtual string _id
        {
            get { return couchDbId; }
            set { couchDbId = value; }
        }

        [DefaultValue("")]
        public string _rev {
            get { return couchDbRev; }
            set { couchDbRev = value; }
        }

        public virtual string DocumentType { 
            get { return documentType; }
            set { documentType = value; }
        }

        public virtual DateTime Created
        {
            get { return created; }
            set { created = value; }
        }
    }
}
