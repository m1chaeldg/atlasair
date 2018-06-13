using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace api.Model
{
    public sealed class Storage : IStorage
    {
        public Document Load(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            if (!fileInfo.Exists)
                return new Document();

            JsonSerializer serializer = new JsonSerializer();
            //serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            try
            {
                using (var sr = fileInfo.OpenText())
                using (var reader = new JsonTextReader(sr))
                {
                    return serializer.Deserialize<Document>(reader);
                }
            }
            catch
            {
                return new Document();
            }
        }

        public void Save(Document document, string fileName)
        {
            JsonSerializer serializer = new JsonSerializer();
            //serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(fileName))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, document);
            }
        }
    }
}
