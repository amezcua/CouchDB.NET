using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using CouchDb.ASP.NET;
using System.Web.Security;
using NUnit.Framework;

namespace CouchDb.ASP.Net.Specs.RoleSpecs
{
    [Binding]
    public class GetRolesForUserSteps
    {
        string userName;
        CouchDbMembershipUser user;
        string[] userRoles;


        [Given(@"I have a valid CouchDbMembershipUser with roles assigned")]
        public void GivenIHaveAValidCouchDbMembershipUserWithRolesAssigned()
        {
            userName = Guid.NewGuid().ToString();
            user = Membership.CreateUser(userName, Guid.NewGuid().ToString()) as CouchDbMembershipUser;

            Roles.CreateRole("admin");
            Roles.CreateRole("webuser");

            Roles.AddUserToRole(userName, "admin");
            Roles.AddUserToRole(userName, "webuser");
        }

        [When(@"I call the GetRolesForUser API")]
        public void WhenICallTheGetRolesForUserAPI()
        {
            userRoles = Roles.GetRolesForUser(userName);
        }

        [Then(@"I get a list of all the roles assigned to the user")]
        public void ThenIGetAListOfAllTheRolesAssignedToTheUser()
        {
            var roleList = new List<string>(userRoles);

            Assert.IsTrue(roleList.Contains("admin"));
            Assert.IsTrue(roleList.Contains("webuser"));
            Assert.IsTrue(roleList.Count == 2);
        }

    }
}
