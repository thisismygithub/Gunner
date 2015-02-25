using System;
using System.Text;
using System.Data;
using System.Xml;
using System.IO;
using Newtonsoft.Json;

namespace CMoney.WebBackend.WebFunction
{
    public class DataConverter
    {
        /// <summary>
        /// Convert a DataTable into an XML string.
        /// </summary>
        /// <param name="dataTable">DataTable</param>
        /// <param name="strTableName">Table Name</param>
        /// <returns></returns>
        public static string ToXmlString(DataTable dataTable, string strTableName)
        {
            if (dataTable == null)
            {
                return "dataTable is null.";
            }

            XmlDocument xmlDoc = ToXmlDocument(dataTable, strTableName);
            return xmlDoc.InnerXml;
        }

        /// <summary>
        /// Convert a DataSet into an XML string.
        /// </summary>
        /// <param name="dataSet">DataSet</param>
        /// <param name="strTableName">Table Name</param>
        /// <returns></returns>
        public static string ToXmlString(DataSet dataSet, string strTableName)
        {
            if (dataSet == null) return "dataSet is null.";

            XmlDocument xmlDoc;
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < dataSet.Tables.Count; ++i)
            {
                xmlDoc = ToXmlDocument(dataSet.Tables[i], strTableName + i.ToString());
                sb.Append(xmlDoc.InnerXml);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Convert a DataTable into an XML document.
        /// </summary>
        /// <param name="dataTable">DataTable</param>
        /// <param name="strTableName">Table Name</param>
        /// <returns></returns>
        public static XmlDocument ToXmlDocument(DataTable dataTable, string strTableName)
        {
            StringWriter stringWriter = new StringWriter();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
            dataTable.TableName = strTableName;
            dataTable.WriteXml(xmlTextWriter, XmlWriteMode.IgnoreSchema);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(stringWriter.ToString());

            return xmlDoc;
        }

        public static string Serialize(object value)
        {
            Type type = value.GetType();

            Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();

            json.NullValueHandling = NullValueHandling.Ignore;

            json.ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace;
            json.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
            json.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            if (type == typeof(DataRow))
            {
                json.Converters.Add(new DataRowConverter());
            }
            else if (type == typeof(DataTable))
            {
                json.Converters.Add(new DataTableConverter());
            }
            else if (type == typeof(DataSet))
            {
                json.Converters.Add(new DataSetConverter());
            }
            string output;
            using (StringWriter sw = new StringWriter())
            using (Newtonsoft.Json.JsonTextWriter writer = new JsonTextWriter(sw))
            {
                writer.QuoteChar = '"';
                json.Serialize(writer, value);

                output = sw.ToString();
                writer.Close();
                sw.Close();
            }

            return output;
        }

        public static object Deserialize(string jsonText, Type valueType)
        {
            Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();

            json.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            json.ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace;
            json.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
            json.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            StringReader sr = new StringReader(jsonText);
            Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
            object result = json.Deserialize(reader, valueType);
            reader.Close();

            return result;
        }

        public static T DeserializeObject<T>(string jsonText)
        {
            return JsonConvert.DeserializeObject<T>(jsonText);
        }

        /// <summary>
        /// Converts a <see cref="DataRow"/> object to and from JSON.
        /// </summary>
        public class DataRowConverter : JsonConverter
        {
            public DataRowConverter()
            {
                //
                // TODO: Add constructor logic here
                //
            }

            /// <summary>
            /// Writes the JSON representation of the object.
            /// </summary>
            /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
            /// <param name="value">The value.</param>
            public override void WriteJson(JsonWriter writer, object dataRow, JsonSerializer ser)
            {
                DataRow row = dataRow as DataRow;

                // *** HACK: need to use root serializer to write the column value
                //     should be fixed in next ver of JSON.NET with writer.Serialize(object)
                //JsonSerializer ser = new JsonSerializer();

                writer.WriteStartObject();
                foreach (DataColumn column in row.Table.Columns)
                {
                    writer.WritePropertyName(column.ColumnName);
                    ser.Serialize(writer, row[column]);
                }
                writer.WriteEndObject();
            }

            /// <summary>
            /// Determines whether this instance can convert the specified value type.
            /// </summary>
            /// <param name="valueType">Type of the value.</param>
            /// <returns>
            ///     <c>true</c> if this instance can convert the specified value type; otherwise, <c>false</c>.
            /// </returns>
            public override bool CanConvert(Type valueType)
            {
                return typeof(DataRow).IsAssignableFrom(valueType);
            }

            /// <summary>
            /// Reads the JSON representation of the object.
            /// </summary>
            /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
            /// <param name="objectType">Type of the object.</param>
            /// <returns>The object value.</returns>
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer ser)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Converts a DataTable to JSON. Note no support for deserialization
        /// </summary>
        public class DataTableConverter : JsonConverter
        {
            public DataTableConverter()
            {
                //
                // TODO: Add constructor logic here
                //
            }

            /// <summary>
            /// Writes the JSON representation of the object.
            /// </summary>
            /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
            /// <param name="value">The value.</param>
            public override void WriteJson(JsonWriter writer, object dataTable, JsonSerializer ser)
            {
                DataTable table = dataTable as DataTable;
                DataRowConverter converter = new DataRowConverter();

                writer.WriteStartObject();

                writer.WritePropertyName("Rows");
                writer.WriteStartArray();

                foreach (DataRow row in table.Rows)
                {
                    converter.WriteJson(writer, row, ser);
                }

                writer.WriteEndArray();
                writer.WriteEndObject();
            }

            /// <summary>
            /// Determines whether this instance can convert the specified value type.
            /// </summary>
            /// <param name="valueType">Type of the value.</param>
            /// <returns>
            ///     <c>true</c> if this instance can convert the specified value type; otherwise, <c>false</c>.
            /// </returns>
            public override bool CanConvert(Type valueType)
            {
                return typeof(DataTable).IsAssignableFrom(valueType);
            }

            /// <summary>
            /// Reads the JSON representation of the object.
            /// </summary>
            /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
            /// <param name="objectType">Type of the object.</param>
            /// <returns>The object value.</returns>
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer ser)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Converts a <see cref="DataSet"/> object to JSON. No support for reading.
        /// </summary>
        public class DataSetConverter : JsonConverter
        {
            public DataSetConverter()
            {
                //
                // TODO: Add constructor logic here
                //
            }

            /// <summary>
            /// Writes the JSON representation of the object.
            /// </summary>
            /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
            /// <param name="value">The value.</param>
            public override void WriteJson(JsonWriter writer, object dataset, JsonSerializer ser)
            {
                DataSet dataSet = dataset as DataSet;

                DataTableConverter converter = new DataTableConverter();

                writer.WriteStartObject();

                writer.WritePropertyName("Tables");
                writer.WriteStartArray();

                foreach (DataTable table in dataSet.Tables)
                {
                    converter.WriteJson(writer, table, ser);
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }

            /// <summary>
            /// Determines whether this instance can convert the specified value type.
            /// </summary>
            /// <param name="valueType">Type of the value.</param>
            /// <returns>
            ///     <c>true</c> if this instance can convert the specified value type; otherwise, <c>false</c>.
            /// </returns>
            public override bool CanConvert(Type valueType)
            {
                return typeof(DataSet).IsAssignableFrom(valueType);
            }

            /// <summary>
            /// Reads the JSON representation of the object.
            /// </summary>
            /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
            /// <param name="objectType">Type of the object.</param>
            /// <returns>The object value.</returns>
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer ser)
            {
                throw new NotImplementedException();
            }
        }
    }
}
