using System;
using System.Web.Security;
using NUnit.Framework;
using TechTalk.SpecFlow;
using CouchDb.ASP.NET;

namespace CouchDb.ASP.Net.Specs
{
    [Binding]
    public class UserCreationSteps
    {
        MembershipUser newUser;
        string userName;
        string password;

        [Given(@"I have the data available to define a new application user, the userName and Password")]
        public void GivenIHaveTheDataAvailableToDefineANewApplicationUserTheUserNameAndPassword()
        {
            userName = Guid.NewGuid().ToString();
            password = "password";
        }


        [When(@"I call the Membership API to create the user")]
        public void WhenICallTheMembershipAPIToCreateTheUser()
        {
            newUser = Membership.CreateUser(Guid.NewGuid().ToString(), password);
        }

        [Then(@"the Membership API must respond with a valid user with a new generated ProviderUserKey")]
        public void ThenTheMembershipAPIMustRespondWithASuccessValue()
        {
            Assert.IsNotNullOrEmpty((string)newUser.ProviderUserKey);
        }

        [Then(@"the Membership API should create the user with a hashed password")]
        public void ThenTheMembershipAPIShouldCreateTheUserWithAHashedPassword()
        {
            Assert.IsNotNull((newUser as CouchDbMembershipUser).PasswordHash);
        }
    }
}
