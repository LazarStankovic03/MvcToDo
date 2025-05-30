using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

namespace MvcMovie.Data
{
    public class MvcMovieContext : DbContext
    {
        public MvcMovieContext (DbContextOptions<MvcMovieContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var roles = Enum.GetValues(typeof(Roles))
                .Cast<Roles>()
                .Select(e => new Role
                {
                    Id = (int)e,
                    Name = e.ToString()
                });
            modelBuilder.Entity<Role>().HasData(roles);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);
        }



        public DbSet<Models.Task> TodoTasks { get; set; } = default!;

        public DbSet<User> User { get; set; } = default!;
        public DbSet<Role> Role { get; set; }

    }
}
