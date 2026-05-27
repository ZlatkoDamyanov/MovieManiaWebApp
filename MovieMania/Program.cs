using Data;
using Microsoft.EntityFrameworkCore;
using Services.Helpers;
using Services.Interfaces;
using Services.Implementations;


namespace MovieMania
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<MovieCatalogDbContext>(options =>
                options.UseSqlite($"Data Source={DbPath.GetPath()}"));
            builder.Services.AddScoped<IMovieService, MovieService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Movies}/{action=Index}");

            app.Run();
        }
    }
}
