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
    public class AddRolesToTheDatabaseSteps
    {
        string newRoleName;

        [Given(@"I have defined a new role")]
        public void GivenIHaveDefinedANewRole()
        {
            newRoleName = "admin" + Guid.NewGuid().ToString();
        }

        [When(@"I call the CreateRole API")]
        public void WhenICallTheCreateRoleAPI()
        {
            Roles.CreateRole(newRoleName);
        }

        [Then(@"the role is added to the database")]
        public void ThenTheRoleIsAddedToTheDatabase()
        {
            
        }

        [Then(@"I can retrieve it using the GetAllRoles API")]
        public void ThenICanRetrieveItUsingTheGetAllRolesAPI()
        {
            string[] roles = Roles.GetAllRoles();
            Assert.IsTrue(roles.Contains(newRoleName));
        }

    }
}
