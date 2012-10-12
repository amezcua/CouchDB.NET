using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CouchDb.Net;

namespace CouchDb.ASP.NET
{
    internal class CouchDbRolesRepository : CouchDbRepository
    {
        private static readonly string rolesDocumentId = "membership_roles";

        public CouchDbRolesRepository(string databaseServer, int databaseServerPort, string databaseName)
            : base(databaseServer, databaseServerPort, databaseName)
        {
            
        }

        internal string[] GetRoles()
        {
            var rolesDocument = GetRolesDocument();

            if (rolesDocument == null)
            {
                string[] empty = { };
                return empty;
            }
            return rolesDocument.All;
        }

        internal CouchDbRoles GetRolesDocument()
        {
            return base.GetById<CouchDbRoles>(rolesDocumentId);
        }

        internal void AddRole(string roleName)
        {
            var rolesDocument = GetRolesDocument();

            if (rolesDocument != null && rolesDocument.HasRole(roleName)) return;

            if (rolesDocument == null)
            {
                rolesDocument = new CouchDbRoles();
            }

            var newRoles = new List<string>(rolesDocument.All);
            newRoles.Add(roleName);
            rolesDocument.All = newRoles.ToArray();

            SaveRolesDocument(rolesDocument);
        }

        internal void SaveRolesDocument(CouchDbRoles rolesDocument)
        {
            SaveWithId(rolesDocumentId, rolesDocument);
        }
    }
}
