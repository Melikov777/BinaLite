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
using API.Middlewares;
using Minio;
using Infrastructure.Services;
using Infrastructure.Extensions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreatePropertyAdRequestValidator>();
builder.Services.AddAutoMapper(typeof(PropertyAdProfile).Assembly);

builder.Services.AddEndpointsApiExplorer();

// Swagger with JWT support
builder.Services.AddSwaggerWithJwt();

// Repository registrations
builder.Services.AddScoped(typeof(IRepository<,>), typeof(GenericRepository<,>));
builder.Services.AddScoped<IPropertyAdRepository, PropertyAdRepository>();
builder.Services.AddScoped<IPropertyAdService, PropertyAdService>();
builder.Services.AddScoped<IPropertyMediaRepository, PropertyMediaRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();

// MinIO Integration
builder.Services.AddMinioStorage(builder.Configuration);
builder.Services.AddScoped<IFileStorageService, Infrastructure.Services.S3MinioFileStorageService>();

// Identity
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<BinaLiteDbContext>()
.AddDefaultTokenProviders();

// JWT Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddDbContext<BinaLiteDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseExceptionHandling();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
