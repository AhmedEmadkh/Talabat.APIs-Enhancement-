using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities.Identitiy;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

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

			builder.Services.AddDbContext<AppIdentityDbContext>(Options =>
			{
				Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
			});

			// Redis
			builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
			{
				var Connection = builder.Configuration.GetConnectionString("RedisConnection");

				return ConnectionMultiplexer.Connect(Connection);
			});


			builder.Services.AddApplicationServices();
            builder.Services.AddIdentityServices();

            #endregion

            var app = builder.Build();

			#region Update - Database
			using var scope = app.Services.CreateScope();

			var services = scope.ServiceProvider;

			var _dbContext = services.GetRequiredService<StoreContext>(); // ASK CLR for creating object from DbContext [Explicitly]
			var _identityDbContext = services.GetRequiredService<AppIdentityDbContext>(); // ASK CLR for creating object from AppIdentityDbContext [Explicitly]

			var LoggerFactory = services.GetRequiredService<ILoggerFactory>();


			try
			{
				await _dbContext.Database.MigrateAsync(); // Update-Database
				await _identityDbContext.Database.MigrateAsync(); // Update-IdentityDatabase
				var userManger = services.GetRequiredService<UserManager<AppUser>>();
				await AppIdentityDbContextSeed.SeedAsync(userManger);
                await StoreContextSeed.SeedAsync(_dbContext); // Seeding in Database
			}
			catch (Exception ex)
			{
				var logger = LoggerFactory.CreateLogger<Program>();
				logger.LogError(ex, "Error occured during the migration of the Database");
			} 
			#endregion

			#region Configure Middlewares
			// Custom Middleware
			app.UseMiddleware<ExceptionMiddleware>();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			app.UseStatusCodePagesWithReExecute("errors/{0}");
			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.MapControllers(); 
			#endregion

			app.Run();
		}
	}
}
