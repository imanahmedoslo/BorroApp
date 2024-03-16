using BorroApp.Data;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BorroAppTests;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class {
	public IConfiguration Configuration { get; private set; }

	protected override void ConfigureWebHost(IWebHostBuilder builder) {
		builder.ConfigureAppConfiguration((context, config) => { Configuration = config.Build(); });

		builder.ConfigureServices(services => {
			// Remove the app's ApplicationDbContext registration.
			var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BorroDbContext>));
			if (descriptor != null) {
				services.Remove(descriptor);
			}

			// Add ApplicationDbContext using an Azure SQL Database.
			services.AddDbContext<BorroDbContext>(options => {
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
			});
		});
	}
}