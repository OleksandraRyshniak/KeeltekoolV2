using Keeltekool_2.Core.Domain;
using Keeltekool_2.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Keeltekool_2

{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //services
            //apiclients

            //dbcontext
            builder.Services.AddDbContext<Keeltekool_2Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
                options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Password.RequiredLength = 8;
                    options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
                    options.Lockout.MaxFailedAccessAttempts = 3;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                })
                .AddEntityFrameworkStores<Keeltekool_2Context>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("CustomEmailConfirmation");

            //auth


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}