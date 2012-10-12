using System;
using System.Collections.Generic;
using System.Linq;
using EasyHttp.Http;

namespace CouchDb.Net
{
    public class CouchDbRepository : ICouchDbRepository
    {
        //private readonly string recordType;
        private CouchDbUrlGenerator urlGenerator;

        public CouchDbRepository(string databaseServer, int databaseServerPort, string databaseName)
        {
            //this.recordType = recordType;
            urlGenerator = new CouchDbUrlGenerator(databaseServer, databaseServerPort, databaseName);
        }

        public virtual IEnumerable<dynamic> GetAll(string designDocument, string viewName)
        {
            var http = getHttpClient();
            
            var response = http.Get(urlGenerator.View(designDocument, viewName));

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return new List<dynamic>();

            dynamic body = response.DynamicBody;

            return body.rows;
        }

        public virtual List<T> GetAll<T>(string designDocument, string viewName)
        {
            var http = getHttpClient();
            var response = http.Get(urlGenerator.View(designDocument, viewName));

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return new List<T>();

            var staticResponse = http.Response.StaticBody<MultipleRowResponse<T>>();

            return (from r in staticResponse.rows select r.value).ToList();
        }

        public virtual dynamic GetById(string id)
        {
            var response = getHttpClient().Get(urlGenerator.Record(id));

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

            return response.DynamicBody;
        }

        public virtual T GetById<T>(string id)
        {
            var response = getHttpClient().Get(urlGenerator.Record(id));

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return default(T);

            return response.StaticBody<T>();
        }

        public virtual dynamic GetByKey(string designDocument, string view, string key)
        {
            var response = getHttpClient().Get(urlGenerator.ViewWithKey(designDocument, view, key));

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

            return response.DynamicBody;
        }

        public virtual T GetByKey<T>(string designDocument, string view, string key)
        {
            var response = getHttpClient().Get(urlGenerator.ViewWithKey(designDocument, view, key));

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return default(T);

            return response.StaticBody<MultipleRowResponse<T>>().rows[0].value;
        }

        public virtual List<T> GetAllByKey<T>(string designDocument, string view, string key)
        {
            var http = getHttpClient();
            var response = http.Get(urlGenerator.ViewWithKey(designDocument, view, key));

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return new List<T>();

            var staticResponse = http.Response.StaticBody<MultipleRowResponse<T>>();

            return (from r in staticResponse.rows select r.value).ToList();
        }

        public virtual void SaveWithId(string id, dynamic model)
        {
            if (model._rev == null) model._rev = String.Empty;
            getHttpClient().Put(urlGenerator.Record(id), model, HttpStrings.jsonAcceptString);
        }

        //public virtual void SaveNewWithId(string id, object model)
        //{
        //    getHttpClient().Put(urlGenerator.Record(id), model, HttpStrings.jsonAcceptString);
        //}

        public virtual void Delete(string id)
        {
            var response = getHttpClient().Get(urlGenerator.Record(id));

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return;

            dynamic document = response.DynamicBody;
            
            getHttpClient().Delete(urlGenerator.RecordWithRevision(id, document._rev));
        }

        private HttpClient getHttpClient()
        {
            var http = new HttpClient();
            http.Request.Accept = HttpStrings.jsonContentTypeString;
            return http;
        }

    }
}
