using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VonnPizzaBackEndService.Controllers
{
    [ApiController]
    [Route("apis/[controller]")]
    public class PizzaTypesController : Controller
    {
        // GET: PizzaTypesController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PizzaTypesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PizzaTypesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PizzaTypesController/Create
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

        // GET: PizzaTypesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PizzaTypesController/Edit/5
        [HttpPost]
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

        // GET: PizzaTypesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PizzaTypesController/Delete/5
        [HttpPost]
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
