using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doggo.Models;
using doggo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace doggo.Controllers
{
    public class DogController : Controller
    {
        // GET: DogController
        private DogRepository _dogRepository;
        public DogController(IConfiguration conifg)
        {
            _dogRepository = new DogRepository(conifg);
        }
        public ActionResult Index()
        {
            List<Dog> dogs = _dogRepository.GetAllDogs();
            return View(dogs);
        }

        // GET: DogController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DogController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Dog dog)
        {
            try
            {
                _dogRepository.Add(dog);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(dog);
            }
        }

        // GET: DogController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DogController/Edit/5
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

        // GET: DogController/Delete/5
        public ActionResult Delete(int id)
        {
            Dog dog = _dogRepository.GetDogById(id);

            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }

            // POST: DogController/Delete/5
            [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Dog dog)
        {
            try
            {
                _dogRepository.Remove(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(dog);
            }
        }
    }
}
