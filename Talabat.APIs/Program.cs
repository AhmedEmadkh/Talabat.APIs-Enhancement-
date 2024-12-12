using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
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
			builder.Services.AddAutoMapper(typeof(MappingProfiles));

			builder.Services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = (actionContext) =>
				{
					var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
														 .SelectMany(P => P.Value.Errors)
														 .Select(P => P.ErrorMessage)
														 .ToList();

					var response = new ApiValidationErrorResponse()
					{
						Errors = errors
					};
					return new BadRequestObjectResult(response);
				};
			});

			// Redis
			builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
			{
				var Connection = builder.Configuration.GetConnectionString("RedisConnection");

				return ConnectionMultiplexer.Connect(Connection);
			});
			


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
