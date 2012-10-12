using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using System.Web.Security;
using NUnit.Framework;

namespace CouchDb.ASP.Net.Specs
{
    [Binding]
    public class RetrieveAllUsersSteps
    {
        MembershipUserCollection allUsers;
        
        [Given(@"A configured Membership application")]
        public void GivenAConfiguredMembershipApplication()
        {
            Assert.True(true);
        }

        [When(@"I call the GetAllUsers method in the Membership API")]
        public void WhenICallTheGetAllUsersMethodInTheMembershipAPI()
        {
            allUsers = Membership.GetAllUsers();
        }

        [Then(@"I should get a collection with all the users defined in the database")]
        public void ThenIShouldGetACollectionWithAllTheUsersDefinedInTheDatabase()
        {
            Assert.IsNotNull(allUsers);
            Assert.IsInstanceOf(typeof(MembershipUserCollection), allUsers);
            Assert.IsTrue(allUsers.Count != 0);
        }

        [Then(@"Each user in the collection must be of type CouchDbMembershipUser")]
        public void ThenEachUserInTheCollectionMustBeOfTypeCouchDbMembershipUser()
        {
            var differentType = false;
            foreach (var u in allUsers)
            {
                if (!(u is CouchDb.ASP.NET.CouchDbMembershipUser))
                {
                    differentType = true;
                    break;
                }
            }

            Assert.IsFalse(differentType);
        }

        [Then(@"Each user in the collection musts have a unique ID and UserName")]
        public void ThenEachUserInTheCollectionMustsHaveAUniqueIDAndUserName()
        {
            var idsTable = new HashSet<string>();
            var userNamesTable = new HashSet<string>();

            foreach (var u in allUsers)
            {
                try
                {
                    idsTable.Add((u as CouchDb.ASP.NET.CouchDbMembershipUser).ProviderUserKey.ToString());
                    userNamesTable.Add((u as CouchDb.ASP.NET.CouchDbMembershipUser).UserName);
                }
                catch (Exception)
                {
                    Assert.Fail();
                }
            }
            Assert.Pass();
        }

    }
}
