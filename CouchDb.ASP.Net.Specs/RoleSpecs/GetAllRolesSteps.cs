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
    public class GetAllRolesSteps
    {
        string[] roles;

        [Given(@"there are roles defined in the database")]
        public void GivenThereAreRolesDefinedInTheDatabase()
        {
            
        }

        [When(@"I call the GetAllRoles API")]
        public void WhenICallTheGetAllRolesAPI()
        {
            roles = Roles.GetAllRoles();
        }

        [Then(@"I receive all the roles defined")]
        public void ThenIReceiveAllTheRolesDefined()
        {
            Assert.IsNotNull(roles);
            Assert.IsTrue(roles.Count() > 0);
        }
    }
}
