using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using System.Web.Security;
using NUnit.Framework;

namespace CouchDb.ASP.Net.Specs.RoleSpecs
{
    [Binding]
    public class DeleteRolesFromDatabaseSteps
    {
        string roleToDelete;
        bool deleted;

        [Given(@"I have a valid role")]
        public void GivenIHaveAValidRole()
        {
            roleToDelete = "roletodelete";
            Roles.CreateRole(roleToDelete);
            Assert.IsTrue(Roles.RoleExists(roleToDelete));
        }

        [When(@"I call the DeleteRole API")]
        public void WhenICallTheDeleteRoleAPI()
        {
            deleted = Roles.DeleteRole(roleToDelete);
        }

        [Then(@"the role is deleted")]
        public void ThenTheRoleIsDeleted()
        {
            Assert.IsTrue(deleted);
        }

        [Then(@"I can not retrieve it using the GetAllRoles API")]
        public void ThenICanNotRetrieveItUsingTheGetAllRolesAPI()
        {
            var roles = Roles.GetAllRoles();
            bool exists = false;

            foreach (var role in roles)
            {
                if (role == roleToDelete)
                {
                    exists = true;
                    break;
                }
            }

            Assert.IsFalse(exists);
        }
    }
}
