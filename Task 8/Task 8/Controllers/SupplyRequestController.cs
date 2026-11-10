using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task_8.Data;
using Task_8.Models;

namespace Task_8.Controllers
{
    [Authorize]
    public class SupplyRequestController : Controller
    {
       
            private readonly ApplicationDbContext _context;
            private readonly UserManager<ApplicationUser> _userManager;

            public SupplyRequestController(
                ApplicationDbContext context,
                UserManager<ApplicationUser> userManager)
            {
                _context = context;
                _userManager = userManager;
            }

            public IActionResult Index()
            {
                var requests = _context.SupplyRequests
                    .AsQueryable();

                if (!User.IsInRole("Admin") &&
                    !User.IsInRole("SuperAdmin"))
                {
                    string? userId = _userManager.GetUserId(User);

                    requests = requests.Where(r => r.UserId == userId);
                }

                return View(requests.ToList());
            }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Products = _context.Products.ToList();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int[] ProductIds, int[] Quantities)
        {
            if (ProductIds.Length != 3 || Quantities.Length != 3)
                return BadRequest();

            var request = new SupplyRequest
            {
                RequestDate = DateTime.Now,
                Status = "Pending",
                UserId = _userManager.GetUserId(User)!
            };

            for (int i = 0; i < ProductIds.Length; i++)
            {
                if (ProductIds[i] == 0 && Quantities[i] == 0)
                    continue;

                var product = _context.Products.Find(ProductIds[i]);

                if (product == null || Quantities[i] <= 0)
                    return BadRequest();

                if (Quantities[i] > product.Quantity)
                    return BadRequest("Not enough stock.");

                if (ProductIds.Count(id => id == ProductIds[i]) > 1)
                    return BadRequest("Select each product only once.");

                var item = new RequestItem
                {
                    ProductId = product.Id,
                    Quantity = Quantities[i],
                    Price = product.Price
                };

                request.RequestItems.Add(item);

                request.TotalPrice += product.Price * Quantities[i];
            }

            if (request.RequestItems.Count == 0)
                return BadRequest("Select at least one product.");

            _context.SupplyRequests.Add(request);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Details(int id)
        {
            var request = _context.SupplyRequests
                .Include(r => r.RequestItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(r => r.Id == id);

            if (request == null)
                return NotFound();

            if (!User.IsInRole("Admin") &&
                !User.IsInRole("SuperAdmin") &&
                request.UserId != _userManager.GetUserId(User))
            {
                return Forbid();
            }

            return View(request);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var request = _context.SupplyRequests
                .Include(r => r.RequestItems)
                .FirstOrDefault(r => r.Id == id);

            if (request == null)
                return NotFound();

            bool isAdmin = User.IsInRole("Admin") ||
                           User.IsInRole("SuperAdmin");

            if (!isAdmin && request.UserId != _userManager.GetUserId(User))
                return Forbid();

            if (!isAdmin && request.Status != "Pending")
                return Forbid();

            return View(request);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, int[] ItemIds,
            int[] Quantities, string? Status)
        {
            var request = _context.SupplyRequests
                .Include(r => r.RequestItems)
                .FirstOrDefault(r => r.Id == id);

            if (request == null)
                return NotFound();

            bool isAdmin = User.IsInRole("Admin") ||
                           User.IsInRole("SuperAdmin");

            if (isAdmin)
            {
                if (Status != "Pending" &&
                    Status != "Approved" &&
                    Status != "Rejected")
                    return BadRequest();

                request.Status = Status;
            }
            else
            {
                if (request.UserId != _userManager.GetUserId(User))
                    return Forbid();

                if (request.Status != "Pending")
                    return Forbid();

                var items = request.RequestItems.OrderBy(i => i.Id).ToList();

                if (ItemIds.Length != items.Count ||
                    Quantities.Length != items.Count)
                    return BadRequest();

                for (int i = 0; i < items.Count; i++)
                {
                    if (ItemIds[i] != items[i].Id || Quantities[i] <= 0)
                        return BadRequest();

                    var product = _context.Products.Find(items[i].ProductId);

                    if (product == null || Quantities[i] > product.Quantity)
                        return BadRequest("Invalid quantity.");

                    items[i].Quantity = Quantities[i];
                }

                request.TotalPrice = items.Sum(i => i.Price * i.Quantity);
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var request = _context.SupplyRequests
                .Include(r => r.RequestItems)
                .FirstOrDefault(r => r.Id == id);

            if (request == null)
                return NotFound();

            bool isAdmin = User.IsInRole("Admin") ||
                           User.IsInRole("SuperAdmin");

            if (!isAdmin && request.UserId != _userManager.GetUserId(User))
                return Forbid();

            if (!isAdmin && request.Status != "Pending")
                return Forbid();

            _context.RequestItems.RemoveRange(request.RequestItems);
            _context.SupplyRequests.Remove(request);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}