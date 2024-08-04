using AuthProvider.Api.Handlers;
using AuthProvider.Data.Context;
using AuthProvider.Data.Entities;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Use connection strings from appsettings.json if in development, otherwise from environment variables
var sqlConnectionString = builder.Configuration.GetConnectionString("SqlServer") ?? Environment.GetEnvironmentVariable("SqlServer_ConnectionString");
var sbConnectionString = builder.Configuration.GetConnectionString("ServiceBus") ?? Environment.GetEnvironmentVariable("ServiceBus_ConnectionString");

builder.Services.AddDbContext<DataContext>(o => o.UseSqlServer(sqlConnectionString));

builder.Services.AddIdentity<UserEntity, IdentityRole>(x =>
{
    x.User.RequireUniqueEmail = true;
    x.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<DataContext>();

builder.Services.AddSingleton(sp =>
{
    return new ServiceBusClient(sbConnectionString);
});

builder.Services.AddScoped<ServiceBusHandler>();

builder.Services.AddHttpClient();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();