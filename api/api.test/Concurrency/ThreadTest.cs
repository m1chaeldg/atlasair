using api.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using System.Collections.Concurrent;

namespace api.test.Concurrency
{
    public class ThreadTest
    {
        [Fact]
        public void ThreadMultiInsert()
        {
            using (var tempFile = new TempFile("{}"))
            {
                FileDb db = new FileDb(new Storage(), tempFile.FileName);
                db.Initialize();

                // insert 1000 x thread=1
                var task1 = Task.Factory.StartNew(() =>
                {
                    for (var i = 0; i < 1000; i++)
                        db.Insert(new Product
                        {
                            Make = "Thread 1",
                            Manufacturer = "Mtuasd",
                            Model = "asdasdh",
                            Year = "2101"
                        });
                });

                // insert 2000 x thread=2
                var task2 = Task.Factory.StartNew(() =>
                {
                    for (var i = 0; i < 2000; i++)
                        db.Insert(new Product
                        {
                            Make = "Thread 2",
                            Manufacturer = "Mtuasd",
                            Model = "asdasdh",
                            Year = "2101"
                        });
                });

                Task.WaitAll(task1, task2);

                FileDb db2 = new FileDb(new Storage(), tempFile.FileName);
                db2.Initialize();

                var records = db2.GetAll();
                var thread1RecordCount = records.Count(c => c.Make == "Thread 1");
                var thread2RecordCount = records.Count(c => c.Make == "Thread 2");

                Assert.Equal(1000, thread1RecordCount);
                Assert.Equal(2000, thread2RecordCount);
            }

        }

        private async Task TaskUpdate(FileDb db, BlockingCollection<string> recordIds, int count)
        {
            string id;
            for (var i = 0; i < count; i++)
            {
                while (!recordIds.TryTake(out id))
                    await Task.Delay(50);

                var product = db.Get(id);
                product.Make = "Updated";

                db.Update(product);
            }
        }

        [Fact]
        public void ThreadInsertWhileUpdating()
        {
            using (var tempFile = new TempFile("{}"))
            {
                FileDb db = new FileDb(new Storage(), tempFile.FileName);
                db.Initialize();

                const int N = 1000;
                var recordIds = new BlockingCollection<string>();

                var task1 = Task.Factory.StartNew(() =>
                {
                    for (var i = 0; i < N; i++)
                    {
                        var product = new Product
                        {
                            Make = $"Insert {i}",
                            Manufacturer = "Mtuasd",
                            Model = "asdasdh",
                            Year = "2101"
                        };

                        db.Insert(product);

                        recordIds.Add(product.Id);
                    }
                        
                });

                var task = TaskUpdate(db, recordIds, N);

                Task.WaitAll(task1, task);

                FileDb db2 = new FileDb(new Storage(), tempFile.FileName);
                db2.Initialize();

                var records = db2.GetAll();
                var recordCount = records.Count(c => c.Make == "Updated");

                Assert.Equal(N, recordCount);
            }

        }

        [Fact]
        public void ThreadInsertWhileGetProduct()
        {
            using (var tempFile = new TempFile("{}"))
            {
                FileDb db = new FileDb(new Storage(), tempFile.FileName);
                db.Initialize();

                const int N = 1000;
                var recordIds = new BlockingCollection<string>();
                bool running = true;

                var task1 = Task.Factory.StartNew(() =>
                {
                    for (var i = 0; i < N; i++)
                    {
                        var product = new Product
                        {
                            Make = $"Insert {i}",
                            Manufacturer = "Mtuasd",
                            Model = "asdasdh",
                            Year = "2101"
                        };

                        db.Insert(product);

                        recordIds.Add(product.Id);
                    }

                    running = false;

                });

                var task2 = Task.Factory.StartNew(() =>
                {
                    while (running)
                    {
                        var id = recordIds.Take();
                        var product = db.Get(id);
                        var products = db.GetAll();
                    }

                });

                Task.WaitAll(task1, task2);

                FileDb db2 = new FileDb(new Storage(), tempFile.FileName);
                db2.Initialize();

                var records = db2.GetAll();
                var recordCount = records.Count();

                Assert.Equal(N, recordCount);
            }

        }
    }
}
