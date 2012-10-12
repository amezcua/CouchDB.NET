using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using System.Web.Security;
using NUnit.Framework;
using CouchDb.ASP.NET;


namespace CouchDb.ASP.Net.Specs
{
    [Binding]
    public class RetrieveKnownUserByUserNameSteps
    {
        string knownUserName;
        MembershipUser retrievedUser;

        [Given(@"a known user name")]
        public void GivenAKnownUserName()
        {
            knownUserName = Guid.NewGuid().ToString();
            var newUser = Membership.CreateUser(knownUserName, "test");
            
        }

        [When(@"I call the GetUser Membership API passing the user name")]
        public void WhenICallTheGetUserMembershipAPI()
        {
            retrievedUser = Membership.GetUser(knownUserName);
        }

        [Then(@"the user's data is returned as a CouchDBMembershipUser object")]
        public void ThenTheUserSDataIsReturnedAsACouchDBMembershipUserObject()
        {
            Assert.IsNotNull(retrievedUser);
            Assert.IsInstanceOf<CouchDbMembershipUser>(retrievedUser);
        }
    }
}
