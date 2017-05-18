using System;
using System.Security.Cryptography;
using System.Text;

namespace EcoLauncher.Models
{
    class ProtectedString
    {
        private static UTF8Encoding _utf8 = new UTF8Encoding(false);

        public static string Protect(string userData, string optionalEntropy)
        {
            try
            {
                var data = ProtectedData.Protect(_utf8.GetBytes(userData), _utf8.GetBytes(optionalEntropy), DataProtectionScope.CurrentUser);
                return Convert.ToBase64String(data);
            }
            catch { }
            return null;
        }

        public static string Unprotect(string encryptedData, string optionalEntropy)
        {
            try
            {
                var data = ProtectedData.Unprotect(Convert.FromBase64String(encryptedData), _utf8.GetBytes(optionalEntropy), DataProtectionScope.CurrentUser);
                return _utf8.GetString(data);
            }
            catch { }
            return null;
        }
    }
}
