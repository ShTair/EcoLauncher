using EcoLauncher.Models;

namespace EcoLauncher.Properties
{
    partial class Settings
    {
        public string Password
        {
            get => ProtectedString.Unprotect(EncryptedPassword, "EcoLauncher");
            set => EncryptedPassword = ProtectedString.Protect(value, "EcoLauncher");
        }
    }
}
