using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using System.Web.Security;
using NUnit.Framework;

namespace CouchDb.ASP.Net.Specs.RoleSpecs
{
    [Binding]
    public class RetrieveAllUsersInRole
    {
        string testRoleName;
        List<string> randomUsers;
        string[] usersInRole;

        [Given(@"I have valid users with roles assigned")]
        public void GivenIHaveValidUsersWithRolesAssigned()
        {
            testRoleName = "TestRole";
            var userCollection = Membership.GetAllUsers();

            randomUsers = new List<string>(5);
            for (int i = 0; i < 5 - 1; i++)
            {
                var rnd = new Random();
                int randomIndex = rnd.Next(userCollection.Count - 1);

                int j = 0;
                foreach (MembershipUser user in userCollection)
                {
                    j++;
                    if (j == randomIndex)
                    {
                        if (!randomUsers.Contains(user.UserName))
                        {
                            randomUsers.Add(user.UserName);
                            break;
                        }
                    }
                }
            }

            Roles.AddUsersToRole(randomUsers.ToArray(), testRoleName);
        }

        [When(@"I call the GetUsersInRole API")]
        public void WhenICallTheGetUsersInRoleAPI()
        {
             usersInRole = Roles.GetUsersInRole(testRoleName);
        }

        [Then(@"I get a list of all the users in that role")]
        public void ThenIGetAListOfAllTheUsersInThatRole()
        {
            foreach (string userName in usersInRole)
            {
                Roles.RemoveUserFromRole(userName, testRoleName);
            }
            Assert.IsTrue(randomUsers.Count == usersInRole.Length);
        }

    }
}
