using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NimUtility
{
    public class NativeDependentInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }
    }

    public class ConfigReader
    {
        const string SettingFile = "config.json";
        private static JObject GetConfigRoot()
        {
            if (!File.Exists(SettingFile))
                return null;
            using (var stream = File.OpenRead(SettingFile))
            {
                StreamReader reader = new StreamReader(stream);
                var content = reader.ReadToEnd();
                reader.Close();
                return JObject.Parse(content);
            }
        }

        private static JToken GetConfigToken()
        {
            var root = GetConfigRoot();
            if (root == null)
                return null;
            var indexToken = root.SelectToken("configs.index");
            int index = indexToken == null ? 0 : indexToken.ToObject<int>();
            var configsToken = root.SelectToken("configs.list");
            if (configsToken == null)
                return null;
            var configList = configsToken.ToArray();
            if (index >= configList.Count())
                return null;
            return configList[index];
        }

        public static string GetSdkVersion()
        {
            var root = GetConfigRoot();
            if (root == null)
                return null;
            var token = root.SelectToken("sdk.version");
            return token.ToObject<string>();
        }

        private static T GetConfigItem<T>(string jsonPath)
        {
            var configToken = GetConfigToken();
            if (configToken == null)
                return default(T);
            var token = configToken.SelectToken(jsonPath);
            return token == null ? default(T) : token.ToObject<T>();
        }

        public static string GetAppKey()
        {
            return GetConfigItem<string>("app_key");
        }

        public static string GetChatRommListServerUrl()
        {
            return GetConfigItem<string>("roomlistserver");
        }

        public static NimConfig GetSdkConfig()
        {
            var configToken = GetConfigToken();
            var cfg = configToken.ToObject<NimConfig>();
            return cfg;
        }
    }
}
