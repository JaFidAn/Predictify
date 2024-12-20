using Prediction.API;
using Prediction.Application;
using Prediction.Infrastructure;
using Prediction.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseApiServices();

//if (app.Environment.IsDevelopment())
//{
//    await app.InitializeDatabaseAsync();
//}

app.Run();
