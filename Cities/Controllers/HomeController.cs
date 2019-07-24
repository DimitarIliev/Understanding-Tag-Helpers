using Cities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cities.Controllers
{
    public class HomeController : Controller
    {
        private IRepository repository;

        public HomeController(IRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index() => View(repository.Cities);

        public ViewResult Edit()
        {
            ViewBag.Countries = new SelectList(repository.Cities.Select(x => x.Country).Distinct());
            return View("Create", repository.Cities.FirstOrDefault());
        }
        

        public ViewResult Create()
        {
            ViewBag.Countries = new SelectList(repository.Cities.Select(x => x.Country).Distinct());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(City city)
        {
            repository.AddCity(city);
            return RedirectToAction("Index");
        }
    }
}
