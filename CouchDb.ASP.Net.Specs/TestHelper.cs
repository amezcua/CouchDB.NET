using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace CouchDb.ASP.Net.Specs
{
    internal class TestHelper
    {
        internal static string GetRandomRole()
        {
            var roles = Roles.GetAllRoles();

            var rnd = new Random();
            return roles[rnd.Next(roles.Length)];
        }
    }
}
