using Infraestructure.Core.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;
using TopMai;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((hostContext, services, configuration) =>
{
    configuration.WriteTo.Console();
    configuration.MinimumLevel.Warning();
    configuration.WriteTo.File("Logs/LogTopmai.txt", rollingInterval: RollingInterval.Day);

});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddControllers().AddJsonOptions(x =>
   {
       x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

   });

builder.Services.AddAutoMapper(typeof(Program).Assembly);
   
//JP
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
    options.AllowAnyHeader();
});



app.MapControllers();

//JP
startup.Configure(app, app.Environment);
app.Run();


enum Turn
{
    Left = 1,
    Right
}

