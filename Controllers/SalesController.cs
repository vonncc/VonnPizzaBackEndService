using Microsoft.AspNetCore.Mvc;

namespace VonnPizzaBackEndService.Controllers
{
    [ApiController]
    [Route("apis/[controller]")]
    public class SalesController : ControllerBase
    {
        

        // GET: apis/Sales/{id}
        [HttpGet("{id}")]
        public ActionResult GetSale(int id)
        {
            // Retrieve sale data by ID
            return Ok();
        }

        // POST: apis/Sales
        

        // DELETE: apis/Sales/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteSale(int id)
        {
            // Delete an existing sale
            return NoContent();
        }
    }
}
