using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItakuDesktop.Tools
{
    public static class ExtensionManager
    {
        public static string extensionFolder;
        public static List<ExtensionHost> hosts = new List<ExtensionHost>();
        public static ExtensionHost LoadExtension(string folderPath)
        {
            var host = new ExtensionHost(folderPath);
            hosts.Add(host);
            return host;
        }

        public static void Refresh()
        {
            foreach(var host in hosts)
                host.Dispose();
            hosts.Clear();
            LoadAllInFolder(extensionFolder);
        }

        public static void LoadAllInFolder(string extensionsFolder)
        {
            extensionFolder = extensionsFolder;
            var directories = Directory.GetDirectories(extensionsFolder);
            foreach(var directory in directories)
            {
                string manifest = Path.Combine(directory, "manifest.json");
                if (File.Exists(manifest))
                    LoadExtension(directory);
            }
        }

        public static void CallAllScript(string function)
        {
            foreach(var host in hosts)
            {
                if(host.runnable && host.enabled)
                    host.CallScript(function);
            }
        }

        public static void CallAllScript(string function, params object[] args)
        {
            foreach (var host in hosts)
            {
                if (host.runnable && host.enabled)
                    host.CallScript(function, args);
            }
        }
    }
}
