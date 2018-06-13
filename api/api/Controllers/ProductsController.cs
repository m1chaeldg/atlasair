using api.Model;
using api.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        IFileDb db;

        public ProductsController(IFileDb db)
        {
            this.db = db;
        }

        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            var product = db.GetAll();

            return new OkObjectResult(product);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            if (!db.Exist(id))
                return new NotFoundResult();

            var product = db.Get(id);

            return new OkObjectResult(product);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] ProductCreate product)
        {
            if(product == null)
                return new BadRequestResult();

            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState);

            var model = new Product
            {
                Make = product.Make,
                Model = product.Model,
                Manufacturer = product.Manufacturer,
                Year = product.Year
            };

            db.Insert(model);

            return new CreatedResult($"api/products/${model.Id}", model);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] ProductUpdate product)
        {
            if ( product == null || product.Id != id)
                return new BadRequestResult();

            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState);

            if (!db.Exist(id))
                return new NotFoundResult();

            var model = new Product
            {
                Id = product.Id,
                Make = product.Make,
                Model = product.Model,
                Manufacturer = product.Manufacturer,
                Year = product.Year
            };

            db.Update(model);

            return new OkResult();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (!db.Exist(id))
                // bad request or not found?
                return new NotFoundResult();

            db.Delete(id);

            return new OkResult();
        }
    }
}
