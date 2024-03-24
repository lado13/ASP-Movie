using ASP_MVC_Movie.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASP_MVC_Movie.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<MovieGenre>()
        //        .HasKey(mg => new { mg.MovieId, mg.GenreId });

        //    modelBuilder.Entity<MovieGenre>()
        //        .HasOne(mg => mg.Movie)
        //        .WithMany(m => m.MovieGenres)
        //        .HasForeignKey(mg => mg.MovieId);

        //    modelBuilder.Entity<MovieGenre>()
        //        .HasOne(mg => mg.Genre)
        //        .WithMany(g => g.MovieGenres)
        //        .HasForeignKey(mg => mg.GenreId);
        //}



        public static async Task EnsureRolesCreated(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }







    }
}
