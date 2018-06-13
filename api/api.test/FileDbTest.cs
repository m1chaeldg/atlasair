using api.Model;
using Moq;
using Xunit;

namespace api.test
{
    public class FileDbTest
    {
        [Fact]
        public void CanLoadInitialData()
        {
            // setup
            Document document = new Document
            {
                ["1"] = new Product
                {
                    Id = "1",
                    Make = "Toyota",
                    Manufacturer = "Gold",
                    Model = "XYZ",
                    Year = "2015"
                },

                ["2"] = new Product
                {
                    Id = "2",
                    Make = "Tesla",
                    Manufacturer = "SpaceX",
                    Model = "Rocket",
                    Year = "2012"
                },

                ["3"] = new Product
                {
                    Id = "3",
                    Make = "Dell",
                    Manufacturer = "AMD",
                    Model = "Rocket",
                    Year = "2012"
                }
            };

            var mock = new Mock<IStorage>();
            mock.Setup(c => c.Load(It.IsAny<string>())).Returns(document);
            mock.Setup(c => c.Save(It.IsAny<Document>(), It.IsAny<string>()))
                .Callback((Document saveDocument, string fileName) =>
                {
                    document = saveDocument;
                });

            var fileDb = new FileDb(mock.Object);

            // act
            fileDb.Initialize();

            //assert
            Assert.Equal(3, fileDb.Documents.Count);
            Assert.Equal("3", fileDb.Documents["3"].Id);
            mock.Verify(c => c.Load(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void CanLoadInitialData_Empty()
        {
            // setup
            Document document = new Document();

            var mock = new Mock<IStorage>();
            mock.Setup(c => c.Load(It.IsAny<string>())).Returns(document);
            mock.Setup(c => c.Save(It.IsAny<Document>(), It.IsAny<string>()))
                .Callback((Document saveDocument, string fileName) =>
                {
                    document = saveDocument;
                });

            var fileDb = new FileDb(mock.Object);

            // act
            fileDb.Initialize();

            //assert
            Assert.Empty(fileDb.Documents);
            mock.Verify(c => c.Load(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void CanInsert()
        {
            // setup
            Document document = new Document();

            var mock = new Mock<IStorage>();
            mock.Setup(c => c.Load(It.IsAny<string>())).Returns(document);
            mock.Setup(c => c.Save(It.IsAny<Document>(), It.IsAny<string>()))
                .Callback((Document saveDocument, string fileName) =>
                {
                    document = saveDocument;
                });

            var fileDb = new FileDb(mock.Object);
            fileDb.Initialize();

            var prod = new Product
            {
                Make = "Dell",
                Manufacturer = "AMD",
                Model = "Rocket",
                Year = "2012"
            };

            // act
            fileDb.Insert(prod);

            //assert
            Assert.Single(fileDb.Documents);
            Assert.NotEmpty(prod.Id);
            Assert.NotNull(fileDb.Documents[prod.Id]);
            Assert.NotNull(fileDb.Get(prod.Id));
            mock.Verify(c => c.Save(It.IsAny<Document>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void CanUpdate()
        {
            // setup
            Document document = new Document();

            var mock = new Mock<IStorage>();
            mock.Setup(c => c.Load(It.IsAny<string>())).Returns(document);
            mock.Setup(c => c.Save(It.IsAny<Document>(), It.IsAny<string>()))
                .Callback((Document saveDocument, string fileName) =>
                {
                    document = saveDocument;
                });

            var fileDb = new FileDb(mock.Object);
            fileDb.Initialize();

            var prod = new Product
            {
                Make = "Dell",
                Manufacturer = "AMD",
                Model = "Rocket",
                Year = "2012"
            };
            fileDb.Insert(prod);

            // act
            fileDb.Update(new Product
            {
                Id = prod.Id,
                Make = "Dellx",
                Manufacturer = "AMDx",
                Model = "Rocketx",
                Year = "2015"
            });

            //assert
            var retrieveProd = fileDb.Get(prod.Id);

            Assert.Equal("Dellx", retrieveProd.Make);
            Assert.Equal("AMDx", retrieveProd.Manufacturer);
            Assert.Equal("Rocketx", retrieveProd.Model);
            Assert.Equal("2015", retrieveProd.Year);
            mock.Verify(c => c.Save(It.IsAny<Document>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public void CanUpdateOnlySingleRecord()
        {
            // setup
            Document document = new Document();

            var mock = new Mock<IStorage>();
            mock.Setup(c => c.Load(It.IsAny<string>())).Returns(document);
            mock.Setup(c => c.Save(It.IsAny<Document>(), It.IsAny<string>()))
                .Callback((Document saveDocument, string fileName) =>
                {
                    document = saveDocument;
                });

            var fileDb = new FileDb(mock.Object);
            fileDb.Initialize();

            var prod = new Product
            {
                Make = "Dell",
                Manufacturer = "AMD",
                Model = "Rocket",
                Year = "2012"
            };
            fileDb.Insert(prod);

            // act
            fileDb.Update(new Product
            {
                Id = prod.Id,
                Make = "Dellx",
                Manufacturer = "AMDx",
                Model = "Rocketx",
                Year = "2015"
            });
            fileDb.Update(new Product
            {
                Id = prod.Id,
                Make = "Dellxy",
                Manufacturer = "AMDxy",
                Model = "Rocketxy",
                Year = "2016"
            });
            //assert
            var retrieveProd = fileDb.Get(prod.Id);

            Assert.Single(fileDb.Documents);
            Assert.Equal("Dellxy", retrieveProd.Make);
            Assert.Equal("AMDxy", retrieveProd.Manufacturer);
            Assert.Equal("Rocketxy", retrieveProd.Model);
            Assert.Equal("2016", retrieveProd.Year);
        }

        [Fact]
        public void CanGetRecord()
        {
            // setup
            Document document = new Document();

            var mock = new Mock<IStorage>();
            mock.Setup(c => c.Load(It.IsAny<string>())).Returns(document);
            mock.Setup(c => c.Save(It.IsAny<Document>(), It.IsAny<string>()))
                .Callback((Document saveDocument, string fileName) =>
                {
                    document = saveDocument;
                });

            var fileDb = new FileDb(mock.Object);
            fileDb.Initialize();

            var prod = new Product
            {
                Make = "Dell",
                Manufacturer = "AMD",
                Model = "Rocket",
                Year = "2012"
            };
            fileDb.Insert(prod);

            // act
            var retrieveProd = fileDb.Get(prod.Id);
            //assert

            Assert.Single(fileDb.Documents);
            Assert.NotNull(retrieveProd);
            Assert.Equal("Dell", retrieveProd.Make);
            Assert.Equal("AMD", retrieveProd.Manufacturer);
            Assert.Equal("Rocket", retrieveProd.Model);
            Assert.Equal("2012", retrieveProd.Year);
        }

        [Fact]
        public void CanNotGetProductWhenItDoesNotExist()
        {
            // setup
            Document document = new Document();

            var mock = new Mock<IStorage>();
            mock.Setup(c => c.Load(It.IsAny<string>())).Returns(document);
            mock.Setup(c => c.Save(It.IsAny<Document>(), It.IsAny<string>()))
                .Callback((Document saveDocument, string fileName) =>
                {
                    document = saveDocument;
                });

            var fileDb = new FileDb(mock.Object);
            fileDb.Initialize();

            // act
            var retrieveProd = fileDb.Get("345678");
            //assert

            Assert.Null(retrieveProd);
        }

        [Fact]
        public void CanGetAllRecord()
        {
            // setup
            Document document = new Document
            {
                ["1"] = new Product
                {
                    Id = "1",
                    Make = "Toyota",
                    Manufacturer = "Gold",
                    Model = "XYZ",
                    Year = "2015"
                },

                ["2"] = new Product
                {
                    Id = "2",
                    Make = "Tesla",
                    Manufacturer = "SpaceX",
                    Model = "Rocket",
                    Year = "2012"
                },

                ["3"] = new Product
                {
                    Id = "3",
                    Make = "Dell",
                    Manufacturer = "AMD",
                    Model = "Rocket",
                    Year = "2012"
                }
            };

            var mock = new Mock<IStorage>();
            mock.Setup(c => c.Load(It.IsAny<string>())).Returns(document);
            mock.Setup(c => c.Save(It.IsAny<Document>(), It.IsAny<string>()))
                .Callback((Document saveDocument, string fileName) =>
                {
                    document = saveDocument;
                });

            var fileDb = new FileDb(mock.Object);
            fileDb.Initialize();

            var prod = new Product
            {
                Make = "Dell",
                Manufacturer = "AMD",
                Model = "Rocket",
                Year = "2012"
            };
            fileDb.Insert(prod);

            // act
            var retrieveProds = fileDb.GetAll();
            //assert

            Assert.Equal(4, retrieveProds.Count);
        }

        [Fact]
        public void CanGetAllRecordThatReturnEmptyWhenNoData()
        {
            // setup
            Document document = new Document();

            var mock = new Mock<IStorage>();
            mock.Setup(c => c.Load(It.IsAny<string>())).Returns(document);
            mock.Setup(c => c.Save(It.IsAny<Document>(), It.IsAny<string>()))
                .Callback((Document saveDocument, string fileName) =>
                {
                    document = saveDocument;
                });

            var fileDb = new FileDb(mock.Object);
            fileDb.Initialize();

            // act
            var retrieveProds = fileDb.GetAll();
            //assert

            Assert.Empty(retrieveProds);
        }

        [Fact]
        public void CanCheckIfProductExist()
        {
            // setup
            Document document = new Document
            {
                ["1"] = new Product
                {
                    Id = "1",
                    Make = "Toyota",
                    Manufacturer = "Gold",
                    Model = "XYZ",
                    Year = "2015"
                },

                ["2"] = new Product
                {
                    Id = "2",
                    Make = "Tesla",
                    Manufacturer = "SpaceX",
                    Model = "Rocket",
                    Year = "2012"
                },

                ["3"] = new Product
                {
                    Id = "3",
                    Make = "Dell",
                    Manufacturer = "AMD",
                    Model = "Rocket",
                    Year = "2012"
                }
            };

            var mock = new Mock<IStorage>();
            mock.Setup(c => c.Load(It.IsAny<string>())).Returns(document);
            mock.Setup(c => c.Save(It.IsAny<Document>(), It.IsAny<string>()))
                .Callback((Document saveDocument, string fileName) =>
                {
                    document = saveDocument;
                });

            var fileDb = new FileDb(mock.Object);
            fileDb.Initialize();

            // act
            var returnFalse = fileDb.Exist("34567");
            var returnTrue = fileDb.Exist("1");

            //assert
            Assert.True(returnTrue);
            Assert.False(returnFalse);
        }

        [Fact]
        public void CanRemoveProductThatExist()
        {
            // setup
            Document document = new Document
            {
                ["1"] = new Product
                {
                    Id = "1",
                    Make = "Toyota",
                    Manufacturer = "Gold",
                    Model = "XYZ",
                    Year = "2015"
                },

                ["2"] = new Product
                {
                    Id = "2",
                    Make = "Tesla",
                    Manufacturer = "SpaceX",
                    Model = "Rocket",
                    Year = "2012"
                },

                ["3"] = new Product
                {
                    Id = "3",
                    Make = "Dell",
                    Manufacturer = "AMD",
                    Model = "Rocket",
                    Year = "2012"
                }
            };

            var mock = new Mock<IStorage>();
            mock.Setup(c => c.Load(It.IsAny<string>())).Returns(document);
            mock.Setup(c => c.Save(It.IsAny<Document>(), It.IsAny<string>()))
                .Callback( (Document saveDocument, string fileName) =>
            {
                document = saveDocument;
            });

            var fileDb = new FileDb(mock.Object);
            fileDb.Initialize();

            // act
            fileDb.Delete("1");
            fileDb.Delete("2");
            fileDb.Delete("3");

            //assert
            Assert.Empty(fileDb.Documents);
            mock.Verify(c => c.Save(It.IsAny<Document>(), It.IsAny<string>()), Times.Exactly(3));
        }
    }

    /*public class MockStorage : IStorage
    {
        Document document;
        public MockStorage(Document document)
        {
            this.document = document;
        }
        public Document Load(string fileName)
        {
            return document;
        }

        public void Save(Document document, string fileName)
        {
            this.document = document;
        }
    }*/
}
