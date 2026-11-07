
using GraduationSupplyManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace GraduationSupplyManagement.Controllers
{
    public class SupplyController : Controller
    {
        // Display all offers
        public IActionResult Index()
        {
            List<SupplyOffer> offers = GetOffers();

            return View(offers);
        }

        // Save selected offers in Session
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectOffers(
            int? certificateId,
            int? hospitalityId,
            int? giftId)
        {
            if (certificateId == null ||
                hospitalityId == null ||
                giftId == null)
            {
                TempData["Message"] = "Please select one offer from each category.";
                return RedirectToAction("Index");
            }

            if (certificateId < 1 || certificateId > 3 ||
                hospitalityId < 4 || hospitalityId > 6 ||
                giftId < 7 || giftId > 9)
            {
                TempData["Message"] = "Invalid offer selection.";
                return RedirectToAction("Index");
            }

            HttpContext.Session.SetInt32("CertificateOffer", certificateId.Value);
            HttpContext.Session.SetInt32("HospitalityOffer", hospitalityId.Value);
            HttpContext.Session.SetInt32("GiftOffer", giftId.Value);

            return RedirectToAction("Review");
        }

        // Display selected offers and calculate totals
        [HttpGet]
        public IActionResult Review()
        {
            int? certificateId = HttpContext.Session.GetInt32("CertificateOffer");
            int? hospitalityId = HttpContext.Session.GetInt32("HospitalityOffer");
            int? giftId = HttpContext.Session.GetInt32("GiftOffer");

            if (certificateId == null ||
                  hospitalityId == null ||
                   giftId == null)
            {
                TempData["Message"] = "Please select one offer from each category.";

                return RedirectToAction("Index");
            }

            List<SupplyOffer> offers = GetOffers();
            List<SupplyOffer> selectedOffers = new List<SupplyOffer>();

            foreach (var offer in offers)
            {
                if (offer.Id == certificateId ||
                    offer.Id == hospitalityId ||
                    offer.Id == giftId)
                {
                    selectedOffers.Add(offer);
                }
            }

            decimal total = 0;
            List<decimal> offerTotals = new List<decimal>();

            foreach (var offer in selectedOffers)
            {
                decimal offerTotal = offer.Quantity * offer.UnitPrice;

                offerTotals.Add(offerTotal);
                total += offerTotal;
            }

            decimal discount = 0;

            if (total > 500)
            {
                discount = 0.15m;
            }
            else if (total > 300)
            {
                discount = 0.10m;
            }

            decimal discountAmount = total * discount;
            decimal finalTotal = total - discountAmount;

            ViewBag.OfferTotals = offerTotals;
            ViewBag.Total = total;
            ViewBag.Discount = discount * 100;
            ViewBag.DiscountAmount = discountAmount;
            ViewBag.FinalTotal = finalTotal;

            return View(selectedOffers);
        }

        // List of all available offers
        private List<SupplyOffer> GetOffers()
        {
            List<SupplyOffer> offers = new List<SupplyOffer>
            {
                new SupplyOffer
                {
                    Id = 1,
                    Category = "Graduation Certificates",
                    SupplierName = "Supplier A",
                    OfferName = "Premium Certificate Package",
                    Quantity = 35,
                    UnitPrice = 2m
                },

                new SupplyOffer
                {
                    Id = 2,
                    Category = "Graduation Certificates",
                    SupplierName = "Supplier B",
                    OfferName = "Standard Certificate Package",
                    Quantity = 35,
                    UnitPrice = 1.5m
                },

                new SupplyOffer
                {
                    Id = 3,
                    Category = "Graduation Certificates",
                    SupplierName = "Supplier C",
                    OfferName = "Luxury Certificate Package",
                    Quantity = 35,
                    UnitPrice = 2.5m
                },

                new SupplyOffer
                {
                    Id = 4,
                    Category = "Hospitality",
                    SupplierName = "Supplier D",
                    OfferName = "Coffee & Drinks Package",
                    Quantity = 100,
                    UnitPrice = 2m
                },

                new SupplyOffer
                {
                    Id = 5,
                    Category = "Hospitality",
                    SupplierName = "Supplier E",
                    OfferName = "Sandwiches & Snacks Package",
                    Quantity = 100,
                    UnitPrice = 3m
                },

                new SupplyOffer
                {
                    Id = 6,
                    Category = "Hospitality",
                    SupplierName = "Supplier F",
                    OfferName = "Full Hospitality Package",
                    Quantity = 100,
                    UnitPrice = 4m
                },

                new SupplyOffer
                {
                    Id = 7,
                    Category = "Graduation Gifts",
                    SupplierName = "Supplier G",
                    OfferName = "Customized Mugs Package",
                    Quantity = 35,
                    UnitPrice = 2m
                },

                new SupplyOffer
                {
                    Id = 8,
                    Category = "Graduation Gifts",
                    SupplierName = "Supplier H",
                    OfferName = "Graduation Bags Package",
                    Quantity = 35,
                    UnitPrice = 3m
                },

                new SupplyOffer
                {
                    Id = 9,
                    Category = "Graduation Gifts",
                    SupplierName = "Supplier I",
                    OfferName = "Premium Gift Boxes Package",
                    Quantity = 35,
                    UnitPrice = 4m
                }
            };

            return offers;
        }

        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

    }
}
