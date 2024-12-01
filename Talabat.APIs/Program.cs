using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Helpers;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;

namespace Talabat.APIs
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			#region Configure Services
			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddDbContext<StoreContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});


			builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));

			#endregion

			var app = builder.Build();

			using var scope = app.Services.CreateScope();

			var services = scope.ServiceProvider;

			var _dbContext = services.GetRequiredService<StoreContext>(); // ASK CLR for creating object from DbContext Explicitly

			var LoggerFactory = services.GetRequiredService<ILoggerFactory>();

			try
			{
				await _dbContext.Database.MigrateAsync(); // Update-Database
				await StoreContextSeed.SeedAsync(_dbContext); // Seeding in Database
			}
			catch (Exception ex)
			{
				var logger = LoggerFactory.CreateLogger<Program>();
				logger.LogError(ex,"Error occured during the migration of the Database");
			}

			#region Configure Middlewares
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();


			app.MapControllers(); 
			#endregion

			app.Run();
		}
	}
}
