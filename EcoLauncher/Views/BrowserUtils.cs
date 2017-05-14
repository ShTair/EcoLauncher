using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

namespace EcoLauncher.Views
{
    static class BrowserUtils
    {
        public static void SetFeatureBrowserEmulation(int value)
        {
            var fileName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
            SetRegistryValue("FEATURE_BROWSER_EMULATION", fileName, value);
        }

        public static void SetFeatureGpuRendering(bool value)
        {
            var fileName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
            SetRegistryValue("FEATURE_GPU_RENDERING", fileName, value ? 1 : 0);
        }

        private static void SetRegistryValue(string key, string name, object value)
        {
            var root = @"Software\Microsoft\Internet Explorer\Main\FeatureControl";
            var regkey = Registry.CurrentUser.CreateSubKey(Path.Combine(root, key));
            regkey.SetValue(name, value);
        }
    }
}
