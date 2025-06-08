// Program.cs
using Elastic.Apm.NetCoreAll; // <-- ADD THIS LINE

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAllElasticApm();

// Add services to the container.
builder.Services.AddControllers(); // Ensures services for controllers are added

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // For Swagger UI

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Enables Swagger UI at /swagger
}

// app.UseHttpsRedirection(); // You can uncomment this later if you set up HTTPS

app.UseAuthorization(); // Add if not present, though often included

app.MapControllers(); // This maps attribute-routed controllers like TrafficReportsController

app.Run();