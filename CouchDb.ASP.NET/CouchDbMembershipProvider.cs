using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Collections.Specialized;
using System.Web.Configuration;
using System.Configuration.Provider;

namespace CouchDb.ASP.NET
{
    public class CouchDbMembershipProvider : MembershipProvider
    {
        private static readonly string DEFAULT_PROVIDER_NAME = "CouchDbMembershipProvider";

        private CouchDbMembershipUserRepository repository = null;

        private string providerName = string.Empty;
        private string applicationName = string.Empty;
        private string description = string.Empty;
        private bool enablePasswordReset;
        private bool enablePasswordRetrieval = false;
        private int maxInvalidPasswordAttempts;
        private int minRequiredNonAlphanumericCharacters;
        private int minRequiredPasswordLength;
        private int passwordAttemptWindow;
        private MembershipPasswordFormat passwordFormat;
        private string passwordStrengthRegularExpression = string.Empty;
        private bool requiresQuestionAndAnswer;
        private bool requiresUniqueEmail;

        private MachineKeySection machineKeySection;

        private string couchDbServerName;
        private int couchDbServerPort;
        private string couchDbDatabaseName;

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null) throw new ArgumentException("config");
            if (String.IsNullOrEmpty(name)) name = DEFAULT_PROVIDER_NAME;
            
            base.Initialize(name, config);

            applicationName = ConfigurationHelper.GetConfigStringValueOrDefault(config, ConfigurationHelper.CONFIG_APPLICATION_NAME_FIELD, System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            description = ConfigurationHelper.GetConfigStringValueOrDefault(config, ConfigurationHelper.CONFIG_DESCRIPTION_FIELD, "Couch DB Membership Provider");
            enablePasswordReset = ConfigurationHelper.GetConfigBoolValueOrDefault(config, ConfigurationHelper.CONFIG_ENABLE_PASSWORD_RESET, false);
            enablePasswordRetrieval = false;
            maxInvalidPasswordAttempts = ConfigurationHelper.GetConfigIntValueOrDefault(config, ConfigurationHelper.CONFIG_MAX_INVALID_PASSWORD_ATTEMPTS, 5);
            minRequiredNonAlphanumericCharacters = ConfigurationHelper.GetConfigIntValueOrDefault(config, ConfigurationHelper.CONFIG_MIN_REQUIRED_NON_ALPHANUMERIC_CHARACTERS, 0);
            minRequiredPasswordLength = ConfigurationHelper.GetConfigIntValueOrDefault(config, ConfigurationHelper.CONFIG_MIN_REQUIRED_PASSWORD_LENGTH, 8);
            passwordAttemptWindow = ConfigurationHelper.GetConfigIntValueOrDefault(config, ConfigurationHelper.CONFIG_PASSWORD_ATTEMPT_WINDOW, 10);
            passwordFormat = MembershipPasswordFormat.Hashed;
            passwordStrengthRegularExpression = ConfigurationHelper.GetConfigStringValueOrDefault(config, ConfigurationHelper.CONFIG_PASSWORD_STRENGTH_REGULAR_EXPRESSION, String.Empty);
            requiresQuestionAndAnswer = ConfigurationHelper.GetConfigBoolValueOrDefault(config, ConfigurationHelper.CONFIG_REQUIRES_QUESTION_AND_ANSWER, false);
            requiresUniqueEmail = true;
            providerName = name;

            couchDbServerName = ConfigurationHelper.MembershipCouchDbServerName;
            couchDbServerPort = ConfigurationHelper.MembershipCouchDbServerPort;
            couchDbDatabaseName = ConfigurationHelper.MembershipCouchDbDatabaseName;

            if (String.IsNullOrEmpty(couchDbDatabaseName))
                throw new ProviderException(Strings.CouchDbConfigurationDatabaseNameMissing);

            machineKeySection = (MachineKeySection)WebConfigurationManager.GetWebApplicationSection("system.web/machineKey");
            if(machineKeySection == null)
                throw new ProviderException(Strings.HashedPasswordsRequireMachineKey);

            if (machineKeySection.ValidationKey.ToLower().Contains("Autogenerate".ToLower()))
                throw new ProviderException(Strings.HashedPasswordsRequireMachineKey);
        }

        public override string ApplicationName { get { return applicationName; } set { applicationName = value; } }
        public override string Description { get { return description; } }
        public override bool EnablePasswordReset { get { return enablePasswordReset; } }
        public override bool EnablePasswordRetrieval { get { return enablePasswordRetrieval; } }
        public override int MaxInvalidPasswordAttempts { get { return maxInvalidPasswordAttempts; } }
        public override int MinRequiredNonAlphanumericCharacters { get { return minRequiredNonAlphanumericCharacters; } }
        public override int MinRequiredPasswordLength { get { return minRequiredPasswordLength; } }
        public override string Name { get { return providerName; } }
        public override int PasswordAttemptWindow { get { return passwordAttemptWindow; } }
        public override MembershipPasswordFormat PasswordFormat { get { return passwordFormat; } }
        public override string PasswordStrengthRegularExpression { get { return passwordStrengthRegularExpression; } }
        public override bool RequiresQuestionAndAnswer { get { return requiresQuestionAndAnswer; } }
        public override bool RequiresUniqueEmail { get { return requiresUniqueEmail; } }
        
        public override MembershipUser CreateUser(string userName, 
            string password, string email, string passwordQuestion, string passwordAnswer, 
            bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            var args = new ValidatePasswordEventArgs(userName, password, true);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (requiresUniqueEmail && GetUserNameByEmail(email) != "")
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            var user = GetUser(userName, false);
            if (user != null)
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }

            var membershipUser = new CouchDbMembershipUser(providerName, userName, Guid.NewGuid().ToString(), email, 
                passwordQuestion, String.Empty, isApproved, false, DateTime.Now, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);

            membershipUser.PasswordHash = new PasswordManager().EncodePassword(password, MembershipPasswordFormat.Hashed, machineKeySection.ValidationKey);

            SaveNewUser(membershipUser);

            status = MembershipCreateStatus.Success;
            return membershipUser;
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if(!ValidateUser(username, oldPassword)) return false;

            var newPasswordHash = new PasswordManager().EncodePassword(newPassword, MembershipPasswordFormat.Hashed, machineKeySection.ValidationKey);

            var user = GetUser(username, false) as CouchDbMembershipUser;
            
            user.PasswordHash = newPasswordHash;

            SaveUser(user);

            return true;
            
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            var user = getRepository().GetUserByUserName(username);

            if (user == null) return false;

            this.DeleteUser(user);
            return true;
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            var db = new CouchDbMembershipUserRepository(couchDbServerName, couchDbServerPort, couchDbDatabaseName);

            totalRecords = 0;

            var returnCollection = new MembershipUserCollection();

            foreach (dynamic mu in db.GetAll())
            {
                returnCollection.Add(CouchDbMembershipUser.FromDynamicDbResponse(mu.value));
            }
            
            return returnCollection;
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            return getRepository().GetUserByUserName(username);
        }

        public override string GetUserNameByEmail(string email)
        {
            var user = getRepository().GetUserByEmail(email);

            return user == null ? string.Empty : user.UserName;
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            var passwordHash = new PasswordManager().EncodePassword(password, passwordFormat, machineKeySection.ValidationKey);

            var user = GetUser(username, false) as CouchDbMembershipUser;

            if (user == null) return false;

            return passwordHash == user.PasswordHash;
        }

        protected override byte[] DecryptPassword(byte[] encodedPassword)
        {
            return base.DecryptPassword(encodedPassword);
        }

        protected override byte[] EncryptPassword(byte[] password)
        {
            return base.EncryptPassword(password);
        }

        protected override byte[] EncryptPassword(byte[] password, MembershipPasswordCompatibilityMode legacyPasswordCompatibilityMode)
        {
            return base.EncryptPassword(password, legacyPasswordCompatibilityMode);
        }

        protected override void OnValidatingPassword(ValidatePasswordEventArgs e)
        {
            base.OnValidatingPassword(e);
        }

        private void SaveNewUser(CouchDbMembershipUser user)
        {
            //getRepository().SaveNewWithId(user.ProviderUserKey.ToString(), user);
            getRepository().SaveWithId(user.ProviderUserKey.ToString(), user);
        }

        private void SaveUser(CouchDbMembershipUser user)
        {
            getRepository().SaveWithId(user.ProviderUserKey.ToString(), user);
        }

        private void DeleteUser(CouchDbMembershipUser user)
        {
            getRepository().Delete(user.ProviderUserKey.ToString());
        }

        private CouchDbMembershipUserRepository getRepository()
        {
            if(repository == null)
                repository = new CouchDbMembershipUserRepository(couchDbServerName, couchDbServerPort, couchDbDatabaseName);
            return repository;
        }
    }
}
