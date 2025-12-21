using System.Text;
using Infrastructure.Seed;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebAPIWithJWTAndIdentity.AutoMapper;
using WebAPIWithJWTAndIdentity.Data;
using WebAPIWithJWTAndIdentity.MiddleWare;
using WebAPIWithJWTAndIdentity.Services;
using WebAPIWithJWTAndIdentity.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// =====================
// Database
// =====================
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connection));

// =====================
// Identity
// =====================
builder.Services.AddIdentity<IdentityUser, IdentityRole>(config =>
{
    config.Password.RequiredLength = 4;
    config.Password.RequireDigit = false;
    config.Password.RequireNonAlphanumeric = false;
    config.Password.RequireUppercase = false;
    config.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// =====================
// JWT Authentication
// =====================
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// =====================
// DI / Services
// =====================
// Тут можешь вставить свои сервисы
builder.Services.AddScoped<Seeder>();
builder.Services.AddScoped<ITodoItemService,TodoItemService>(); // <-- сюда свои сервисы добавишь
builder.Services.AddScoped<IAccountService,AccountService>(); // <-- сюда свои сервисы добавишь
builder.Services.AddAutoMapper(typeof(MapperProfile)); 

// =====================
// Swagger
// =====================
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sample Web API",
        Version = "v1",
        Description = "Sample API Services."
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// =====================
// Controllers
// =====================
builder.Services.AddControllers();

var app = builder.Build();
// =====================
// Auto migrate DB + seed roles/users
// =====================
try
{
    using var scope = app.Services.CreateScope();
    var serviceProvider = scope.ServiceProvider;

    var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();

    var seeder = serviceProvider.GetRequiredService<Seeder>();
    await seeder.SeedRole();
    await seeder.SeedUser();
}
catch (Exception ex)
{
    // Можно логировать, если нужно
}

// =====================
app.UseMiddleware<CustomLoggingMiddleware>();
// =====================
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
