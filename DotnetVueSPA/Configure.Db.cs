using Microsoft.EntityFrameworkCore;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using DotnetVueSPA.Data;

[assembly: HostingStartup(typeof(DotnetVueSPA.ConfigureDb))]

namespace DotnetVueSPA;

public class ConfigureDb : IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureServices((context, services) => {
            var connectionString = context.Configuration.GetConnectionString("DefaultConnection")
                ?? "DataSource=App_Data/app.db;Cache=Shared";
            
            services.AddSingleton<IDbConnectionFactory>(new OrmLiteConnectionFactory(
                connectionString, SqliteDialect.Provider));

            // $ dotnet ef migrations add CreateIdentitySchema
            // $ dotnet ef database update
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString, b => b.MigrationsAssembly(nameof(DotnetVueSPA))));
            
            // Enable built-in Database Admin UI at /admin-ui/database
            services.AddPlugin(new AdminDatabaseFeature());
        });
}