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

    public class OrderDetailsUploadDto
    {
        public required IFormFile orderDetailsCSVFile { get; set; }
    }
    [ApiController]
    [Route("apis/[controller]")]
    public class OrderDetailsController : Controller
    {
        private readonly OrderDetailsServices _orderDetailsService;

        public OrderDetailsController(OrderDetailsServices orderDetailsService)
        {
            _orderDetailsService = orderDetailsService;
        }

        // GET: OrderDetailsController/GetAll
        [HttpGet("{limit}")]
        public async Task<IActionResult> GetAllOrderDetails(int limit)
        {
            var orders = await _orderDetailsService.GetAllOrderDetailsAsync(limit);
            return Ok(orders);
        }

        // GET: OrderDetailsController/GetByID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetailById(int id)
        {
            var order = await _orderDetailsService.GetOrderDetailByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        // POST: OrderDetailsController/Add
        [HttpPost]
        public async Task<IActionResult> AddOrderDetails([FromBody] OrderDetails orders)
        {
            await _orderDetailsService.AddOrderDetailsAsync(orders);
            return CreatedAtAction(nameof(GetOrderDetailById), new { id = orders.OrderID }, orders);
        }

        // PUT: OrderDetailsController/Update
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderDetails(int id, [FromBody] OrderDetails orders)
        {
            if (id != orders.OrderID)
            {
                return BadRequest();
            }
            await _orderDetailsService.UpdateOrderDetailsAsync(orders);
            return NoContent();
        }

        // POST: OrderDetailsController/Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetails(int id)
        {
            await _orderDetailsService.DeleteOrderDetailsAsync(id);
            return NoContent();
        }

        // POST: OrderDetailsController/Import
        [HttpPost("import")]
        public async Task<IActionResult> ImportCsv(OrderDetailsUploadDto ordersDetailsCSV)
        {
            if (ordersDetailsCSV.orderDetailsCSVFile == null || ordersDetailsCSV.orderDetailsCSVFile.Length == 0)
            {
                return BadRequest("CSV file is required.");
            }

            // Process the OrderType CSV file
            var ordersDetailsRecords = ProcessOrdersCsv(ordersDetailsCSV.orderDetailsCSVFile);

            // Save the merged records in chunks
            await SaveInChunksAsync(ordersDetailsRecords);

            return Ok("CSV files imported successfully.");
        }

        private List<OrderDetails> ProcessOrdersCsv(IFormFile toProcessOrdersDetailsCSV)
        {
            var processedOrdersDetailsRecords = new List<OrderDetailsImportModel>();

            using (var reader = new StreamReader(toProcessOrdersDetailsCSV.OpenReadStream()))
            using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
            {
                processedOrdersDetailsRecords = csv.GetRecords<OrderDetailsImportModel>().ToList();
            }

            var newRecord = new List<OrderDetails>();

            foreach (var orderDetailsRecord in processedOrdersDetailsRecords)
            {

                var order = new OrderDetails
                {
                    OrderDetailID = orderDetailsRecord.order_details_id,
                    OrderID = orderDetailsRecord.order_id,
                    PizzaID = orderDetailsRecord.pizza_id,
                    Quantity = orderDetailsRecord.quantity
                };
                newRecord.Add(order);
            }
 
            return newRecord;
        }


        private async Task SaveInChunksAsync(List<OrderDetails> processedRecords)
        {
            await _orderDetailsService.SaveInChunksAsync(processedRecords);
        }
    }
}
