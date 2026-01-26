using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Application.Abstracts.Repositories;
using Persistence.Repositories;
using Application.Abstracts.Services;
using Persistence.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IRepository<,>),
                           typeof(GenericRepository<,>));

builder.Services.AddScoped<IPropertyAdRepository, PropertyAdRepository>();
builder.Services.AddScoped<IPropertyAdService, PropertyAdService>();

builder.Services.AddDbContext<BinaLiteDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
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
