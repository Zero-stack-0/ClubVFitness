using System.Runtime.CompilerServices;
using Data.Repository;
using Microsoft.EntityFrameworkCore;
using Service.Dto.Request;
using Webservice.DependecnyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.Configure<Encryption>(builder.Configuration.GetSection("Encryption"));

DInjection.AddDependecyInjection(builder.Services, builder.Configuration.GetConnectionString("DefaultConnection"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseHttpsRedirection();
app.Run();

