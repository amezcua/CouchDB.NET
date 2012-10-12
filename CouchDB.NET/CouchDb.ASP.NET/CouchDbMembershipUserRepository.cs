using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CouchDb.Net;
using System.Web.Security;
using System.Dynamic;

namespace CouchDb.ASP.NET
{
    public class CouchDbMembershipUserRepository : CouchDbRepository
    {
        private static readonly string designDocument = "membership_user";
        private static readonly string view_all = "all";
        private static readonly string view_all_by_email = "all_by_email";
        private static readonly string view_by_single_role = "by_single_role";

        public CouchDbMembershipUserRepository(string databaseServer, int databaseServerPort, string databaseName)
            : base(databaseServer, databaseServerPort, databaseName)
        {
            
        }

        public IEnumerable<dynamic> GetAll()
        {
            return base.GetAll(designDocument, view_all);
        }

        public CouchDbMembershipUser GetUserByUserName(string userName)
        {
            dynamic results = base.GetByKey(designDocument, view_all, userName).rows;

            if (results.Length == 0) return null;

            return CouchDbMembershipUser.FromDynamicDbResponse(results[0].value); // TODO Vale con devolver solo el primero?
        }

        public CouchDbMembershipUser GetUserByEmail(string email)
        {
            dynamic results = base.GetByKey(designDocument, view_all_by_email, email).rows;

            if (results.Length == 0) return null;

            return CouchDbMembershipUser.FromDynamicDbResponse(results[0].value); // TODO Vale con devolver solo el primero?
        }

        public IEnumerable<dynamic> GetUsersByRole(string roleName)
        {
            return base.GetByKey(designDocument, view_by_single_role, roleName).rows;
        }
    }
}
