using System;
using System.Configuration.Provider;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace CouchDb.ASP.NET
{
    internal class PasswordManager
    {
        internal string EncodePassword(string password, MembershipPasswordFormat passwordFormat, string key)
        {
            string encodedPassword = password;

            switch (passwordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    throw new ProviderException(Strings.UnsupportedPasswordFormat);
                case MembershipPasswordFormat.Encrypted:
                    throw new ProviderException(Strings.UnsupportedPasswordFormat);
                case MembershipPasswordFormat.Hashed:
                    var hash = new HMACSHA1();
                    hash.Key = HexToByte(key);
                    encodedPassword =
                      Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
                    break;
                default:
                    throw new ProviderException(Strings.UnsupportedPasswordFormat);
            }

            return encodedPassword;
        }

        internal string UnEncodePassword(string encodedPassword, MembershipPasswordFormat passwordFormat)
        {
            throw new ProviderException(Strings.CanNotUnencodeHashedPassword);
            //string password = encodedPassword;

            //switch (passwordFormat)
            //{
            //    case MembershipPasswordFormat.Clear:
            //        throw new ProviderException(Strings.UnsupportedPasswordFormat);
            //    case MembershipPasswordFormat.Encrypted:
            //        throw new ProviderException(Strings.UnsupportedPasswordFormat);
            //    case MembershipPasswordFormat.Hashed:
            //        throw new ProviderException(Strings.CanNotUnencodeHashedPassword);
            //    default:
            //        throw new ProviderException(Strings.UnsupportedPasswordFormat);
            //}

            //return password;
        }

        private byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
    }
}
