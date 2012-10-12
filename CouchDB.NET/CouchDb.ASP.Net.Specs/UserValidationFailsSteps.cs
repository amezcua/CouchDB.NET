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
    public class UserValidationFailsSteps
    {
        string userName;
        string password;
        string invalidPassword;

        bool userValidated;

        [Given(@"a user's UserName and Invalid Password")]
        public void GivenAUserSUserNameAndInvalidPassword()
        {
            userName = Guid.NewGuid().ToString();
            password = "password";
            
            Membership.CreateUser(userName, password);
        }

        [When(@"I call the ValidateUser Membership API passing the invalid password")]
        public void WhenICallTheValidateUserMembershipAPIpassingtheinvalidpassword()
        {
            invalidPassword = "badpassword";
            userValidated = Membership.ValidateUser(userName, invalidPassword);
        }

        [Then(@"the response is that the user is invalid")]
        public void ThenTheResponseIsThatTheUserIsInValid()
        {
            Assert.IsFalse(userValidated);
        }
    }
}
