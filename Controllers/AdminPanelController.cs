using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;
using Task = MvcMovie.Models.Task;
//using Task = MvcMovie.Models.Task;

namespace MvcMovie.Controllers
{
    public class AdminPanelController : Controller
    {
        private readonly MvcMovieContext _context;

        public AdminPanelController(MvcMovieContext context)
        {
            _context = context;
        }



        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1)
        {

            // Broj usera po stranici
            int pageSize = 7;
            var users = _context.User.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.Username.ToLower().Contains(searchString.ToLower()));
            }

            var paginatedUsers = await PaginatedList<User>.CreateAsync(users, pageNumber, pageSize);
            return View(paginatedUsers);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserTasks(int id)
        {
            var user = await _context.User
                .Include(u => u.Tasks)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTask(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.TodoTasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.TodoTasks.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _context.User.FindAsync(id);
            if (user == null)
                return NotFound();

            ViewBag.RoleId = new SelectList(_context.Role, "Id", "Name", user.RoleId);
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.RoleId = new SelectList(_context.Role, "Id", "Name", user.RoleId);
            return View(user);
        }


        public IActionResult EditTask(int id)
        {
            return RedirectToAction("Edit", "Task", new { id = id });
        }
    }
}
