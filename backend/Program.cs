using backend.Data;
using backend.Interfaces;
using backend.Repository;
using backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IWatchlistRepository, CreateWatchlistRepository>();
builder.Services.AddScoped<IWatchlistItemRepository, CreateWatchlistItemRepository>();
builder.Services.AddHttpClient();

builder.Services.AddScoped<RateRefreshService>();
builder.Services.AddScoped<LatestRateService>();
builder.Services.AddScoped<HistoryRateService>();
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowFrontend" , policy =>
	{
		//policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
		policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
	});
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();
app.MapControllers();


app.Run();


