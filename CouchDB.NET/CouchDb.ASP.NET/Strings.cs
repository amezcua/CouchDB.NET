using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace CouchDb.ASP.NET
{
    public class Strings
    {
        public static readonly string UnsupportedPasswordFormat = "Unsupported password format.";
        public static readonly string CanNotUnencodeHashedPassword = "Cannot unencode a hashed password.";
        public static readonly string HashedPasswordsRequireMachineKey = "Hashed passwords are not supported with auto-generated machine keys. Please add the correct machineKey section to your web.config file.";

        public static readonly string CouchDbConfigurationDatabaseNameMissing = "The CouchDB database name configuration field is empty ('" + ConfigurationHelper.CONFIG_COUCHDB_DATABASE_NAME + "'). Please provide a valid database name.";
    }
}
