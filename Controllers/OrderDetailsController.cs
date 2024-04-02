using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VonnPizzaBackEndService.Controllers
{
    [ApiController]
    [Route("apis/[controller]")]
    public class OrderDetailsController : Controller
    {
        // GET: OrdersDetailsController/GetAll
        [HttpGet]
        public ActionResult GetAll()
        {
            // Retrieve sale data by ID
            return Ok();
        }


        // GET: OrdersDetailsController/GetID
        [HttpGet("{id}")]
        public ActionResult GetWithID(int id)
        {
            return Ok();
        }


        // POST: OrdersDetailsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // POST: OrdersDetailsController/Edit
        [HttpPut]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: OrdersDetailsController/Delete
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
