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
    public class UserValidationSuccedsSteps
    {
        string userName;
        string validPassword;

        bool userValidated;

        [Given(@"a user's UserName and Password")]
        public void GivenAUserSUserNameAndPassword()
        {
            userName = Guid.NewGuid().ToString();
            validPassword = "password";
            Membership.CreateUser(userName, validPassword);
        }

        [When(@"I call the ValidateUser Membership API")]
        public void WhenICallTheValidateUserMembershipAPI()
        {
            userValidated = Membership.ValidateUser(userName, validPassword);
        }

        [Then(@"the response is that the user is valid")]
        public void ThenTheResponseIsThatTheUserIsValid()
        {
            Assert.IsTrue(userValidated);
        }
    }
}
