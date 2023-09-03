using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ItakuDesktop
{
    public static class WindowsUtil
    {
        public static bool IsApplicationOnStartup()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            bool r = rk.GetValue(Application.ProductName, null) != null;
            rk.Close();
            return r;
        }

        public static void SetApplicationStartup(bool to)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
             ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (to)
                rk.SetValue(Application.ProductName, Application.ExecutablePath);
            else
                rk.DeleteValue(Application.ProductName, false);
            rk.Close();
        }

        public static string FixPath(this string path)
        {
            // To get the location the assembly normally resides on disk or the install directory
            string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;

            // Once you have the path you get the directory with:
            var directory = Path.GetDirectoryName(codeBase);

            // And then combine it and also remove the file:\\
            return Path.Combine(directory, path).Replace("file:\\", string.Empty);
        }
    }
}
