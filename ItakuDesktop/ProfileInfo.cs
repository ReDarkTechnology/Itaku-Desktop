using System.IO;
using Newtonsoft.Json;

namespace ItakuDesktop
{
    public class ProfileInfo
    {
        public string name;
        public string path;

        public static ProfileInfo Read(string path) => JsonConvert.DeserializeObject<ProfileInfo>(File.ReadAllText(path));
        public void Save(string path) => File.WriteAllText(path, JsonConvert.SerializeObject(this));
    }
}