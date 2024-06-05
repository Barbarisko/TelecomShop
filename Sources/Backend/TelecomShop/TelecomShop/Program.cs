using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TelecomShop;
using TelecomShop.DBModels;
using TelecomShop.ErrorHandlers;
using TelecomShop.Repository;
using TelecomShop.Services;
using TelecomShop.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Handlers
builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddTransient<ITokenGenerator, TokenGenerator>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<ISuperpowerService, SuperpowersService>();

builder.Services.AddEntityFrameworkNpgsql().AddDbContext<TelcoShopDbContext>(opt =>
                                 opt.UseNpgsql(builder.Configuration.GetConnectionString("db")));

builder.Services.AddScoped<IRepository<ActiveProduct>, Repository<ActiveProduct>>();
builder.Services.AddScoped<IRepository<BillingAccount>, Repository<BillingAccount>>();
builder.Services.AddScoped<IRepository<Characteristic>, Repository<Characteristic>>();
builder.Services.AddScoped<IRepository<CharInvolvement>, Repository<CharInvolvement>>();
builder.Services.AddScoped<IRepository<Product>, Repository<Product>>();
builder.Services.AddScoped<IRepository<User>, Repository<User>>();


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


// Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var tokenSettings = builder.Configuration.GetSection("TokenSettings");

        options.Audience = tokenSettings.GetValue<string>("Audience");
        options.ClaimsIssuer = tokenSettings.GetValue<string>("Issuer");
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = false,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                    tokenSettings.GetValue<string>("Key")))
        };
    });

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
   {
     new OpenApiSecurityScheme
     {
       Reference = new OpenApiReference
       {
         Type = ReferenceType.SecurityScheme,
         Id = "Bearer"
       }
      },
      new string[] { }
        }
  });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(opt => { });

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
