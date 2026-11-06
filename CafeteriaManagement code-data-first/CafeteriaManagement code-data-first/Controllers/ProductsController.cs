
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CafeteriaManagement_code_data_first.Data;

namespace CafeteriaManagement_code_data_first.Controllers
{
    public class ProductsController : Controller
    {
        private readonly CafeteriaDbContext _context;

        public ProductsController(CafeteriaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Categ)
                .Include(p => p.Supplier)
                .ToListAsync();

            return View(products);
        }

    }
}
