using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using FailurePredictionSystemBack.Common.ConfigModels;
using FailurePredictionSystemBack.Persistence;
using FailurePredictionSystemBack.ServiceLayer.BackgroundServices;
using FailurePredictionSystemBack.ServiceLayer.Services.AlertService;
using FailurePredictionSystemBack.ServiceLayer.Services.AuthService;
using FailurePredictionSystemBack.ServiceLayer.Services.CurrentUserService;
using FailurePredictionSystemBack.ServiceLayer.Services.DashboardService;
using FailurePredictionSystemBack.ServiceLayer.Services.EquipmentNotificationSubscriptionService;
using FailurePredictionSystemBack.ServiceLayer.Services.EquipmentService;
using FailurePredictionSystemBack.ServiceLayer.Services.MetricService;
using FailurePredictionSystemBack.ServiceLayer.Services.MlPredictionClient;
using FailurePredictionSystemBack.ServiceLayer.Services.NotificationService;
using FailurePredictionSystemBack.ServiceLayer.Services.PredictionService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.Configure<MlServiceSettings>(
    builder.Configuration.GetSection("MlService"));

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

var mlServiceSettings = builder.Configuration
    .GetSection("MlService")
    .Get<MlServiceSettings>();

if (mlServiceSettings is null || string.IsNullOrWhiteSpace(mlServiceSettings.BaseUrl))
{
    throw new InvalidOperationException("MlService:BaseUrl is not configured.");
}

var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>();

if (jwtSettings is null)
    throw new InvalidOperationException("JwtSettings is not configured.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddHttpClient<IMlPredictionClient, MlPredictionClient>(client =>
{
    client.BaseAddress = new Uri(mlServiceSettings.BaseUrl);
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<FailureSystemDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseLazyLoadingProxies();
});

builder.Services.AddHostedService<PredictionBackgroundService>();

builder.Services.AddScoped<IEquipmentService, EquipmentService>();
builder.Services.AddScoped<IMetricService, MetricService>();
builder.Services.AddScoped<IPredictionService, PredictionService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IEmailNotificationSender, EmailNotificationSender>();
builder.Services.AddScoped<IEquipmentNotificationSubscriptionService, EquipmentNotificationSubscriptionService>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Введите JWT токен"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler(options => options.Run(async context =>
    {
        var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionFeature?.Error;

        var statusCode = exception switch
        {
            InvalidOperationException => HttpStatusCode.BadRequest,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            KeyNotFoundException => HttpStatusCode.NotFound,
            _ => HttpStatusCode.InternalServerError
        };

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/problem+json";

        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = statusCode.ToString(),
            Detail = exception?.Message,
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(problemDetails);
    })
);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();