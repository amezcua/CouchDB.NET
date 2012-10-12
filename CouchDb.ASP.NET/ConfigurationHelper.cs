using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Web.Configuration;

namespace CouchDb.ASP.NET
{
    public class ConfigurationHelper
    {
        internal static readonly string CONFIG_APPLICATION_NAME_FIELD = "applicationName";
        internal static readonly string CONFIG_DESCRIPTION_FIELD = "description";
        internal static readonly string CONFIG_ENABLE_PASSWORD_RESET = "enablePasswordReset";
        internal static readonly string CONFIG_ENABLE_PASSWORD_RETRIEVAL = "enablePasswordRetrieval";
        internal static readonly string CONFIG_MAX_INVALID_PASSWORD_ATTEMPTS = "maxInvalidPasswordAttempts";
        internal static readonly string CONFIG_MIN_REQUIRED_NON_ALPHANUMERIC_CHARACTERS = "minRequiredNonAlphanumericCharacters";
        internal static readonly string CONFIG_MIN_REQUIRED_PASSWORD_LENGTH = "minRequiredPasswordLength";
        internal static readonly string CONFIG_PASSWORD_ATTEMPT_WINDOW = "passwordAttemptWindow";
        internal static readonly string CONFIG_PASSWORD_FORMAT = "passwordFormat";
        internal static readonly string CONFIG_PASSWORD_STRENGTH_REGULAR_EXPRESSION = "passwordStrengthRegularExpression";
        internal static readonly string CONFIG_REQUIRES_QUESTION_AND_ANSWER = "requiresQuestionAndAnswer";
        internal static readonly string CONFIG_REQUIRES_UNIQUE_EMAIL = "requiresUniqueEmail";
        internal static readonly string CONFIG_WRITE_EXCEPTIONS_TO_EVENT_LOG = "writeExceptionsToEventLog";

        internal static readonly string LOCALHOST = "localhost";
        internal static readonly string CONFIG_COUCHDB_SERVER_NAME = "couchDbServerName";
        internal static readonly string CONFIG_COUCHDB_SERVER_PORT = "couchDbServerPort";
        internal static readonly string CONFIG_COUCHDB_DATABASE_NAME = "couchDbDatabaseName";

        public static string MembershipCouchDbServerName
        {
            get
            {
                var membershipSection = (MembershipSection)WebConfigurationManager.GetWebApplicationSection("system.web/membership");
                return membershipSection.Providers[membershipSection.DefaultProvider].Parameters["couchDbServerName"];
            }
        }

        public static int MembershipCouchDbServerPort
        {
            get
            {
                var membershipSection = (MembershipSection)WebConfigurationManager.GetWebApplicationSection("system.web/membership");
                return Int32.Parse(membershipSection.Providers[membershipSection.DefaultProvider].Parameters["couchDbServerPort"]);
            }
        }

        public static string MembershipCouchDbDatabaseName
        {
            get
            {
                var membershipSection = (MembershipSection)WebConfigurationManager.GetWebApplicationSection("system.web/membership");
                return membershipSection.Providers[membershipSection.DefaultProvider].Parameters["couchDbDatabaseName"];
            }
        }

        public static string RolesCouchDbServerName
        {
            get
            {
                var roleManagerSection = (RoleManagerSection)WebConfigurationManager.GetWebApplicationSection("system.web/roleManager");
                return roleManagerSection.Providers[roleManagerSection.DefaultProvider].Parameters["couchDbServerName"];
            }
        }

        public static int RolesCouchDbServerPort
        {
            get
            {
                var roleManagerSection = (RoleManagerSection)WebConfigurationManager.GetWebApplicationSection("system.web/roleManager");
                return Int32.Parse(roleManagerSection.Providers[roleManagerSection.DefaultProvider].Parameters["couchDbServerPort"]);
            }
        }

        public static string RolesCouchDbDatabaseName
        {
            get
            {
                var roleManagerSection = (RoleManagerSection)WebConfigurationManager.GetWebApplicationSection("system.web/roleManager");
                return roleManagerSection.Providers[roleManagerSection.DefaultProvider].Parameters["couchDbDatabaseName"];
            }
        }
        

        internal static string GetConfigStringValueOrDefault(NameValueCollection config, string key, string defaultValue)
        {
            var configValue = config.Get(key);
            return String.IsNullOrEmpty(configValue) ? defaultValue : configValue;
        }

        internal static int GetConfigIntValueOrDefault(NameValueCollection config, string key, int defaultValue)
        {
            var configValue = config.Get(key);
            if (String.IsNullOrEmpty(configValue)) return defaultValue;

            int typedValue;
            return Int32.TryParse(configValue, out typedValue) ? typedValue : defaultValue;
        }

        internal static bool GetConfigBoolValueOrDefault(NameValueCollection config, string key, bool defaultValue)
        {
            var configValue = config.Get(key);
            if (String.IsNullOrEmpty(configValue)) return defaultValue;

            bool typedValue;
            return Boolean.TryParse(configValue, out typedValue) ? typedValue : defaultValue;
        }
    }
}
