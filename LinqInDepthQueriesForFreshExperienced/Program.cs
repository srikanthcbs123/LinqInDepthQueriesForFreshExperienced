using LinqInDepthQueriesForFreshExperienced.NorthWind_Connect;
using LinqInDepthQueriesForFreshExperienced.NorthWind_DB_DBConnect;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<NorthwindContext>();
builder.Services.AddDbContext<NorthwindDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
