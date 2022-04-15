using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CRUDelicious.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDelicious.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MyContext _context;
        public HomeController(ILogger<HomeController> logger, MyContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("home")]
        public IActionResult Index()
        {
            ViewBag.AllDishes = _context.Dishes.OrderByDescending(n => n.CreatedAt).ToList();
            return View();
        }

        [HttpGet("dishes/add")]
        public IActionResult Form()
        {
            return View("Form");
        }

        [HttpPost("dishes/add")]
        public IActionResult AddDish(Dish newDish)
        {
            if(ModelState.IsValid)
            {
                _context.Add(newDish);
                _context.SaveChanges();
                return RedirectToAction("Index");
            } else {
                return View("Form");
            }
        }

        [HttpGet("dishes/view/{DishID}")]
        public IActionResult ViewDish(int DishID)
        {
            Dish ViewDish = _context.Dishes.FirstOrDefault(a => a.DishID == DishID);
            return View(ViewDish);
        }

        [HttpGet("dish/edit/{DishID}")]
        public IActionResult EditDish(int DishID)
        {
            Dish UpdateDish = _context.Dishes.FirstOrDefault(a => a.DishID == DishID);
            return View("EditDish", UpdateDish);
        }

        [HttpPost("dishes/update/{DishID}")]
        public IActionResult UpdateDish(int DishID, Dish updatedDish)
        {
            if(ModelState.IsValid)
            {
                Dish OldDish = _context.Dishes.FirstOrDefault(a => a.DishID == DishID);
                OldDish.DishName = updatedDish.DishName;
                OldDish.ChefName = updatedDish.ChefName;
                OldDish.Calories = updatedDish.Calories;
                OldDish.Tastiness = updatedDish.Tastiness;
                OldDish.Description = updatedDish.Description;
                OldDish.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
                return RedirectToAction("Index");
            } else {
                return View("EditDish", updatedDish);
            }
        }

        [HttpGet("dish/remove/{DishID}")]
        public IActionResult RemoveDish(int DishID)
        {
            Dish RemoveDish = _context.Dishes.SingleOrDefault(a => a.DishID == DishID);
            _context.Dishes.Remove(RemoveDish);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
