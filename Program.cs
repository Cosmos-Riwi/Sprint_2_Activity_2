using RestaurantSystem.Services;
using RestaurantSystem.Configuration;
using RestaurantSystem.Data;

namespace RestaurantSystem
{
    /// <summary>
    /// Main application entry point for ASP.NET Core web application
    /// </summary>
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllersWithViews();
            
            // Register database context
            builder.Services.AddScoped<PostgreSqlDbContext>();
            
            // Register application services
            builder.Services.AddScoped<CustomerService>();
            builder.Services.AddScoped<WaiterService>();
            builder.Services.AddScoped<DishService>();
            builder.Services.AddScoped<OrderService>();
            builder.Services.AddScoped<ReservationService>();

            var app = builder.Build();

            // Initialize database on startup
            await InitializeDatabaseAsync(app.Services);

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            // Configure routing
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        /// <summary>
        /// Initialize database tables on application startup
        /// </summary>
        private static async Task InitializeDatabaseAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<PostgreSqlDbContext>();
            
            try
            {
                // Test database connection first
                var isConnected = await dbContext.TestConnectionAsync();
                if (!isConnected)
                {
                    Console.WriteLine("Error: No se pudo conectar a la base de datos");
                    return;
                }
                
                Console.WriteLine("✓ Conexión a base de datos exitosa");
                await dbContext.InitializeTablesAsync();
                Console.WriteLine("✓ Tablas de base de datos inicializadas correctamente");
            }
            catch (Exception ex)
            {
                // Log error but don't stop the application
                Console.WriteLine($"Warning: Could not initialize database tables: {ex.Message}");
            }
        }
    }
}