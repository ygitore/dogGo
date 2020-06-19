using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doggo.Models;
using doggo.Models.ViewModels;
using doggo.Repositories;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace doggo.Controllers
{
    public class OwnersController : Controller
    {
        // GET: OwnersController
        private readonly OwnerRepository _ownerRepo;
        private readonly DogRepository _dogRepo;
        private readonly WalkerRepository _walkerRepo;
        private readonly NeighborhoodRepository _neighborhoodRepo;
        public OwnersController(IConfiguration config)
        {
            _ownerRepo = new OwnerRepository(config);
            _dogRepo = new DogRepository(config);
            _walkerRepo = new WalkerRepository(config);
            _neighborhoodRepo = new NeighborhoodRepository(config);
        }
        public ActionResult Index()
        {
            List<Owner> owners = _ownerRepo.getAllOwners();
            return View(owners);
        }

        // GET: OwnersController/Details/5
        public ActionResult Details(int id)
        {
            ProfileViewModel vm = new ProfileViewModel();
            vm.Dogs = _dogRepo.GetDogsByOwnerId(id);
            vm.Owner = _ownerRepo.GetOwnerById(id);
            if (vm.Owner == null)
            {
                return NotFound();
            }
            vm.Walkers = _walkerRepo.GetWalkersInNeighborhood(vm.Owner.NeighborhoodId);

            return View(vm);
        }

        // GET: OwnersController/Create
        public ActionResult Create()
        {
            List<Neighborhood> neighborhoods = _neighborhoodRepo.GetAll();

            OwnerFormViewModel vm = new OwnerFormViewModel()
            {
                Owner = new Owner(),
                Neighborhoods = neighborhoods
            };

            return View(vm);
        }

        // POST: OwnersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Owner owner)
        {
            try
            {
                _ownerRepo.AddOwner(owner);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(owner);
            }
        }

        // GET: OwnersController/Edit/5
        public ActionResult Edit(int id)
        {
            List<Neighborhood> neighborhoods = _neighborhoodRepo.GetAll();

            OwnerFormViewModel vm = new OwnerFormViewModel()
            {
                Owner = _ownerRepo.GetOwnerById(id),
                Neighborhoods = neighborhoods
            };

            return View(vm);
        }

        // POST: OwnersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, OwnerFormViewModel vm)
        {
            try
            {
                _ownerRepo.UpdateOwner(vm.Owner);
                return RedirectToAction("Index");                
            }
            catch
            {
                return View(vm);
            }
        }

        // GET: OwnersController/Delete/5
        public ActionResult Delete(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);
            return View(owner);
        }

        // POST: OwnersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Owner owner)
        {
            try
            {
                _ownerRepo.DeleteOwner(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(owner);
            }
        }
    }
}
