using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CouchDb.Net
{
    public class CouchDbUrlGenerator
    {
        private static readonly string DESIGN_URL_FRAGMENT = "/_design/";
        private static readonly string VIEW_URL_FRAGMENT = "/_view/";

        private readonly string databaseServer;
        private readonly int databaseServerPort;
        private readonly string databaseName;

        public CouchDbUrlGenerator(string serverName, int serverPort, string databaseName)
        {
            this.databaseServer = serverName;
            this.databaseServerPort = serverPort;
            this.databaseName = databaseName;
        }

        public string View(string designDocument, string viewName)
        {
            var builder = new StringBuilder(getDatabaseRootUrl());
            builder.Append(DESIGN_URL_FRAGMENT);
            builder.Append(designDocument);
            builder.Append(VIEW_URL_FRAGMENT);
            builder.Append(viewName);

            return builder.ToString();
        }

        public string ViewWithKey(string designDocument, string viewName, string keyValue)
        {
            var builder = new StringBuilder(getDatabaseRootUrl());
            builder.Append(DESIGN_URL_FRAGMENT);
            builder.Append(designDocument);
            builder.Append(VIEW_URL_FRAGMENT);
            builder.Append(viewName);
            builder.Append("?key=\"");
            builder.Append(keyValue);
            builder.Append("\"");

            return builder.ToString();
        }

        public string Record(string id)
        {
            var builder = new StringBuilder(getDatabaseRootUrl());
            builder.Append("/");
            builder.Append(id);
            
            return builder.ToString();
        }

        public string RecordWithRevision(string id, string revision)
        {
            var builder = new StringBuilder(getDatabaseRootUrl());
            builder.Append("/");
            builder.Append(id);
            builder.Append("?rev=");
            builder.Append(revision);

            return builder.ToString();
        }

        public string getDatabaseRootUrl()
        {
            var uriBuilder = new UriBuilder();
            uriBuilder.Host = databaseServer;
            uriBuilder.Port = databaseServerPort;
            uriBuilder.Path = databaseName;

            return uriBuilder.Uri.AbsoluteUri;
        }
    }
}
