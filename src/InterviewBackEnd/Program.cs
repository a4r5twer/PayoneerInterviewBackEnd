using InterviewBackEnd.Infrastructure;
using NLog.Extensions.Logging;
var builder = WebApplication.CreateBuilder(args);

//set up nlog for this project 
builder.Logging.ClearProviders();
builder.Logging.AddNLog("Nlog.config");


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterFilters();
builder.Services.RegisterDbContext(builder.Configuration.GetConnectionString("LocalTest"));
builder.Services.RegisterServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseErrorHandling();
app.UseRequestResponseLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();
