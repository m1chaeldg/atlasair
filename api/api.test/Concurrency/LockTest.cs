using api.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace api.test.Concurrency
{
    public class LockTest
    {
        [Fact]
        public void LoopWithUpdate()
        {
            using (var tempFile = new TempFile("{}"))
            {
                FileDb db = new FileDb(new Storage(), tempFile.FileName);
                db.Initialize();

                db.Insert(new Product
                {
                    Make = "Mtfgusdf0",
                    Manufacturer = "Mtuasd",
                    Model = "asdasdh",
                    Year = "211"
                });

                db.Insert(new Product
                {
                    Make = "Mtfgusdf0",
                    Manufacturer = "Mtuasd",
                    Model = "asdasdh",
                    Year = "211"
                });

                db.Insert(new Product
                {
                    Make = "Mtfgusdf0",
                    Manufacturer = "Mtuasd",
                    Model = "asdasdh",
                    Year = "211"
                });


                FileDb db2 = new FileDb(new Storage(), tempFile.FileName);
                db2.Initialize();
                foreach (var record in db2.GetAll())
                {
                    record.Model = "ModelXYZ";

                    db.Update(record);
                }

                var records = db2.GetAll();
                foreach (var record in records)
                {
                    Assert.Equal("ModelXYZ", record.Model);
                }

                Assert.Equal(3, records.Count);
            }

        }
    }
}
