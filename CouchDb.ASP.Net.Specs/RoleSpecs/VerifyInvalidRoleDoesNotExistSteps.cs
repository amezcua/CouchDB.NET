using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using NUnit.Framework;
using System.Web.Security;

namespace CouchDb.ASP.Net.Specs.RoleSpecs
{
    [Binding]
    public class VerifyInvalidRoleDoesNotExistSteps
    {
        string invalidRoleName;
        bool roleExists;

        [Given(@"I have an invalid role name")]
        public void GivenIHaveAnInvalidRoleName()
        {
            invalidRoleName = Guid.NewGuid().ToString();
        }

        [When(@"I call the RoleExists API on that name")]
        public void WhenICallTheRoleExistsAPIOnThatName()
        {
            roleExists = Roles.RoleExists(invalidRoleName);
        }

        [Then(@"the API response is false")]
        public void ThenTheAPIResponseIsFalse()
        {
            Assert.IsFalse(roleExists);
        }

    }
}
