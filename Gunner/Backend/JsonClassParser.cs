using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Gunner.Backend
{
    public class JsonClassParser
    {
        public class RawClass
        {
            public string ClassName;
            public string PropertyHash;
            public Dictionary<string, string> Properties;
        }

        //蒐集所有類別
        public static List<RawClass> Library
            = new List<RawClass>();
        private static string GetClsType(JTokenType t)
        {
            switch (t)
            {
                case JTokenType.Boolean:
                    return "bool";
                case JTokenType.Bytes:
                    return "byte[]";
                case JTokenType.Date:
                    return "DateTime";
                case JTokenType.Float:
                    return "decimal";
                case JTokenType.Integer:
                    return "int";
                case JTokenType.String:
                    return "string";
                default:
                    throw new ApplicationException(
                        t + " is not supported");
            }
        }

        //註冊類別
        public static string RegisterClass(JObject jo, string className)
        {
            var c = new RawClass
            {
                ClassName = "C" +
                            (className ??
                            Path.GetFileNameWithoutExtension(
                                Path.GetTempFileName())),
                PropertyHash = string.Join(",",
                    jo.Properties().Select(o => o.Name).ToArray()),
                Properties = new Dictionary<string, string>()
            };
            //將所有類別組成Hash字串，用以比對類別是否相同
            foreach (JProperty p in jo.Properties())
            {
                string t;
                switch (p.Value.Type)
                {
                    case JTokenType.Object:
                        t = RegisterClass((JObject)jo[p.Name], p.Name);
                        break;
                    case JTokenType.Array:
                        var ary = (JArray)jo[p.Name];
                        string typeName = null;
                        foreach (JToken jv in ary)
                        {
                            if (jv.Type == JTokenType.Array)
                                throw new ApplicationException(
                                    "Array of array is not supported!");
                            string s =
                                jv.Type == JTokenType.Object ?
                                RegisterClass((JObject)jv, p.Name) :
                                GetClsType(jv.Type);
                            if (typeName == null)
                                typeName = s;
                            else if (typeName != s)
                                throw new ApplicationException(
                                    "Complex array is not supported!");
                        }
                        t = typeName + "[]";
                        break;

                    case JTokenType.Boolean:
                    case JTokenType.Date:
                    case JTokenType.Float:
                    case JTokenType.Integer:
                    case JTokenType.String:
                        t = GetClsType(p.Value.Type);
                        break;
                    default:
                        throw new ApplicationException(
                        p.Type.ToString() + " is not supported!");
                }
                c.Properties.Add(p.Name, t);
            }
            //檢查是否已有重覆類別存在
            var q = (from o in Library
                     where o.PropertyHash == c.PropertyHash
                     select o).SingleOrDefault();
            //不存在時新增
            if (q == null)
            {
                Library.Add(c);
                return c.ClassName;
            }
            return q.ClassName;
        }

        public static string GenClassCode()
        {
            var sb = new StringBuilder();
            foreach (RawClass rc in Library)
            {
                sb.AppendFormat("public class {0} {{\r\n", rc.ClassName);
                foreach (string p in rc.Properties.Keys)
                {
                    sb.AppendFormat("  public {0} {1} {{ get; set; }}\r\n",
                        rc.Properties[p], p);
                }
                sb.AppendLine("}");
            }
            return sb.ToString();
        }
    }
}