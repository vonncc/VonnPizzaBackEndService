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

    public class OrderUploadDto
    {
        public required IFormFile orderCSVFile { get; set; }
    }

    [ApiController]
    [Route("apis/[controller]")]
    public class OrdersController : Controller
    {
        private readonly OrdersServices _ordersService;

        public OrdersController(OrdersServices ordersService)
        {
            _ordersService = ordersService;
        }

        // GET: OrdersController/GetAll
        [HttpGet("GetAll/{limit}")]
        public async Task<IActionResult> GetAllOrders(int limit)
        {
            var orders = await _ordersService.GetAllOrdersAsync(limit);
            return Ok(orders);
        }

        // GET: OrdersController/GetByID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _ordersService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        // POST: OrdersController/Add
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] Orders orders)
        {
            await _ordersService.AddOrderAsync(orders);
            return CreatedAtAction(nameof(GetOrderById), new { id = orders.OrderID }, orders);
        }

        // PUT: OrdersController/Update
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Orders orders)
        {
            if (id != orders.OrderID)
            {
                return BadRequest();
            }
            await _ordersService.UpdateOrderAsync(orders);
            return NoContent();
        }

        // POST: OrdersController/Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _ordersService.DeleteOrderAsync(id);
            return NoContent();
        }

        // POST: OrdersController/Import
        [HttpPost("import")]
        public async Task<IActionResult> ImportCsv(OrderUploadDto ordersCSV)
        {
            if (ordersCSV.orderCSVFile == null || ordersCSV.orderCSVFile.Length == 0)
            {
                return BadRequest("CSV file is required.");
            }

            // Process the OrderType CSV file
            var ordersRecords = ProcessOrdersCsv(ordersCSV.orderCSVFile);

            // Save the merged records in chunks
            await _ordersService.SaveInChunksAsync(ordersRecords);

            return Ok("CSV files imported successfully.");
        }

        private List<Orders> ProcessOrdersCsv(IFormFile toProcessOrdersCSV)
        {
            var processedOrdersRecords = new List<OrdersImportModel>();

            using (var reader = new StreamReader(toProcessOrdersCSV.OpenReadStream()))
            using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
            {
                processedOrdersRecords = csv.GetRecords<OrdersImportModel>().ToList();
            }

            var newRecord = new List<Orders>();

            foreach (var orderRecord in processedOrdersRecords)
            {
                if (DateTime.TryParseExact(orderRecord.date + " " + orderRecord.time, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime combinedDateTime))
                {
                    var order = new Orders
                    {
                        OrderID = orderRecord.order_id,
                        OrderDateTime = combinedDateTime
                    };
                    newRecord.Add(order);
                }
                else
                {
                    // Handle parsing failure (e.g., log error, skip record, etc.)
                }
            }
            return newRecord;
        }


        private async Task SaveInChunks(List<Orders> processedRecords)
        {
            await _ordersService.SaveInChunksAsync(processedRecords);
        }
    }
}
