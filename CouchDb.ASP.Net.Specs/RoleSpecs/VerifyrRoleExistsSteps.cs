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
    public class VerifyrRoleExistsSteps
    {
        string roleName;
        bool roleExists;

        [Given(@"I have a valid role name")]
        public void GivenIHaveAValidRoleName()
        {
            roleName = Guid.NewGuid().ToString();
            Roles.CreateRole(roleName);
        }

        [When(@"I call the RoleExists API")]
        public void WhenICallTheRoleExistsAPI()
        {
            roleExists = Roles.RoleExists(roleName);
        }

        [Then(@"the API response is true")]
        public void ThenTheAPIResponseIsTrue()
        {
            Assert.IsTrue(roleExists);
            Roles.DeleteRole(roleName);
            roleExists = Roles.RoleExists(roleName);
            Assert.IsFalse(roleExists);
        }

    }
}
