using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Book_Events.Infrastructure.Context;
using Book_Events.Domain.Interfaces;
using Book_Events.Domain.Services;
using Book_Events.Infrastructure.Interfaces;
using Book_Events.Infrastructure.Repositories;
using Book_Events.Domain.Mappings;
using Book_Events.Infrastructure.Unit_of_Work;
using Book_Events.Plugins.Logging;
using Book_Events.Domain.SignalR;
using Book_Events.Domain.Factory;
using Book_Events.Domain.ExceptionHandler;

namespace Book_Events
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString /*, b => b.MigrationsAssembly("Book_Events")*/));

            builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = false).AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddScoped<IEventRepository, EventRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IEventService, EventServices>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<IAdminService,AdminServices>();
            builder.Services.AddAutoMapper(typeof(IStartup).Assembly, typeof(DomainToDtoMappingProfile).Assembly);
            builder.Services.AddSingleton<ILog, Log>();
            builder.Services.AddScoped<IFacade, Facade>();
            builder.Services.AddScoped<IFacadeFactory<IFacade>,  FacadeFactory<IFacade>>();

            builder.Services.AddSignalR();

            // Add services to the container.
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<CustomExceptionHandlerAttribute>();
            });
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //app.UseMiddleware<AcessControlMiddleware>();

            app.MapHub<CommentHub>("/commentHub");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Event}/{action=HomePage}/{id?}");
            app.MapRazorPages();

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new[] { "Admin", "User" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                string email = "myadmin@bookevents.com";
                string password = "Admin1!";

                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new AppUser();
                    user.Email = email;
                    user.UserName = email;
                    user.FirstName = "Admin";
                    user.LastName = "Admin";


                    await userManager.CreateAsync(user, password);

                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            app.Run();
        }
    }
}
