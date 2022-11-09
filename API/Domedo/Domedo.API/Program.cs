using Domedo.API.Config.Swagger;
using Domedo.API.Database.Seeds;
using Domedo.API.Validator;
using Domedo.App.IRepositroy;
using Domedo.App.Mapper;
using Domedo.App.Middlewares;
using Domedo.App.Repository;
using Domedo.App.Services.Token;
using Domedo.App.Utils;
using Domedo.Domain.Context;
using Domedo.Domain.Entities;
using Domedo.Domain.Requests.Auth;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ReflectionIT.Mvc.Paging;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddTransient<IValidator<LoginRequest>, LoginRequestValidator>();

builder.Services.AddHttpClient();
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});
builder.Services.AddPaging(options =>
{
    options.PageParameterName = "page";
});

var defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");

var defaultDbConnectionString = Environment.GetEnvironmentVariable("DomedoDbConnectionString");

builder.Services.AddCors(options =>
{
    options.AddPolicy(AppConstants.MyAllowedSpecificOrigins,
        builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Domedo", Version = "v1" });
    c.EnableAnnotations();

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization Header using Bearer Security Scheme. \r\r\r\r Enter Bearer [space] and then the security token to authenticate",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"

    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                    new OpenApiSecurityScheme
                    {

                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },

                    new List<string>()
                    }
                });
    c.OperationFilter<ReApplyOptionalRouteParameterOperationFilter>();

});

builder.Services.AddDbContext<DomedoDbContext>(o =>
           o.UseMySql((defaultDbConnectionString ?? defaultConnection) ?? string.Empty,
               ServerVersion.AutoDetect(defaultDbConnectionString ?? defaultConnection)));

builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = false;

})
                .AddRoles<Role>()
                .AddUserManager<UserManager<User>>()
                .AddEntityFrameworkStores<DomedoDbContext>()
                .AddDefaultTokenProviders();

var jwtKey = builder.Configuration.GetValue<string>("JwtSettings:Key");
var keyBytes = Encoding.ASCII.GetBytes(jwtKey);

TokenValidationParameters tokenValidation = new()
{
    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
    ValidateLifetime = true,
    ValidateAudience = false,
    ValidateIssuer = false,
    ClockSkew = TimeSpan.Zero
};

builder.Services.AddSingleton(tokenValidation);

builder.Services.AddAuthentication(authOptions =>
{
    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

})
               .AddJwtBearer(jwtOptions =>
               {
                   jwtOptions.TokenValidationParameters = tokenValidation;
               });

builder.Services.AddAutoMapper(typeof(DomedoMapper));

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IMealRepository, MealRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


builder.Services.AddScoped<IJwtService, JwtService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Domedo v1");
    c.DefaultModelsExpandDepth(-1);

    c.RoutePrefix = "";
});

app.UseHttpsRedirection();

app.SeedData();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(AppConstants.MyAllowedSpecificOrigins);

app.UseAuthentication();

app.UseMiddleware<JwtMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
