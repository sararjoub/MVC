using Microsoft.AspNetCore.Mvc;
using UniversityCafeteriaDay6.Data;
using UniversityCafeteriaDay6.Models;

namespace UniversityCafeteriaDay6.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupplierController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var supplier = _context.Suppliers.ToList();
            return View(supplier);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}