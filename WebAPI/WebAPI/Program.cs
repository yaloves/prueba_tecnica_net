using Infraestructure.Data;
using Infraestructure.Services;
using Logic;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApiDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("Api")
    ?? throw new NotImplementedException()));

builder.Services.AddHttpClient();
builder.Services.AddScoped<ApiManager>();
builder.Services.AddScoped<OpenDataService>();


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
