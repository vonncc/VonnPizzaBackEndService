using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VonnPizzaBackEndService.Controllers
{
    [ApiController]
    [Route("apis/[controller]")]
    public class PizzasController : Controller
    {
        // GET: PizzasController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PizzasController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PizzasController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PizzasController/Create
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

        // GET: PizzasController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PizzasController/Edit/5
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

        // GET: PizzasController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PizzasController/Delete/5
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
