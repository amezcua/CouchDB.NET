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
    public class RetreiveKnownUserNameByEmailSteps
    {
        string knownEmailAddress;
        string retrievedUserName;

        [Given(@"a known email address")]
        public void GivenAKnownEmailAddress()
        {
            knownEmailAddress = "alejandro_mezcua@hotmail.com";
            try
            {
                Membership.CreateUser("alejandro_mezcua@hotmail.com", "password", knownEmailAddress);
            }
            catch (Exception) { }
        }

        [When(@"I call the GetUser Membership API passing the email")]
        public void WhenICallTheGetUserMembershipAPIPassingTheEmail()
        {
            retrievedUserName = Membership.GetUserNameByEmail(knownEmailAddress);
        }

        [Then(@"the user's UserName is returned")]
        public void ThenTheUserSUserNameIsReturned()
        {
            Assert.AreEqual("alejandro_mezcua@hotmail.com", retrievedUserName);
        }
    }
}
