
using Microsoft.AspNetCore.HttpLogging;
using MyGuitarShop.Data.Ado.Factories;
using System.Diagnostics;

namespace MyGuitarShop.Api
{
	public static class Program
	{
		public static async Task Main(string[] args)
		{
			try
			{
				var builder = WebApplication.CreateBuilder(args);

				AddLogging(builder);
				AddServices(builder);

				// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
				if (builder.Environment.IsDevelopment())
				{
					builder.Services.AddEndpointsApiExplorer();
					builder.Services.AddSwaggerGen();
				}

				var app = builder.Build();

				// Configure the HTTP request pipeline.
				if (app.Environment.IsDevelopment())
				{
					app.UseSwagger();
					app.UseSwaggerUI();
				}

				ConfigureApplication(app);

				await app.RunAsync();
			}
			catch (Exception e)
			{
				if (Debugger.IsAttached) Debugger.Break();

				Console.WriteLine(e);
				throw;
			}
		}

		private static void AddLogging(WebApplicationBuilder builder)
		{
			builder.Services.AddLogging(logging =>
			{
				logging.ClearProviders();
				logging
					.AddFilter("Microsoft", LogLevel.Information)
					.AddFilter("Microsoft.AspNetCore.HttpLogging", LogLevel.Information)
					.AddConsole();
			});

			builder.Services.AddHttpLogging(options =>
			{
				options.LoggingFields = HttpLoggingFields.RequestPath
										| HttpLoggingFields.RequestMethod
										| HttpLoggingFields.ResponseStatusCode;
			});
		}

		private static void ConfigureApplication(WebApplication app)
		{
			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapControllers();
		}

		private static void AddServices(WebApplicationBuilder builder)
		{
			var connectionString = builder.Configuration.GetConnectionString("MyGuitarShop");

			builder.Services.AddSingleton(new SqlConnectionFactory(connectionString));

			builder.Services.AddControllers();
		}
	}
}
