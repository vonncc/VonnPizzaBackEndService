using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using VonnPizzaBackEndService.Models;
using VonnPizzaBackEndService.Services;
using CsvHelper;

namespace VonnPizzaBackEndService.Controllers
{
    public class PizzaUploadDto
    {
        public IFormFile pizzaSchema { get; set; }
        public IFormFile pizzaTypeSchema { get; set; }
    }
    [ApiController]
    [Route("apis/[controller]")]
    public class PizzasController : Controller
    {
        private readonly PizzasServices _pizzaService;

        public PizzasController(PizzasServices pizzaService)
        {
            _pizzaService = pizzaService;
        }

        // GET: PizzasController/GetAll
        [HttpGet]
        public IActionResult GetAllPizzas()
        {
            var pizzas = _pizzaService.GetAllPizzas();
            return Ok(pizzas);
        }

        // GET: PizzasController/GetByID
        [HttpGet("{id}")]
        public IActionResult GetPizzaById(string id)
        {
            var pizza = _pizzaService.GetPizzaById(id);
            if (pizza == null)
            {
                return NotFound();
            }
            return Ok(pizza);
        }
        
        // POST: PizzasController/Add
        [HttpPost]
        public IActionResult AddPizza([FromBody] Pizzas pizza)
        {
            _pizzaService.AddPizza(pizza);
            return CreatedAtAction(nameof(GetPizzaById), new { id = pizza.PizzaID }, pizza);
        }

        // PUT: PizzasController/Update
        [HttpPut("{id}")]
        public IActionResult UpdatePizza(string id, [FromBody] Pizzas pizza)
        {
            if (id != pizza.PizzaID)
            {
                return BadRequest();
            }
            _pizzaService.UpdatePizza(pizza);
            return NoContent();
        }

        // POST: PizzasController/Delete
        [HttpDelete("{id}")]
        public IActionResult DeletePizza(int id)
        {
            _pizzaService.DeletePizza(id);
            return NoContent();
        }

        // POST: PizzasController/Import
        [HttpPost("import")]
        public IActionResult ImportCsv([FromForm] PizzaUploadDto pizzaUploadDto)
        {
            if (pizzaUploadDto.pizzaSchema == null || pizzaUploadDto.pizzaSchema.Length == 0 || pizzaUploadDto.pizzaTypeSchema == null || pizzaUploadDto.pizzaTypeSchema.Length == 0)
            {
                return BadRequest("Both CSV files are required.");
            }

            // Process the PizzaType CSV file
            var pizzaTypeRecords = ProcessPizzaTypeCsv(pizzaUploadDto.pizzaTypeSchema);

            // Process the Pizza CSV file
            var pizzaRecords = ProcessPizzaCsv(pizzaUploadDto.pizzaSchema);

            // Merge the records from both schemas
            var mergedRecords = MergeRecords(pizzaTypeRecords, pizzaRecords);

            // Save the merged records in chunks
            SaveInChunks(mergedRecords);

            return Ok("CSV files imported successfully.");

        }

        private List<PizzaTypeImportModel> ProcessPizzaTypeCsv(IFormFile pizzaTypeSchema)
        {
            var pizzaTypeRecords = new List<PizzaTypeImportModel>();

            using (var reader = new StreamReader(pizzaTypeSchema.OpenReadStream()))
            using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
            {
                pizzaTypeRecords = csv.GetRecords<PizzaTypeImportModel>().ToList();
            }

            return pizzaTypeRecords;
        }

        private List<PizzaImportModel> ProcessPizzaCsv(IFormFile pizzaSchema)
        {
            var pizzaRecords = new List<PizzaImportModel>();

            using (var reader = new StreamReader(pizzaSchema.OpenReadStream()))
            using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
            {
                pizzaRecords = csv.GetRecords<PizzaImportModel>().ToList();
            }

            return pizzaRecords;
        }

        private List<Pizzas> MergeRecords(List<PizzaTypeImportModel> pizzaTypeRecords, List<PizzaImportModel> pizzaRecords)
        {
            var mergedRecords = new List<Pizzas>();

            foreach (var pizzaRecord in pizzaRecords)
            {
                var matchingTypeRecord = pizzaTypeRecords.FirstOrDefault(p => p.pizza_type_id == pizzaRecord.pizza_type_id);
                if (matchingTypeRecord != null)
                {
                    var mergedRecord = new Pizzas
                    {
                        PizzaID = pizzaRecord.pizza_id,
                        Name = matchingTypeRecord.name,
                        Category = matchingTypeRecord.category,
                        Ingredients = matchingTypeRecord.ingredients,
                        Size = pizzaRecord.size,
                        Price = pizzaRecord.price
                    };
                    mergedRecords.Add(mergedRecord);
                }
            }

            return mergedRecords;
        }

        private void SaveInChunks(List<Pizzas> mergedRecords)
        {
            _pizzaService.SaveInChunks(mergedRecords);
        }
    }
}
