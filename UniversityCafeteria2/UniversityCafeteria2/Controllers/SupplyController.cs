
using Microsoft.AspNetCore.Mvc;

namespace UniversityCafeteria.Controllers
{
    public class SupplyController : Controller
    {

        public IActionResult Index()
        {
            string[] categories =
            {
        "Drinks",
        "Food",
        "Snacks",
        "Hot Drinks",
        "Cold Drinks"
    };

            ViewBag.Categories = categories;

            return View();
        }



        public IActionResult Products()
        {
            ViewData["Title"] = "Cafeteria Products";

            ViewData["Manager"] = "Ahmad Ali";

            ViewData["TotalProducts"] = 5;

            string[] products =
            {
        "Water",
        "Orange Juice",
        "Coffee",
        "Sandwich",
        "Chocolate"
    };

            ViewData["Products"] = products;

            return View();
        }



        public IActionResult Suppliers()
        {
            ViewBag.Title = "Our Suppliers";

            ViewBag.TotalSuppliers = 3;

            string[] suppliers =
            {
        "Fresh Food Company",
        "Happy Drinks Company",
        "Daily Supplies Company"
    };

            ViewBag.Suppliers = suppliers;

            return View();
        }


        [HttpGet]
        public IActionResult CreateRequest()
        {
            return View();
        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitRequest()
        {
            string employeeName = Request.Form["EmployeeName"].ToString();

            string productName = Request.Form["ProductName"].ToString();

            string quantityText = Request.Form["Quantity"].ToString();

            string requiredDateText = Request.Form["RequiredDate"].ToString();

            if (string.IsNullOrWhiteSpace(employeeName))
            {
                ViewBag.Message = "Employee Name is required!";

                return View("CreateRequest");
            }

            if (!int.TryParse(quantityText, out int quantity) || quantity < 1)
            {
                ViewBag.Message = "Please enter a valid quantity.";

                return View("CreateRequest");
            }

            if (!DateTime.TryParse(requiredDateText, out DateTime requiredDate))
            {
                ViewBag.Message = "Please select a valid date.";

                return View("CreateRequest");
            }


            TempData["Message"] = "Request submitted successfully!";

            return RedirectToAction("About");

        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult CheckQuantity(int quantity)
        {
            if (quantity < 20)
            {
                ViewBag.Message = "Small Order";
            }
            else if (quantity <= 50)
            {
                ViewBag.Message = "Medium Order";
            }
            else
            {
                ViewBag.Message = "Large Order";
            }

            ViewBag.Quantity = quantity;

            return View();
        }

    }
}
