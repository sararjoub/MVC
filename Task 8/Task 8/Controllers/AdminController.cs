
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Task_8.Models;

namespace Task_8.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            ViewBag.Roles = new List<string>
    {
        "User", "Admin", "SuperAdmin"
    };

            var roles = await _userManager.GetRolesAsync(user);

            ViewBag.CurrentRole = roles.FirstOrDefault();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            if (role != "User" && role != "Admin" && role != "SuperAdmin")
                return BadRequest();

            if (user.Id == _userManager.GetUserId(User))
                return BadRequest("You cannot change your own role.");

            var oldRoles = await _userManager.GetRolesAsync(user);

            var removeResult = await _userManager.RemoveFromRolesAsync(user, oldRoles);

            if (!removeResult.Succeeded)
                return BadRequest("Could not remove old roles.");

            var addResult = await _userManager.AddToRoleAsync(user, role);

            if (!addResult.Succeeded)
                return BadRequest("Could not assign the new role.");

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            if (user.Id == _userManager.GetUserId(User))
                return BadRequest("You cannot delete your own account.");

            if (await _userManager.IsInRoleAsync(user, "SuperAdmin"))
                return BadRequest("You cannot delete a SuperAdmin account.");

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                return BadRequest("Could not delete user.");

            return RedirectToAction("Index");
        }

    }
}
