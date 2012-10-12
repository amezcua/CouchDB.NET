using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CouchDb.Net;

namespace CouchDb.ASP.NET
{
    public class CouchDbRoles : CouchDbBase
    {
        private CouchDbRolesRepository repository;

        public CouchDbRoles()
        {
            All = new string[] { };
        }

        private string documentId = "membership_roles";
        public override string _id
        {
            get
            {
                return documentId;
            }
            set
            {
                // Nothing
            }
        }

        private string[] all;
        public string[] All
        {
            get
            {
                return all;
            }
            set
            {
                all = value;
            }
        }

        internal bool HasRole(string roleName)
        {
            foreach (string role in this.All)
            {
                if (role == roleName) return true;
            }
            return false;
        }

        internal bool DeleteRole(string roleName)
        {
            bool eliminado;
            var roleList = new List<string>(all);
            if(eliminado = roleList.Remove(roleName))
                this.all = roleList.ToArray();

            Save();
            
            return eliminado;
        }

        internal void Save()
        {
            if (String.IsNullOrEmpty(_id)) _id = Guid.NewGuid().ToString();
            getRepository().SaveWithId(_id, this);
        }

        internal CouchDbRolesRepository getRepository()
        {
            if (repository == null) repository = new CouchDbRolesRepository(ConfigurationHelper.RolesCouchDbServerName, ConfigurationHelper.RolesCouchDbServerPort, ConfigurationHelper.RolesCouchDbDatabaseName);
            return repository;
        }
    }
}
