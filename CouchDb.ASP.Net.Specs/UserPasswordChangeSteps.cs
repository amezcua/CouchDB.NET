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
    public class UserPasswordChangeSteps
    {
        string userName;
        string password;
        string newPassword;
        bool changePasswordResult;

        [Given(@"a valid username and password")]
        public void GivenAValidUsernameAndPassword()
        {
            userName = Guid.NewGuid().ToString();
            password = "password";
            Membership.CreateUser(userName, password);
        }

        [When(@"the user requests to change his password")]
        public void WhenTheUserRequestsToChangeHisPassword()
        {
            var user = Membership.GetUser(userName);
            newPassword = "newPassword";
            changePasswordResult = user.ChangePassword(password, newPassword);
        }


        [Then(@"the password change operation succeds")]
        public void ThenThePasswordChangeOperationSucceds()
        {
            Assert.IsTrue(changePasswordResult);
            Assert.IsTrue(Membership.ValidateUser(userName, newPassword));
        }

    }
}
