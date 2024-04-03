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
        [HttpGet]
        public IActionResult GetAllOrderDetails()
        {
            var orders = _orderDetailsService.GetAllOrderDetails();
            return Ok(orders);
        }

        // GET: OrderDetailsController/GetByID
        [HttpGet("{id}")]
        public IActionResult GetOrderDetailById(int id)
        {
            var order = _orderDetailsService.GetOrderDetailById(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        // POST: OrderDetailsController/Add
        [HttpPost]
        public IActionResult AddOrderDetails([FromBody] OrderDetails orders)
        {
            _orderDetailsService.AddOrderDetails(orders);
            return CreatedAtAction(nameof(GetOrderDetailById), new { id = orders.OrderID }, orders);
        }

        // PUT: OrderDetailsController/Update
        [HttpPut("{id}")]
        public IActionResult UpdateOrderDetails(int id, [FromBody] OrderDetails orders)
        {
            if (id != orders.OrderID)
            {
                return BadRequest();
            }
            _orderDetailsService.UpdateOrderDetails(orders);
            return NoContent();
        }

        // POST: OrderDetailsController/Delete
        [HttpDelete("{id}")]
        public IActionResult DeleteOrderDetails(int id)
        {
            _orderDetailsService.DeleteOrderDetails(id);
            return NoContent();
        }

        // POST: OrderDetailsController/Import
        [HttpPost("import")]
        public IActionResult ImportCsv(OrderDetailsUploadDto ordersDetailsCSV)
        {
            if (ordersDetailsCSV.orderDetailsCSVFile == null || ordersDetailsCSV.orderDetailsCSVFile.Length == 0)
            {
                return BadRequest("CSV file is required.");
            }

            // Process the OrderType CSV file
            var ordersDetailsRecords = ProcessOrdersCsv(ordersDetailsCSV.orderDetailsCSVFile);

            // Save the merged records in chunks
            SaveInChunks(ordersDetailsRecords);

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


        private void SaveInChunks(List<OrderDetails> processedRecords)
        {
            _orderDetailsService.SaveInChunks(processedRecords);
        }
    }
}
