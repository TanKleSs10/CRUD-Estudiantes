using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Estudiantes.Controllers
{
    public class MateriaController : Controller
    {
        // GET: ClaseController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ClaseController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClaseController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClaseController/Create
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

        // GET: ClaseController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ClaseController/Edit/5
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

        // GET: ClaseController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ClaseController/Delete/5
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
