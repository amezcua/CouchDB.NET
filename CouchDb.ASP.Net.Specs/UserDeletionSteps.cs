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
    public class UserDeletionSteps
    {
        string userName;
        string password;
        bool userDeleted;
        MembershipUser membershipUser;

        [Given(@"a valid username")]
        public void GivenAValidUsername()
        {
            userName = Guid.NewGuid().ToString();
            password = "password";

            Membership.CreateUser(userName, password);
        }

        [When(@"I call the DeleteUser method on the MemberchipProvider")]
        public void WhenICallTheDeleteUserMethodOnTheMemberchipProvider()
        {
            userDeleted = Membership.DeleteUser(userName);
        }

        [Then(@"all the user's data is deleted")]
        public void ThenAllTheUserSDataIsDeleted()
        {
            Assert.IsTrue(userDeleted);

            membershipUser = Membership.GetUser(userName);
            Assert.IsNull(membershipUser);
        }

    }
}
