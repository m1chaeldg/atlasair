using api.Controllers;
using api.Model;
using api.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace api.test
{
    public class ProductControllerTest
    {
        [Fact]
        public void GetReturnAllProducts()
        {
            // setup

            var list = (new List<Product>
            {
                new Product
                {
                    Id = "1",
                    Make ="Toyota",
                    Manufacturer="Gold",
                    Model="XYZ",
                    Year="2015"
                },
                new Product
                {
                    Id = "2",
                    Make ="Tesla",
                    Manufacturer="SpaceX",
                    Model="Rocket",
                    Year="2012"
                },
                new Product
                {
                    Id = "3",
                    Make ="Dell",
                    Manufacturer="AMD",
                    Model="Rocket",
                    Year="2012"
                },
            }).AsReadOnly();

            var mock = new Mock<IFileDb>();
            mock.Setup(foo => foo.GetAll()).Returns(list);

            var ctrl = new ProductsController(mock.Object);

            // act
            var result = ctrl.Get();

            //assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.IsAssignableFrom<IReadOnlyList<Product>>(okResult.Value);

            IReadOnlyList<Product> returnList = (IReadOnlyList<Product>)okResult.Value;

            Assert.Equal(returnList, list);

            Assert.Equal(returnList[0].Id, list[0].Id);
            Assert.Equal(returnList[1].Id, list[1].Id);
            Assert.Equal(returnList[2].Id, list[2].Id);
        }


        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        public void GetByIdReturnSpecifiedProduct(string id)
        {
            // setup

            var dic = new Dictionary<string, Product>
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

            var mock = new Mock<IFileDb>();
            mock.Setup(c => c.Exist(It.IsAny<string>())).Returns(true);
            mock.Setup(c => c.Get(It.IsAny<string>())).Returns(dic[id]);

            var ctrl = new ProductsController(mock.Object);

            var expectedProduct = dic[id];
            // act
            var result = ctrl.Get(id);

            //assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.IsAssignableFrom<Product>(okResult.Value);

            var returnValue = (Product)okResult.Value;

            Assert.Equal(returnValue.Id, expectedProduct.Id);
            Assert.Equal(returnValue.Make, expectedProduct.Make);
            Assert.Equal(returnValue.Manufacturer, expectedProduct.Manufacturer);
            Assert.Equal(returnValue.Year, expectedProduct.Year);
            Assert.Equal(returnValue.Model, expectedProduct.Model);
        }

        
        [Fact]
        public void GetByIdReturnNotFoundWhenProductIdIsNotInDb()
        {
            // setup
            string idNotFound = "10";
            var dic = new Dictionary<string, Product>
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

            var mock = new Mock<IFileDb>();
            mock.Setup(c => c.Exist(It.IsAny<string>())).Returns(false);

            var ctrl = new ProductsController(mock.Object);

            // act
            var result = ctrl.Get(idNotFound);

            //assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void CanCreateProductWhenInputIsValid()
        {
            // setup
            var prod = new ProductCreate
            {
                Make = "Toyota",
                Manufacturer = "Gold",
                Model = "XYZ",
                Year = "2015"
            };

            var mock = new Mock<IFileDb>();
            mock.Setup(c => c.Insert(It.IsAny<Product>()));

            var ctrl = new ProductsController(mock.Object);

            // act
            var result = ctrl.Post(prod);

            //assert
            Assert.IsType<CreatedResult>(result);

            mock.Verify(c => c.Insert(It.IsAny<Product>()));
        }

        [Theory]
        [InlineData(null,"Gold","XYZ","2015")]
        [InlineData("Toyota", null, "XYZ", "2015")]
        [InlineData("Toyota", "Gold", null, "2015")]
        [InlineData("Toyota", "Gold", "XYZ", null)]
        public void CanNotCreateProductWhenOneFieldIsInvalid(string make, string manufacturer, string model, string year)
        {
            // setup
            var prod = new ProductCreate
            {
                Make = make,
                Manufacturer = manufacturer,
                Model = model,
                Year = year
            };

            var mock = new Mock<IFileDb>();

            var ctrl = new ProductsController(mock.Object);
            if(make==null)
                ctrl.ModelState.AddModelError(nameof(prod.Make), "Required");
            if (manufacturer == null)
                ctrl.ModelState.AddModelError(nameof(prod.Manufacturer), "Required");
            if (model == null)
                ctrl.ModelState.AddModelError(nameof(prod.Model), "Required");
            if (year == null)
                ctrl.ModelState.AddModelError(nameof(prod.Year), "Required");
            // act
            var result = ctrl.Post(prod);

            //assert
            Assert.IsType<BadRequestObjectResult>(result);

            mock.Verify(c => c.Insert(It.IsAny<Product>()), Times.Never());
        }

        [Fact]
        public void CanNotCreateProductWhenPostBodyIsNull()
        {
            // setup
            var mock = new Mock<IFileDb>();
            mock.Setup(c => c.Insert(It.IsAny<Product>()));

            var ctrl = new ProductsController(mock.Object);

            // act
            var result = ctrl.Post(null);

            //assert
            Assert.IsType<BadRequestResult>(result);

            mock.Verify(c => c.Insert(It.IsAny<Product>()), Times.Never());
        }


        [Fact]
        public void CanUpdateProductWhenInputIsValid()
        {
            // setup
            var prod = new ProductUpdate
            {
                Id="23456789",
                Make = "Toyota",
                Manufacturer = "Gold",
                Model = "XYZ",
                Year = "2015"
            };

            var mock = new Mock<IFileDb>();
            mock.Setup(c => c.Update(It.IsAny<Product>()));
            mock.Setup(c => c.Exist(It.IsAny<string>())).Returns(true);

            var ctrl = new ProductsController(mock.Object);

            // act
            var result = ctrl.Put(prod.Id, prod);

            //assert
            Assert.IsType<OkResult>(result);

            mock.Verify(c => c.Update(It.IsAny<Product>()));
        }

        [Fact]
        public void CanNotUpdateProductWhenProductDoesNotExistInDb()
        {
            // setup
            var prod = new ProductUpdate
            {
                Id = "23456789",
                Make = "Toyota",
                Manufacturer = "Gold",
                Model = "XYZ",
                Year = "2015"
            };

            var mock = new Mock<IFileDb>();
            mock.Setup(c => c.Update(It.IsAny<Product>()));
            mock.Setup(c => c.Exist(It.IsAny<string>())).Returns(false);

            var ctrl = new ProductsController(mock.Object);

            // act
            var result = ctrl.Put(prod.Id, prod);

            //assert
            Assert.IsType<NotFoundResult>(result);

            mock.Verify(c => c.Update(It.IsAny<Product>()), Times.Never());
        }

        [Fact]
        public void CanNotUpdateProductWhenProductIsNull()
        {
            // setup
            var prod = new ProductUpdate
            {
                Id = "23456789",
                Make = "Toyota",
                Manufacturer = "Gold",
                Model = "XYZ",
                Year = "2015"
            };

            var mock = new Mock<IFileDb>();
            mock.Setup(c => c.Update(It.IsAny<Product>()));
            mock.Setup(c => c.Exist(It.IsAny<string>())).Returns(false);

            var ctrl = new ProductsController(mock.Object);

            // act
            var result = ctrl.Put(prod.Id, null);

            //assert
            Assert.IsType<BadRequestResult>(result);

            mock.Verify(c => c.Update(It.IsAny<Product>()), Times.Never());
        }

        [Fact]
        public void CanNotUpdateProductWhenProductIdIsDifferentFromTheIdInBody()
        {
            // setup
            var prod = new ProductUpdate
            {
                Id = "23456789",
                Make = "Toyota",
                Manufacturer = "Gold",
                Model = "XYZ",
                Year = "2015"
            };

            var mock = new Mock<IFileDb>();
            mock.Setup(c => c.Update(It.IsAny<Product>()));
            mock.Setup(c => c.Exist(It.IsAny<string>())).Returns(false);

            var ctrl = new ProductsController(mock.Object);

            // act
            var result = ctrl.Put("9058jf", prod);

            //assert
            Assert.IsType<BadRequestResult>(result);

            mock.Verify(c => c.Update(It.IsAny<Product>()), Times.Never());
        }

        [Theory]
        [InlineData("56789", null, "Gold", "XYZ", "2015")]
        [InlineData("56789", "Toyota", null, "XYZ", "2015")]
        [InlineData("56789", "Toyota", "Gold", null, "2015")]
        [InlineData("56789", "Toyota", "Gold", "XYZ", null)]
        public void CanNotUpdateProductWhenOneFieldIsInvalid(string id, string make, string manufacturer, string model, string year)
        {
            // setup
            var prod = new ProductUpdate
            {
                Id = id,
                Make = make,
                Manufacturer = manufacturer,
                Model = model,
                Year = year
            };

            var mock = new Mock<IFileDb>();
            mock.Setup(c => c.Update(It.IsAny<Product>()));
            mock.Setup(c => c.Exist(It.IsAny<string>())).Returns(true);

            var ctrl = new ProductsController(mock.Object);
            if (id == null)
                ctrl.ModelState.AddModelError(nameof(prod.Id), "Required");
            if (make == null)
                ctrl.ModelState.AddModelError(nameof(prod.Make), "Required");
            if (manufacturer == null)
                ctrl.ModelState.AddModelError(nameof(prod.Manufacturer), "Required");
            if (model == null)
                ctrl.ModelState.AddModelError(nameof(prod.Model), "Required");
            if (year == null)
                ctrl.ModelState.AddModelError(nameof(prod.Year), "Required");
            // act
            var result = ctrl.Put(id, prod);

            //assert
            Assert.IsType<BadRequestObjectResult>(result);

            mock.Verify(c => c.Insert(It.IsAny<Product>()), Times.Never());
        }

        [Fact]
        public void CanDeleteProdctWhenProductIdExistInDb()
        {
            // setup
            var id = "9058jf";
            var mock = new Mock<IFileDb>();
            mock.Setup(c => c.Delete(id));
            mock.Setup(c => c.Exist(id)).Returns(true);

            var ctrl = new ProductsController(mock.Object);

            // act
            var result = ctrl.Delete(id);

            //assert
            Assert.IsType<OkResult>(result);

            mock.Verify(c => c.Delete(id));
            mock.Verify(c => c.Exist(id));
        }

        [Fact]
        public void CanNotDeleteProdctWhenProductIdDoesNotExistInDb()
        {
            // setup
            var id = "9058jf";
            var mock = new Mock<IFileDb>();
            mock.Setup(c => c.Delete(id));
            mock.Setup(c => c.Exist(It.IsAny<string>())).Returns(false);

            var ctrl = new ProductsController(mock.Object);

            // act
            var result = ctrl.Delete(id);

            //assert
            Assert.IsType<NotFoundResult>(result);

            mock.Verify(c => c.Delete("9058jf"), Times.Never());
            mock.Verify(c => c.Exist(It.IsAny<string>()));
        }
    }
}
