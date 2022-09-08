using Common.Config;
using Common.Data;
using Microsoft.EntityFrameworkCore;
using Services.Middleware;

// Add services to the container.
var arycaCorsOrigin = "_ARYCA-CORS-ORIGIN";
var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls(GlobalConfigFactory.For().GetApiUrl());
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddCors(options =>
{
	options.AddPolicy(name: arycaCorsOrigin,
		policy =>
		{
			policy.WithOrigins("http://client-container:9998");
		});
});

Console.WriteLine(builder.Environment.ToString());

builder.Services.AddDbContext<DataContext>(options =>
{
	options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
	options.UseSqlite(b => b.MigrationsAssembly("Services"));
});

// Configure the HTTP request pipeline.
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseRouting();
app.UseCors(arycaCorsOrigin);
//app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(origin => true));

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
});

app.Run();