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

var sqlConnectionString = Environment.GetEnvironmentVariable("SqlServer_ConnectionString");
var sbConnectionString = Environment.GetEnvironmentVariable("ServiceBus_ConnectionString");

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