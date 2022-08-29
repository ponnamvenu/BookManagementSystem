using ConceptArchitect.BookManagement;
using ConceptArchitect.BookManagement.Repositories.AdoNet;
using ConceptArchitect.Utils;
using System.Data.Common;
using System.Data.SqlClient;
// PROJECT
namespace BooksWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //---------------------
            //repostories
            //factory pattern
            builder.Services.AddScoped<Func<DbConnection>>(s =>
            {
                IConfiguration configuration = s.GetService<IConfiguration>();
                return () => new SqlConnection()
                {
                    ConnectionString = configuration.GetConnectionString("BooksDb")
                };
            });
            //Models
            builder.Services.AddScoped<DbRepository>();

            builder.Services.AddScoped<IAuthorService, DefaultAuthorManager>();

            
            builder.Services.AddScoped<IAuthorRepository, AdoAuthorRepository>();


            builder.Services.AddScoped<IBookService, DefaultBookManager>();

            builder.Services.AddScoped<IBookRepository, AdoBookRepository>();


            //----------------


            // Add services to the container.
            builder.Services
                .AddControllersWithViews();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            else
                app.UseDeveloperExceptionPage();


            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}