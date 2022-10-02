using Core.Abstractions;
using Core.Extensions;
using Core.Settings;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;
// Add services to the container.

builder.Services.AddDistributedMemoryCache();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.Configure<CacheSettings>(config.GetSection("CacheSettings"));
builder.Services.AddCoreLayer(config); 
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
