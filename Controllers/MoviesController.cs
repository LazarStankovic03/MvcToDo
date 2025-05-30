using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MvcMovieContext _context;

        public MoviesController(MvcMovieContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            return View(await _context.TodoTasks.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price")] Models.Task task)
        {
            if (ModelState.IsValid)
            {
                _context.Add(task);
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

            var task = _context.TodoTasks.FindAsync(id);

            if(task == null)
            {
                return NotFound();
            }

            return View(task);
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

        private bool MovieExists(int id)
        {
            return _context.TodoTasks.Any(e => e.Id == id);
        }
    }
}
