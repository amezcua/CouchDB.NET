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
    public class UserCreationFailsForDuplicateUserNameSteps
    {
        string knownUserName;
        string password;
        MembershipUser membershipUser;
        MembershipCreateUserException ex;

        [Given(@"a known user")]
        public void GivenAKnownUser()
        {
            knownUserName = Guid.NewGuid().ToString();
            password = "password";
            Membership.CreateUser(knownUserName, password);
        }

        [When(@"I try to create a new user with the same UserName")]
        public void WhenITryToCreateANewUserWithTheSameUserName()
        {
            try
            {
                membershipUser = Membership.CreateUser(knownUserName, "password");
            }
            catch (MembershipCreateUserException e)
            {
                ex = e;
            }
        }

        [Then(@"the user creation operation fails")]
        public void ThenTheUserCreationOperationFails()
        {
            Assert.IsNull(membershipUser);
            Assert.IsNotNull(ex);
        }
    }
}
