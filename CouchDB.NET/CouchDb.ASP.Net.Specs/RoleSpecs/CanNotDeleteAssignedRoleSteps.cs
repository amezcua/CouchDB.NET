using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using CouchDb.ASP.NET;
using System.Web.Security;
using NUnit.Framework;
using System.Configuration.Provider;

namespace CouchDb.ASP.Net.Specs.RoleSpecs
{
    [Binding]
    public class CanNotDeleteAssignedRoleSteps
    {
        string roleName;
        CouchDbMembershipUser user;
        Exception deleteRoleException;

        [Given(@"I have a valid role assigned to a user")]
        public void GivenIHaveAValidRoleAssignedToAUser()
        {
            roleName = "RoleToAssignAndRemoveFromUser";

            Roles.CreateRole(roleName);

            var users = Membership.GetAllUsers();
            var rnd = new Random().Next(users.Count);
            int i = 0;
            foreach (var u in Membership.GetAllUsers())
            {
                i++;
                if (i == rnd)
                {
                    user = u as CouchDbMembershipUser;
                    break;
                }
            }
            Roles.AddUserToRole(user.UserName, roleName);
        }

        [When(@"I call the Delete Role API for that role asking for an exception if the role is in use")]
        public void WhenICallTheDeleteRoleAPIForThatRoleAskingForAnExceptionIfTheRoleIsInUse()
        {
            try
            {
                Roles.DeleteRole(roleName);
            }
            catch (ProviderException ex)
            {
                deleteRoleException = ex;
            }
        }

        [Then(@"a ProviderException is thrown")]
        public void ThenAProviderExceptionIsThrown()
        {
            Assert.IsInstanceOf<ProviderException>(deleteRoleException);

        }


        [Then(@"I can still retrieve the role using the GetAllRoles API")]
        public void ThenICanStillRetrieveTheRoleUsingTheGetAllRolesAPI()
        {
            var roles = new List<string>(Roles.GetAllRoles());
            Assert.IsTrue(roles.Contains(roleName));

            // Cleanup
            Roles.RemoveUserFromRole(user.UserName, roleName);
            Roles.DeleteRole(roleName);
            roles = new List<string>(Roles.GetAllRoles());
            Assert.IsFalse(roles.Contains(roleName));
        }
    }
}
