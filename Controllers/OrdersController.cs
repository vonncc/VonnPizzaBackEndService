using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VonnPizzaBackEndService.Controllers
{
    [ApiController]
    [Route("apis/[controller]")]
    public class OrdersController : Controller
    {
        // GET: OrdersController/GetAll
        [HttpGet]
        public ActionResult GetAll()
        {
            // Retrieve sale data by ID
            return Ok();
        }


        // GET: OrdersController/GetID
        [HttpGet("{id}")]
        public ActionResult GetWithID(int id)
        {
            return Ok();
        }


        // POST: OrdersController/Create
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


        // POST: OrdersController/Edit
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

        // POST: OrdersController/Delete
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
