using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class TaskController : Controller
    {
        private readonly MvcMovieContext _context;

        public TaskController(MvcMovieContext context)
        {
            _context = context;
        }

        // GET: Movies


        [Authorize] 
        public async Task<IActionResult> Index()
        {
            //var userIdClaim = User.FindFirst("UserId")?.Value;
            //if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            //{
            //    return Unauthorized();
            //}

            var userId = 1;
            var tasks = await _context.TodoTasks
                .Where(t => t.UserId == userId)
                .ToListAsync();
            return View(tasks);        
        }
        public async Task<IActionResult> Drag()
        {
            return View(await _context.TodoTasks.ToListAsync());
        }


        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.Task task)
        {
            if (ModelState.IsValid)
            {
                _context.TodoTasks.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }



        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var task = _context.TodoTasks.FirstOrDefaultAsync(m => m.Id == id);

            if(task == null)
            {
                return NotFound();
            }

            return View(task);

        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var task = _context.TodoTasks.FindAsync(id);
            if(task != null)
            {
                _context.Remove(task);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var task = await _context.TodoTasks.FindAsync(id);

            if(task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Models.Task updatedTask)
        {
            if (id != updatedTask.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                var existingTask = await _context.TodoTasks.FindAsync(id);
                if (existingTask == null)
                    return NotFound();

                existingTask.Title = updatedTask.Title;
                existingTask.DueDate = updatedTask.DueDate;
                existingTask.Priority = updatedTask.Priority;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(updatedTask);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateData(int id, string newStatus)
        {
            var task = await _context.TodoTasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            if (Enum.TryParse<MovieStatus>(newStatus, out var parsedStatus))
            {
                task.Status = parsedStatus;
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest("Nevazeci status.");
        }

    }
}
