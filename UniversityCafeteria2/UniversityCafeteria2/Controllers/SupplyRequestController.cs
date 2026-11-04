
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UniversityCafeteria2.Models;

namespace UniversityCafeteria2.Controllers
{
    public class SupplyRequestController : Controller
    {
        // List of confirmed requests
        private static List<ConfirmedRequest> confirmedRequests =
            new List<ConfirmedRequest>();


        // 1. Create Supply Request
        public IActionResult Index()
        {
            string? savedData =
                HttpContext.Session.GetString("DraftItems");

            List<SupplyItem> items = string.IsNullOrEmpty(savedData)
                ? new List<SupplyItem>()
                : JsonSerializer.Deserialize<List<SupplyItem>>(savedData)
                  ?? new List<SupplyItem>();

            ViewBag.ItemCount = items.Count;

            string? preferredPriority =
                Request.Cookies["PreferredPriority"];

            ViewBag.PreferredPriority =
                preferredPriority == "Normal" ||
                preferredPriority == "Urgent" ||
                preferredPriority == "Emergency"
                    ? preferredPriority
                    : "Normal";

            return View();
        }


        // 2. Add Product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddItem(
            string ProductName,
            string Category,
            int Quantity,
            decimal UnitPrice,
            string Priority)
        {
            // Validate model binding
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Please enter valid data!";
                return RedirectToAction("Index");
            }

            // Validate product name
            if (string.IsNullOrWhiteSpace(ProductName))
            {
                TempData["Message"] = "Product Name is required!";
                return RedirectToAction("Index");
            }

            // Validate category
            if (string.IsNullOrWhiteSpace(Category))
            {
                TempData["Message"] = "Category is required!";
                return RedirectToAction("Index");
            }

            // Validate quantity and price
            if (Quantity <= 0 || UnitPrice <= 0)
            {
                TempData["Message"] =
                    "Quantity and Price must be greater than zero!";

                return RedirectToAction("Index");
            }

            // Validate priority
            if (Priority != "Normal" &&
                Priority != "Urgent" &&
                Priority != "Emergency")
            {
                TempData["Message"] = "Invalid Priority!";
                return RedirectToAction("Index");
            }

            // Read products from Session
            string? savedData =
                HttpContext.Session.GetString("DraftItems");

            List<SupplyItem> items;

            if (string.IsNullOrEmpty(savedData))
            {
                items = new List<SupplyItem>();
            }
            else
            {
                items = JsonSerializer.Deserialize<List<SupplyItem>>(savedData)
                        ?? new List<SupplyItem>();
            }

            // Keep the same priority for the whole request
            if (items.Count > 0)
            {
                Priority = items[0].Priority;
            }

            // Create a new product
            SupplyItem newItem = new SupplyItem
            {
                ProductName = ProductName,
                Category = Category,
                Quantity = Quantity,
                UnitPrice = UnitPrice,
                Priority = Priority
            };

            // Add product to the list
            items.Add(newItem);

            // Save products in Session
            string json = JsonSerializer.Serialize(items);

            HttpContext.Session.SetString("DraftItems", json);

            TempData["Message"] = "Product added successfully!";

            return RedirectToAction("Index");
        }


        // 3. Review Draft
        public IActionResult Review()
        {
            string? savedData =
                HttpContext.Session.GetString("DraftItems");

            List<SupplyItem> items = string.IsNullOrEmpty(savedData)
                ? new List<SupplyItem>()
                : JsonSerializer.Deserialize<List<SupplyItem>>(savedData)
                  ?? new List<SupplyItem>();

            // Calculate subtotal
            decimal subtotal = 0;

            foreach (var item in items)
            {
                subtotal += item.Quantity * item.UnitPrice;
            }

            // Get request priority
            string priority = items.Count > 0
                ? items[0].Priority
                : "Normal";

            // Calculate preparation fee
            decimal preparationFee = 0;

            if (items.Count > 0)
            {
                switch (priority)
                {
                    case "Normal":
                        preparationFee = 2;
                        break;

                    case "Urgent":
                        preparationFee = 5;
                        break;

                    case "Emergency":
                        preparationFee = 10;
                        break;
                }
            }

            // Calculate discount
            decimal discount = 0;

            if (subtotal > 200)
            {
                discount = subtotal * 0.15m;
            }
            else if (subtotal > 100)
            {
                discount = subtotal * 0.10m;
            }

            // Calculate final total
            decimal finalTotal =
                subtotal + preparationFee - discount;

            // Send data to View
            ViewBag.Items = items;
            ViewBag.Priority = priority;
            ViewBag.Subtotal = subtotal;
            ViewBag.PreparationFee = preparationFee;
            ViewBag.Discount = discount;
            ViewBag.FinalTotal = finalTotal;

            return View();
        }


        // 4. Confirm Request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmRequest()
        {
            string? savedData =
                HttpContext.Session.GetString("DraftItems");

            List<SupplyItem> items = string.IsNullOrEmpty(savedData)
                ? new List<SupplyItem>()
                : JsonSerializer.Deserialize<List<SupplyItem>>(savedData)
                  ?? new List<SupplyItem>();

            // Prevent empty requests
            if (items.Count == 0)
            {
                TempData["Message"] =
                    "Cannot confirm an empty request!";

                return RedirectToAction("Review");
            }

            // Calculate subtotal
            decimal subtotal = 0;

            foreach (var item in items)
            {
                subtotal += item.Quantity * item.UnitPrice;
            }

            string priority = items[0].Priority;

            // Calculate preparation fee
            decimal preparationFee = 0;

            switch (priority)
            {
                case "Normal":
                    preparationFee = 2;
                    break;

                case "Urgent":
                    preparationFee = 5;
                    break;

                case "Emergency":
                    preparationFee = 10;
                    break;
            }

            // Calculate discount
            decimal discount = 0;

            if (subtotal > 200)
            {
                discount = subtotal * 0.15m;
            }
            else if (subtotal > 100)
            {
                discount = subtotal * 0.10m;
            }

            // Calculate final total
            decimal finalTotal =
                subtotal + preparationFee - discount;

            // Create confirmed request
            ConfirmedRequest request = new ConfirmedRequest
            {
                NumberOfProducts = items.Count,
                Subtotal = subtotal,
                PreparationFee = preparationFee,
                Discount = discount,
                FinalTotal = finalTotal,
                Status = "Confirmed"
            };

            // Save confirmed request
            lock (confirmedRequests)
            {
                request.RequestId = confirmedRequests.Count + 1;
                confirmedRequests.Add(request);
            }

            // Clear draft after confirmation
            HttpContext.Session.Remove("DraftItems");

            TempData["Message"] =
                "Request confirmed successfully!";

            return RedirectToAction("MyRequests");
        }


        // 5. Clear Draft
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClearDraft()
        {
            HttpContext.Session.Remove("DraftItems");

            TempData["Message"] =
                "Draft cleared successfully!";

            return RedirectToAction("Index");
        }


        // 6. Save Employee Preference
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SavePreference(string PreferredPriority)
        {
            // Validate priority
            if (PreferredPriority != "Normal" &&
                PreferredPriority != "Urgent" &&
                PreferredPriority != "Emergency")
            {
                TempData["Message"] = "Invalid Priority!";
                return RedirectToAction("Index");
            }

            // Cookie expires after 7 days
            CookieOptions options = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(7),
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.Lax,
                Secure = Request.IsHttps
            };

            // Save Cookie
            Response.Cookies.Append(
                "PreferredPriority",
                PreferredPriority,
                options
            );

            TempData["Message"] =
                "Preference saved successfully!";

            return RedirectToAction("Index");
        }


        // 7. My Confirmed Requests
        public IActionResult MyRequests()
        {
            lock (confirmedRequests)
            {
                ViewBag.Requests =
                    new List<ConfirmedRequest>(confirmedRequests);
            }

            return View();
        }


        // 8. About Page
        public IActionResult About()
        {
            return View();
        }
    }
}
