using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Dynamic;

namespace CouchDb.ASP.NET
{
    //[DataContract]
    public class CouchDbMembershipUser : MembershipUser
    {
        private CouchDbMembershipUserRepository repository = null;
        private string couchDbId = "";
        private string couchDbRev = "";
        private string couchDbDocumentType = "CouchDbMembershipUser";
        private string passwordHash = "";
        private string[] roles = { };

        private string userName;

        public CouchDbMembershipUser(string providerName, string name, string providerUserKey, 
                            string email, string passwordQuestion, string comment, 
                            bool isApproved, bool isLockedOut, DateTime creationDate, DateTime lastLoginDate, 
                        DateTime lastActivityDate, DateTime lastPasswordChangedDate, DateTime lastLockoutDate) 
            : base(providerName, name, providerUserKey, 
                    email, passwordQuestion, comment, 
                    isApproved, isLockedOut, creationDate, 
                    lastLoginDate, lastActivityDate, 
                    lastPasswordChangedDate, lastLockoutDate)
        {
            
        }

        public new string UserName
        {
            get
            {
                return base.UserName;
            }
            set
            {
                userName = value;
            }
        }

        public string _id {
            get
            {
                return couchDbId;
            }
            set
            {
                couchDbId = value;
            }
        }

        [DefaultValue("")]
        public string _rev
        {
            get
            {
                return couchDbRev;
            }
            set
            {
                couchDbRev = value;
            }
        }

        public string type { 
            get { return couchDbDocumentType; }
            set { couchDbDocumentType = value; }
        }

        public string PasswordHash {
            get { return passwordHash; }
            set { passwordHash = value; }
        }

        public string[] Roles
        {
            get { return roles; }
            set { roles = value; }
        }

        internal static CouchDbMembershipUser FromDynamicDbResponse(dynamic dbUser)
        {
            var user = new CouchDbMembershipUser(dbUser.ProviderName, 
                dbUser.UserName, dbUser.ProviderUserKey, dbUser.Email, 
                dbUser.PasswordQuestion, dbUser.Comment, dbUser.IsApproved, 
                dbUser.IsLockedOut, 
                DateTime.Parse(dbUser.CreationDate), 
                DateTime.Parse(dbUser.LastLoginDate), 
                DateTime.Parse(dbUser.LastActivityDate),
                DateTime.Parse(dbUser.LastPasswordChangedDate),
                DateTime.Parse(dbUser.LastLockoutDate));

            user._id = dbUser._id;
            user._rev = dbUser._rev;
            user.type = dbUser.type;
            user.PasswordHash = dbUser.PasswordHash;
            user.Roles = Array.ConvertAll((object[])dbUser.Roles, (item => (string)item)); ;
            
            return user;
        }

        internal void Save()
        {
            if (String.IsNullOrEmpty(_id)) _id = Guid.NewGuid().ToString();
            getRepository().SaveWithId(_id, this);
        }

        internal CouchDbMembershipUserRepository getRepository()
        {
            if (repository == null) repository = 
                new CouchDbMembershipUserRepository(
                    ConfigurationHelper.MembershipCouchDbServerName, 
                    ConfigurationHelper.MembershipCouchDbServerPort, 
                    ConfigurationHelper.MembershipCouchDbDatabaseName);
            return repository;
        }
    }
}
