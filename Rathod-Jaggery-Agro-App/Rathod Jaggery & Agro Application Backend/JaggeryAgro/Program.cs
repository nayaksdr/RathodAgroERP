using Azure;
using Azure.AI.OpenAI;
using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Core.Services;
using JaggeryAgro.Infrastructure.Data;
using JaggeryAgro.Infrastructure.Repositories;
using JaggeryAgro.Infrastructure.Services;
using JaggeryAgro.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using AutoMapper;
using System.Globalization;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

#region 🔹 DATABASE

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion

#region 🔹 IDENTITY

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

#endregion

#region 🔹 JWT AUTHENTICATION

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

#endregion

#region 🔹 CORS (ANGULAR)

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod());
});

#endregion

#region 🔹 CONTROLLERS + SWAGGER

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });
   

builder.Services.AddAutoMapper(
    cfg => { },
    typeof(LaborTypeRateProfile).Assembly
);


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Jaggery Agro Web API",
        Version = "v1"
    });
});

#endregion

#region 🔹 AUTHORIZATION

builder.Services.AddAuthorization();

#endregion

#region 🔹 REPOSITORIES (✅ ALL REGISTERED)

builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
builder.Services.AddScoped<ILaborRepository, LaborRepository>();
builder.Services.AddScoped<IAdvancePaymentRepository, AdvancePaymentRepository>();
builder.Services.AddScoped<ILaborPaymentRepository, LaborPaymentRepository>();
builder.Services.AddScoped<ILaborTypeRepository, LaborTypeRepository>();
builder.Services.AddScoped<ILaborTypeRateRepository, LaborTypeRateRepository>();
builder.Services.AddScoped<IFarmerRepository, FarmerRepository>();
builder.Services.AddScoped<ICaneAdvanceRepository, CaneAdvanceRepository>();
builder.Services.AddScoped<ICanePaymentRepository, CanePaymentRepository>();
builder.Services.AddScoped<ICanePurchaseRepository, CanePurchaseRepository>();
builder.Services.AddScoped<IProduceRepository, ProduceRepository>();
builder.Services.AddScoped<IDealerRepository, DealerRepository>();
builder.Services.AddScoped<IDealerAdvanceRepository, DealerAdvanceRepository>();
builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();
builder.Services.AddScoped<ISplitwiseRepository, SplitwiseRepository>();
builder.Services.AddScoped<IExpenseTypeRepository, ExpenseTypeRepository>();
builder.Services.AddScoped<IJaggerySaleRepository, JaggerySaleRepository>();

// ✅ MISSING FIX (IMPORTANT)
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddHttpClient();

#endregion

#region 🔹 SERVICES

builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IFarmerService, FarmerService>();
builder.Services.AddScoped<IProduceService, ProduceService>();
builder.Services.AddScoped<AiAgentService>();
builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddScoped<ILaborTypeRateService,LaborTypeRateService>();
#endregion

#region 🔹 SERILOG

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("Logs/api-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

#endregion

var app = builder.Build();

#region 🔹 MIDDLEWARE PIPELINE (ORDER IS IMPORTANT)

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthentication();
app.UseAuthorization();

#endregion

#region 🔹 ROUTING

app.MapControllers();

// Optional root endpoint
app.MapGet("/", () => "Jaggery Agro API is running");

#endregion

app.Run();
