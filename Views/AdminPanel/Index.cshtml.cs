using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;

namespace MvcMovie.Views.AdminPanel
{
    public class IndexModel : PageModel
    {
        private readonly MvcMovie.Data.MvcMovieContext _context;

        public IndexModel(MvcMovie.Data.MvcMovieContext context)
        {
            _context = context;
        }

        public IList<User> User { get;set; } = default!;

        //public async Task OnGetAsync()
        //{
        //    User = await _context.User
        //        .Include(u => u.Role).ToListAsync();
        //}
    }
}
