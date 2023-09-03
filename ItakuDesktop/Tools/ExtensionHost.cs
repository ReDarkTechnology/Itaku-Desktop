using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

using Newtonsoft.Json;
using MoonSharp.Interpreter;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace ItakuDesktop.Tools
{
    public class ExtensionHost : IDisposable
    {
        public bool enabled { get; set; }

        public string id;
        public string code { get; set; }

        public bool runnable { get; private set; }
        public bool containErrors => _logs.Count > 0;
        public string directoryPath { get; private set; }
        public string manifestPath { get; private set; }
        public string scriptPath { get; private set; }

        public ExtensionManifest manifest { get; private set; }

        private List<ExtensionLog> _logs = new List<ExtensionLog>();
        public EventHandler<ExtensionLog> onLog { get; set; }
        private Script _script;

        public ExtensionHost() { }
        public ExtensionHost(string directory)
        {
            directoryPath = directory;
            manifestPath = Path.Combine(directory, "manifest.json");
            manifest = ExtensionManifest.LoadFile(manifestPath);
            if (manifest != null)
            {
                id = manifest.id;
                scriptPath = Path.Combine(directory, manifest.mainScript);
                if (File.Exists(scriptPath))
                {
                    code = File.ReadAllText(scriptPath);
                    RunScript();
                    Console.WriteLine("Script running");
                }
                else
                {
                    AddLog(ExtLogType.Error, this, "ScriptLoadError: Main script isn't set or is not found");
                }
            }
            else
            {
                AddLog(ExtLogType.Error, this, "ManifestError: Manifest file isn't found");
            }
        }

        private void AddLog(ExtensionLog error)
        {
            _logs.Add(error);
            onLog?.Invoke(this, error);
            Console.WriteLine(error.message);
        }

        private void AddLog(ExtLogType type, object owner, string message)
        {
            var error = new ExtensionLog(type, owner, message);
            AddLog(error);
        }

        public void AddExceptionError(Exception e)
        {
            if (e is ScriptRuntimeException scr)
            {
                AddLog(
                    ExtLogType.Error,
                    _script,
                    "RuntimeError: " + scr.DecoratedMessage
                );
            }
            else if (e is InternalErrorException intr)
            {
                AddLog(
                    ExtLogType.Error,
                    _script,
                    "InternalError: " + intr.DecoratedMessage
                );
            }
            else if (e is SyntaxErrorException syn)
            {
                AddLog(
                    ExtLogType.Error,
                    _script,
                    "SyntaxError: " + syn.DecoratedMessage
                );
            }
            else if (e is DynamicExpressionException dyn)
            {
                AddLog(
                    ExtLogType.Error,
                    _script,
                    "DynamicExpressionError: " + dyn.DecoratedMessage
                );
            }
            else
            {
                AddLog(
                    ExtLogType.Error,
                    _script,
                    "UnknownError: " + e.Message + e.StackTrace
                );
            }
        }

        public void ClearLogs()
        {
            _logs.Clear();
        }

        public ExtensionLog[] GetLogs()
        {
            return _logs.ToArray();
        }

        public DynValue CallScript(string function)
        {
            var func = _script.Globals[function];
            if (func != null)
            {
                try
                {
                    Console.WriteLine("Try running " + function);
                    return _script.Call(func);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed " + function);
                    AddExceptionError(e);
                }
            }
            else
            {
                Console.WriteLine("Failed no func " + function);
                AddLog(ExtLogType.Error, _script, $"FunctionCallError: A function called {function} isn't found");
            }
            return null;
        }

        public DynValue CallScriptIgnoreNull(string function)
        {
            var func = _script.Globals[function];
            if (func != null)
            {
                try
                {
                    return _script.Call(func);
                }
                catch (Exception e)
                {
                    AddExceptionError(e);
                }
            }
            return null;
        }

        public DynValue CallScript(string function, params object[] args)
        {
            var func = _script.Globals[function];
            if (func != null)
            {
                try
                {
                    Console.WriteLine("Try running " + function);
                    return _script.Call(func, args);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed " + function);
                    AddExceptionError(e);
                }
            }
            else
            {
                Console.WriteLine("Failed no func " + function);
                AddLog(ExtLogType.Error, _script, $"FunctionCallError: A function called {function} isn't found");
            }
            return null;
        }

        public DynValue CallScriptIgnoreNull(string function, params object[] args)
        {
            var func = _script.Globals[function];
            if (func != null)
            {
                try
                {
                    Console.WriteLine("Running " + function);
                    return _script.Call(func, args);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed " + function);
                    AddExceptionError(e);
                }
            }
            return null;
        }

        public void Reload() => RunScript(); 
        private void RunScript()
        {
            _logs.Clear();
            LuaHost.Initialize();

            _script = new Script();
            _script.Globals["Browser"] = new BrowserMoonsharp(directoryPath);
            _script.Globals["Popup"] = (Func<string, string>)(val => {
                return MessageBox.Show(val, "Itaku: " + manifest.name).ToString("G");
            });
            _script.Globals["Log"] = (Func<string, DynValue>)(val => {
                AddLog(ExtLogType.Info, this, val);
                return DynValue.Nil;
            });
            try
            {
                Console.WriteLine("TryRunning");
                _script.DoString(code);
                runnable = true;
                enabled = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
                AddExceptionError(e);
            }
        }

        public void Dispose()
        {
            _logs.Clear();
            enabled = false;
            runnable = false;
            code = null;
            manifest = null;
            onLog = null;
            _script = null;
            GC.SuppressFinalize(this);
        }
    }

    public class ExtensionManifest
    {
        public string id { get; set; }
        public string name { get; set; }
        public string author { get; set; }

        public int version { get; set; }

        [JsonProperty("manifest_version")]
        public int manifestVersion { get; set; }

        [JsonProperty("main_script")]
        public string mainScript { get; set; }

        public ExtensionManifest() { }
        public static ExtensionManifest LoadFile(string path)
        {
            if(File.Exists(path))
            {
                var json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<ExtensionManifest>(json);
            }
            return null;
        }
    }

    public class ExtensionLog
    {
        public ExtLogType type;
        public object owner;
        public string message;

        public ExtensionLog() { }
        public ExtensionLog(ExtLogType type, object owner, string message)
        {
            this.type = type; this.owner = owner; this.message = message;
        }
    }

    public enum ExtLogType
    {
        Info,
        Warning,
        Error
    }

    public static class LuaHost
    {
        public static Type[] types = {
            typeof(WebView2), typeof(CoreWebView2), typeof(MainForm),
            typeof(ItakuHTTPClient), typeof(ItakuScraper), typeof(ExtensionHost),
            typeof(ExtensionManager), typeof(Exception), typeof(MainForm.SettingsData),
            typeof(BrowserMoonsharp)
        };

        public static bool isInitialized;

        public static void Initialize()
        {
            if (isInitialized) return;

            UserData.RegisterAssembly();
            foreach (var t in types)
                UserData.RegisterType(t);

            isInitialized = true;
        }
    }

    public class BrowserMoonsharp
    {
        public static WebView2 webView2 => MainForm.self.webBrowser;
        public string relative;

        public BrowserMoonsharp() { }
        public BrowserMoonsharp(string relative) { this.relative = relative; }

        public string Url => webView2.Source.AbsoluteUri;
        public string ExecuteScript(string script)
        {
            try
            {
                var task = webView2.ExecuteScriptAsync(script);
                return "Success";
            }
            catch (Exception e)
            {
                return "JSScriptError: " + e.Message + e.StackTrace;
            }
        }

        public string ExecuteFile(string file)
        {
            try
            {
                var filePath = Path.Combine(relative, file);
                if (File.Exists(filePath))
                {
                    var script = File.ReadAllText(filePath);
                    return ExecuteScript(script);
                }
                return "JSScriptError: " + "File not found " + file;
            }
            catch (Exception e)
            {
                return "JSScriptError: " + e.Message + e.StackTrace;
            }
        }
    }
}
