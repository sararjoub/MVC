using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_8.Data;
using Task_8.Models;

namespace Task_8.Controllers
{
    [Authorize]  // معناه لازم المستخدم يكون مسجل دخول حتى يفتح صفحات هذا الـ Controller.

    public class ProductController : Controller
    {
       
            private readonly ApplicationDbContext _context;

            public ProductController(ApplicationDbContext context)
            {
                _context = context;
            }

            public IActionResult Index()
            {
                var products = _context.Products.ToList();

                return View(products);
            }

            [Authorize(Roles = "Admin,SuperAdmin")]   // معناه الـ Action مسموح للدورين Admin أو SuperAdmin فقط.
        [HttpGet]
            public IActionResult Create()
            {
                return View();
            }

            [Authorize(Roles = "Admin,SuperAdmin")]
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Create(Product product)
            {
                if (ModelState.IsValid)   // إذا البيانات صحيحة، بنحفظ المنتج في SQL Server وبنرجع لصفحة Index.
            {
                    _context.Products.Add(product);
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }

                return View(product);
            }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Update(product);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(product);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }



    }
}
