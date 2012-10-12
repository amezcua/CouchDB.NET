using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyHttp.Http;
using System.Web.Security;
using System.Web.Configuration;
using System.Collections.Specialized;

namespace TestWebSitePopulation
{
    class Program
    {
        static string regularUserName = "user@couchdb.test";
        static string adminUserName = "admin@couchdb.test";
        static string password = "couchdb";
        static string adminRoleName = "Admin";
        static string regularUserRoleName = "RegularUser";
        
        static void Main(string[] args)
        {
            string couchDbServerName = MembershipCouchDbServerName;
            int couchdbport = MembershipCouchDbServerPort;
            string testdatabasename = MembershipCouchDbDatabaseName;

            Console.WriteLine("Checking if database '" + testdatabasename + "' exists...");

            var response = createDatabase();
            if (response == System.Net.HttpStatusCode.Created) Console.WriteLine("Database '" + testdatabasename + "' created.");
            if (response == System.Net.HttpStatusCode.PreconditionFailed) Console.WriteLine("Database '" + testdatabasename + "' already exists.");
            if (response == System.Net.HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("CouchDB is protected with admin accounts. This version of the application requires no admin accounts.");
                printExit();
                return;
            }

            Console.WriteLine("Creating views...");
            createViews();

            Console.WriteLine("Creating roles...");
            Roles.CreateRole(adminRoleName);
            Roles.CreateRole(regularUserRoleName);

            try
            {
                Console.WriteLine("Creating admin account...");
                Membership.CreateUser(adminUserName, password, adminUserName);
                Roles.AddUserToRole(adminUserName, adminRoleName);
            }
            catch (MembershipCreateUserException)
            {
                Console.WriteLine("Admin account already exists.");
            }

            try
            {
                Console.WriteLine("Creating normal user account...");
                Membership.CreateUser(regularUserName, password, regularUserName);
                Roles.AddUserToRole(regularUserName, regularUserRoleName);
            }
            catch (MembershipCreateUserException)
            {
                Console.WriteLine("Normal user account already exists.");
            }

            Console.WriteLine();
            Console.WriteLine("Database configured.");
            Console.WriteLine("Configure the ASP.NET MVC application to point to the same database and try to logon with the regular user '" + regularUserName + "' and with the admin user '" + adminUserName + "', both with password '" + password + "', all without quotes, to test ASP.NET features.");

            printExit();
        }

        static void printExit()
        {
            Console.WriteLine();
            Console.WriteLine("Finished. Press ENTER to exit.");
            Console.ReadLine();
        }

        static string getBaseUrl()
        {
            return new UriBuilder("http", MembershipCouchDbServerName, MembershipCouchDbServerPort).ToString();
        }

        static System.Net.HttpStatusCode createDatabase()
        {
            var url = new StringBuilder(getBaseUrl()).Append(MembershipCouchDbDatabaseName).ToString();
            var http = new HttpClient();
            http.Request.Accept = "application/json";
            http.Put(url, String.Empty, "application/json");
            return http.Response.StatusCode;
        }

        static void createViews()
        {
            // membership_user
            var url = new StringBuilder(getBaseUrl()).Append(MembershipCouchDbDatabaseName).Append("/_design/membership_user").ToString();
            var http = new HttpClient();
            http.Request.Accept = "application/json";
            
            string all_view = "function(doc) { if(doc.type == 'CouchDbMembershipUser')  { emit(doc.UserName, doc) ; } }";
            string all_by_email = "function(doc) { if(doc.UserName) { emit(doc.Email, doc); }}";
            string by_single_role = "function(doc) { if(doc.type == 'CouchDbMembershipUser')  { if(doc.Roles.length > 0) { for(var idx in doc.Roles) { emit(doc.Roles[idx], doc); } } } }";

            var all_map = new { map = all_view };
            var all_by_email_map = new { map = all_by_email };
            var by_single_role_map = new { map = by_single_role };

            var views_document = new { _id = "_design/membership_user", views = new { all = all_map, all_by_email = all_by_email_map, by_single_role = by_single_role_map } };
            http.Put(url, views_document, "application/json");
        }

        static string MembershipCouchDbServerName
        {
            get
            {
                var membershipSection = (MembershipSection)WebConfigurationManager.GetWebApplicationSection("system.web/membership");
                return membershipSection.Providers[membershipSection.DefaultProvider].Parameters["couchDbServerName"];
            }
        }

        static int MembershipCouchDbServerPort
        {
            get
            {
                var membershipSection = (MembershipSection)WebConfigurationManager.GetWebApplicationSection("system.web/membership");
                return Int32.Parse(membershipSection.Providers[membershipSection.DefaultProvider].Parameters["couchDbServerPort"]);
            }
        }

        static string MembershipCouchDbDatabaseName
        {
            get
            {
                var membershipSection = (MembershipSection)WebConfigurationManager.GetWebApplicationSection("system.web/membership");
                return membershipSection.Providers[membershipSection.DefaultProvider].Parameters["couchDbDatabaseName"];
            }
        }
    }
}
