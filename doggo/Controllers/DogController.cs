using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using doggo.Models;
using doggo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace doggo.Controllers
{
    [Authorize]
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
            int ownerId = GetCurrentUserId();
            List<Dog> dogs = _dogRepository.GetDogsByOwnerId(ownerId);
            return View(dogs);
        }

        // GET: DogController/Details/5
        public ActionResult Details(int id)
        {
            Dog dog = _dogRepository.GetDogById(id);
            int ownerId = GetCurrentUserId();
            if(dog.OwnerId != ownerId)
            {
                return NotFound();
            }
            else
            {
                return View(dog);
            }
        }

        // GET: DogController/Create
        public ActionResult Create(int id)
        {
            Dog dog = _dogRepository.GetDogById(id);
            return View(dog);
        }

        // POST: DogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Dog dog)
        {
            try
            {
                // update the dogs OwnerId to the current user's Id 
                dog.OwnerId = GetCurrentUserId();

                _dogRepository.Add(dog);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }

        // GET: DogController/Edit/5
        public ActionResult Edit(int id)
        {
            Dog dog = _dogRepository.GetDogById(id);

            if (dog.OwnerId != GetCurrentUserId() || dog == null)
            {
                return NotFound();
            }
            else
            {
                return View(dog);

            }

        }

        // POST: DogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Dog dog)
        {
            try
            {
              
                _dogRepository.UpdateDog(dog);

                return RedirectToAction("Index");
                
            }
            catch
            {
                return View(dog);
            }
        }

        // GET: DogController/Delete/5
        public ActionResult Delete(int id)
        {
            Dog dog = _dogRepository.GetDogById(id);
            int ownerId = GetCurrentUserId();
            if (dog == null || dog.OwnerId != ownerId)
            {
                return NotFound();
            }
            else
            {
                return View(dog);
            }

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
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
