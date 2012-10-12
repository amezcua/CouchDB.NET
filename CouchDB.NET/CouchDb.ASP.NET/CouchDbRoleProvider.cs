using System;
using System.Configuration.Provider;
using System.Web.Security;
using System.Collections.Generic;
using System.Linq;

namespace CouchDb.ASP.NET
{
    public class CouchDbRoleProvider : RoleProvider
    {
        private CouchDbRolesRepository repository = null;
        private CouchDbMembershipUserRepository membershipRepository = null;

        private static readonly string DEFAULT_PROVIDER_NAME = "CouchDbRoleProvider";

        private string couchDbServerName;
        private int couchDbServerPort;
        private string couchDbDatabaseName;

        private string providerName = string.Empty;
        private string applicationName = string.Empty;

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null) throw new ArgumentException("config");
            if (String.IsNullOrEmpty(name)) name = DEFAULT_PROVIDER_NAME;

            base.Initialize(name, config);

            applicationName = ConfigurationHelper.GetConfigStringValueOrDefault(config, ConfigurationHelper.CONFIG_APPLICATION_NAME_FIELD, System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);

            couchDbServerName = ConfigurationHelper.RolesCouchDbServerName;
            couchDbServerPort = ConfigurationHelper.RolesCouchDbServerPort;
            couchDbDatabaseName = ConfigurationHelper.RolesCouchDbDatabaseName;

            if (String.IsNullOrEmpty(couchDbDatabaseName))
                throw new ProviderException(Strings.CouchDbConfigurationDatabaseNameMissing);
        }

        public override string ApplicationName
        {
            get
            {
                return applicationName;
            }
            set
            {
                applicationName = value;
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            foreach (var username in usernames)
            {
                var user = Membership.GetUser(username) as CouchDbMembershipUser;
                if (user != null)
                {
                    addUserToRoles(user, roleNames);
                }
            }
        }

        private void addUserToRoles(CouchDbMembershipUser user, string[] roles)
        {
            var currentRoles = new List<string>(user.Roles);
            foreach (var role in roles)
            {
                if (!currentRoles.Contains(role)) currentRoles.Add(role);
            }
            user.Roles = currentRoles.ToArray();
            user.Save();
        }

        public override void CreateRole(string roleName)
        {
            getRepository().AddRole(roleName);
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            if (!getRepository().GetRolesDocument().HasRole(roleName)) return false;

            if(throwOnPopulatedRole)
            {
                var usersWithRole = GetUsersInRole(roleName);
                if (usersWithRole.Length != 0)
                {
                    throw new ProviderException("Can not delete role '" + roleName + "'. " + usersWithRole.Length + " users have it assigned.");
                }
            }

            var deleted = getRepository().GetRolesDocument().DeleteRole(roleName);
            
            return deleted;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            return getRepository().GetRoles();
        }

        public override string[] GetRolesForUser(string username)
        {
            var user = Membership.GetUser(username) as CouchDbMembershipUser;
            if (user == null) throw new ArgumentException("The user: " + username + " does not exist.");
            return user.Roles;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            var users = getMembershipRepository().GetUsersByRole(roleName);

            var db = getMembershipRepository();

            var userNamesList = new List<String>();

            foreach (dynamic mu in db.GetUsersByRole(roleName))
            {
                userNamesList.Add(CouchDbMembershipUser.FromDynamicDbResponse(mu.value).UserName);
            }

            return userNamesList.ToArray();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            if (!getRepository().GetRolesDocument().HasRole(roleName)) throw new ArgumentException("The role does not exist: " + roleName);
            var user = Membership.GetUser(username) as CouchDbMembershipUser;
            if (user == null) throw new ArgumentException("The user does not exist: " + username);

            var roleList = new List<string>(user.Roles);
            return roleList.Contains(roleName);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            foreach (var username in usernames)
            {
                var user = Membership.GetUser(username) as CouchDbMembershipUser;
                if (user == null) continue;

                foreach (var roleName in roleNames)
                {
                    removeUserFromRole(user, roleName);
                }
            }
        }

        private void removeUserFromRole(CouchDbMembershipUser user, string role)
        {
            var roleList = new List<string>(user.Roles);

            roleList.Remove(role);

            user.Roles = roleList.ToArray();
            user.Save();
        }

        public override bool RoleExists(string roleName)
        {
            return getRepository().GetRolesDocument().HasRole(roleName);
        }

        private CouchDbRolesRepository getRepository()
        {
            if (repository == null)
                repository = new CouchDbRolesRepository(couchDbServerName, couchDbServerPort, couchDbDatabaseName);
            return repository;
        }

        private CouchDbMembershipUserRepository getMembershipRepository()
        {
            if (membershipRepository == null)
                membershipRepository = new CouchDbMembershipUserRepository(couchDbServerName, couchDbServerPort, couchDbDatabaseName);
            return membershipRepository;
        }
    }
}
