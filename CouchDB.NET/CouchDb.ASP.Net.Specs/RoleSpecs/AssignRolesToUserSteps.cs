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
    public class AssignRolesToUserSteps
    {
        CouchDbMembershipUser user;
        string selectedRole;

        [Given(@"I have a valid CouchDbMembershipUser")]
        public void GivenIHaveAValidCouchDbMembershipUser()
        {
            user = Membership.CreateUser(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()) as CouchDbMembershipUser;
        }

        [When(@"I assign a role to a user")]
        public void WhenIAssignARoleToAUser()
        {
            selectedRole = TestHelper.GetRandomRole();

            Roles.AddUsersToRole(new string[] { user.UserName }, selectedRole);
        }

        [Then(@"I can check that the user has the role")]
        public void ThenICanCheckThatTheUserHasTheRole()
        {
            Assert.IsTrue(Roles.IsUserInRole(user.UserName, selectedRole));
        }

    }
}
