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
    public class RemoveUserFromRoleSteps
    {
        string userName;
        CouchDbMembershipUser user;
        
        [Given(@"I have a valid user with roles assigned")]
        public void GivenIHaveAValidUserWithRolesAssigned()
        {
            userName = Guid.NewGuid().ToString();
            user = Membership.CreateUser(userName, Guid.NewGuid().ToString()) as CouchDbMembershipUser;

            Roles.CreateRole("admin");
            Roles.CreateRole("webuser");

            Roles.AddUserToRole(userName, "admin");
            Roles.AddUserToRole(userName, "webuser");

            var userRoles = new List<string>(Roles.GetRolesForUser(userName));

            Assert.IsTrue(userRoles.Contains("admin"));
        }

        [When(@"I call the RemoveFromRole API")]
        public void WhenICallTheRemoveFromRoleAPI()
        {
            Roles.RemoveUserFromRole(userName, "admin");
        }

        [Then(@"the user is not assigned to that role anymore")]
        public void ThenTheUserIsNotAssignedToThatRoleAnymore()
        {
            var userRoles = new List<string>(Roles.GetRolesForUser(userName));

            Assert.IsFalse(userRoles.Contains("admin"));
        }

    }
}
