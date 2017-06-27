using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace HRMTS.WebhookClient
{
    [Serializable]
    public class PostData
    {
        public PostData()
        {
            Items = new List<PostItem>();
        }

        private static string PostDataFile
        {
            get
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + @"\PostData.xml";

                return path;
            }
        }

        public List<PostItem> Items { get; set; }

        public string ToXml()
        {
            try
            {
                var xmlSerializer = new XmlSerializer(GetType());

                using (var textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, this);

                    return textWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static PostData FromXml(string xml)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(PostData));

                var textReader = new StringReader(xml);

                return (PostData) xmlSerializer.Deserialize(textReader);
            }
            catch
            {
                return new PostData();
            }
        }

        public static void WriteToDisk(PostData postData)
        {
            File.WriteAllText(PostDataFile, postData.ToXml());
        }

        public static PostData ReadFromDisk()
        {
            return !File.Exists(PostDataFile)
                ? new PostData()
                : FromXml(File.ReadAllText(PostDataFile));
        }
    }

    [Serializable]
    public class PostItem
    {
        public PostItem()
        {
            Headers = new List<KeyValue>();

            Body = string.Empty;
        }

        public List<KeyValue> Headers { get; set; }

        public string Body { get; set; }
    }

    [Serializable]
    public class KeyValue
    {
        public KeyValue()
        {
        }

        public KeyValue(string key, string value)
        {
            Key = key;

            Value = value;
        }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}