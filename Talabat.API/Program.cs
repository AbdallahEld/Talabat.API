using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.API.Errors;
using Talabat.API.Extensions;
using Talabat.API.Helpers;
using Talabat.API.Middlewears;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.API
{
    public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			#region Services
			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			#region Ensure Swagger Differ Between Schemas With the same name
			builder.Services.AddSwaggerGen(c =>
				{
					c.CustomSchemaIds(type => type.FullName); // Use fully qualified type names for schema IDs
				}); 
			#endregion
			builder.Services.AddDbContext<StoreContext>(Options =>
			Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
			);
			builder.Services.AddDbContext<AppIdentityDbContext>(Options =>
			{
				Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
			});
			builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
			{
				var Connections = builder.Configuration.GetConnectionString("RedisConnection");
				return ConnectionMultiplexer.Connect(Connections);
			});
			
			builder.Services.AddApplicationServices();

			builder.Services.AddIdentityServices(builder.Configuration);
			builder.Services.AddCors(Options =>
			{
				Options.AddPolicy("MyPolicy", Options =>
				{
					Options.AllowAnyHeader();
					Options.AllowAnyMethod();
					Options.WithOrigins(builder.Configuration["FrontBaseUrl"]);
				});
			});
			#endregion

			var app = builder.Build();

			var Scope = app.Services.CreateScope();
			var Services = Scope.ServiceProvider;
			var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
			try
			{
				var DbContext = Services.GetRequiredService<StoreContext>();
				await DbContext.Database.MigrateAsync();

				var AppDbcontext = Services.GetRequiredService<AppIdentityDbContext>();
				await AppDbcontext.Database.MigrateAsync();

				var userManger = Services.GetRequiredService<UserManager<AppUser>>();

				await AppIdentityDbContextSeed.SeedUserAsync(userManger);
				await StoreContextSeed.SeedAsync(DbContext);
			} catch (Exception ex)
			{
				var Logger = LoggerFactory.CreateLogger<Program>();
				Logger.LogError(ex, "Error Occured While Adding Migration");
			}

			// Configure the HTTP request pipeline.
			#region Configurations
			if (app.Environment.IsDevelopment())
			{
				app.UseMiddleware<ExceptionMiddlewear>();
				app.UseSwaggerMiddlewears();
			}
			app.UseStatusCodePagesWithReExecute("/errors/{0}");
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCors("MyPolicy");
			app.UseAuthentication();
			app.UseAuthorization();
			app.MapControllers();
			#endregion
			app.Run();
		}
	}
}
