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
    public class UserCreationFailsForDuplicateEmailSteps
    {
        string knownUserName;
        string password;
        string email;
        MembershipUser membershipUser;
        MembershipCreateUserException ex;

        [Given(@"a known user's email address")]
        public void GivenAKnownUserSEmailAddress()
        {
            knownUserName = Guid.NewGuid().ToString();
            password = "password";
            email = Guid.NewGuid().ToString();
            Membership.CreateUser(knownUserName, password, email);
        }

        [When(@"I try to create a new user with the same email")]
        public void WhenITryToCreateANewUserWithTheSameEmail()
        {
            var newUserName = Guid.NewGuid().ToString();
            try
            {
                membershipUser = Membership.CreateUser(newUserName, password, email);
            }
            catch (MembershipCreateUserException e)
            {
                ex = e;
            }
        }

        [Then(@"the user creation operation fails for that email")]
        public void ThenTheUserCreationOperationFailsForThatEmail()
        {
            Assert.IsNull(membershipUser);
            Assert.IsNotNull(ex);
        }

    }
}
