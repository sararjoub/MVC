using Microsoft.AspNetCore.Mvc;
using UniversityCafeteriaDay6.Data;
using UniversityCafeteriaDay6.Models;

namespace UniversityCafeteriaDay6.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            var categories = _context.Categories.ToList();
            ViewBag.Products = _context.Products.ToList(); // هون منبعث قائمة المنتجات إلى الـفيو حتى نقدر نحسب عدد المنتجات التابعه لكل كاتجوري 


            return View(categories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)   // Category category: استقبال بيانات الفورم باستخدام Model Binding.
        {
            _context.Categories.Add(category);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id) // بستقبل  ال id لل catrgory يلي رح نعدله
        {
            var category = _context.Categories.Find(id); // نبحث عن التصنيف جوا الداتا بيز 

            if (category == null)
                return NotFound();

            return View(category); // هون ارسلت بيانات التصنيف الى صفحه ال edit 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            _context.Categories.Update(category); // منجهز البيانات المعدله عشان نحفظم 

            _context.SaveChanges(); // منحفظ التعديلات بال sql server 

            return RedirectToAction("Index"); // منرجع لصفحه عرض الكاتجوري الاصليه 
        }
        public IActionResult Details(int id)
        {
            var category = _context.Categories.Find(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);

            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
    }
