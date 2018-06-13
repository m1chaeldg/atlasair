using api.Model;
using System.IO;
using Xunit;

namespace api.test
{
    public class StorageTest
    {
        const string jsonData = "{\"42df9afa-fd7d-410c-9494-4f7e0945da15\":{\"Id\":\"42df9afa-fd7d-410c-9494-4f7e0945da15\",\"Manufacturer\":\"Ford\",\"Make\":\"Mustang\",\"Model\":\"GT\",\"Year\":\"2015\"}}";

        [Fact]
        public void CanLoadData()
        {
            using (var temp = new TempFile(jsonData))
            {
                // setup
                var expectedDocument = Newtonsoft.Json.JsonConvert.DeserializeObject<Document>(jsonData);

                Storage storage = new Storage();
                // act

               var doc = storage.Load(temp.FileName);

                // assert
                Assert.Equal(expectedDocument.Count, doc.Count);
                Assert.NotNull(doc["42df9afa-fd7d-410c-9494-4f7e0945da15"]);
            }

        }

        [Fact]
        public void CanSaveData()
        {
            using (var temp = new TempFile(jsonData))
            {
                // setup
                var doc = Newtonsoft.Json.JsonConvert.DeserializeObject<Document>(jsonData);
                doc["123456"] = new Product
                {
                    Id = "123456",
                    Make = "werty",
                    Manufacturer = "M",
                    Model = "model",
                    Year = "2020"
                };
                Storage storage = new Storage();


                // act
                storage.Save(doc, temp.FileName);

                var allText = File.ReadAllText(temp.File.FullName);
                var doc2 = Newtonsoft.Json.JsonConvert.DeserializeObject<Document>(allText);

                // assert
                Assert.Equal(doc.Count, doc2.Count);
                Assert.NotNull(doc2["42df9afa-fd7d-410c-9494-4f7e0945da15"]);
                Assert.NotNull(doc2["123456"]);
            }

        }
    }
}
