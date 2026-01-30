using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Application.Abstracts.Repositories;
using Persistence.Repositories;
using Application.Abstracts.Services;
using Persistence.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Application.Validations;
using Application.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreatePropertyAdRequestValidator>();
builder.Services.AddAutoMapper(typeof(PropertyAdProfile).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IRepository<,>),
                           typeof(GenericRepository<,>));

builder.Services.AddScoped<IPropertyAdRepository, PropertyAdRepository>();
builder.Services.AddScoped<IPropertyAdService, PropertyAdService>();

builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICityService, CityService>();

builder.Services.AddDbContext<BinaLiteDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
