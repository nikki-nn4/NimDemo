using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace NimUtility
{
    public interface IJsonObject
    {
        string Serialize();
    }

    public class NimJsonObject<T> : IJsonObject where T:NimJsonObject<T>
    {
        public virtual string Serialize()
        {
            return NimUtility.Json.JsonParser.Serialize(this,IgnoreDefauleValue);
        }

        public virtual string SerializeWithIndented()
        {
            return NimUtility.Json.JsonParser.SerializeWithIndented(this);
        }

        public static T Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json))
                return default(T);
            try
            {
                T ret = NimUtility.Json.JsonParser.Deserialize<T>(json);
                ret.RawJson = json;
                return ret;
            }
            catch (Exception e)
            {
                NimUtility.Log.Error("SDKJsonObject.Deserialize\r\n" + e.ToString() + "\r\njson:" + json);
                return default(T);
            }
        }

        [JsonIgnore]
        public string RawJson { get; private set; }

        [JsonIgnore]
        protected virtual bool IgnoreDefauleValue { get; set; }
    }
}
