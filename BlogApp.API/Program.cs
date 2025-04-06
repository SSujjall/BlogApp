using BlogApp.Infrastructure.DI;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using BlogApp.Domain.Configs;
using CloudinaryDotNet;
using BlogApp.Application.Helpers.EmailService.Config;
using BlogApp.Application.Helpers.CloudinaryService.Config;
using BlogApp.Application.Mappings;
using Microsoft.AspNetCore.Authentication.Google;
using BlogApp.Application.Helpers.GoogleAuthService.Config;
using Microsoft.AspNetCore.RateLimiting;
using BlogApp.Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Dependency Injection for DB Connection, Services and Repos.
builder.Services.AddInfrastructure(builder.Configuration);

var configuration = builder.Configuration;

// USE ENVIRONMENT VARIABLES (Necessary for using secrets in production through environment variables)
configuration.AddEnvironmentVariables();

#region Authentication JWT Config
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters();
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero, // Prevent extra valid time after expiry
            ValidAudience = configuration["JWT:ValidAudience"],
            ValidIssuer = configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
        };
    });
//.AddGoogle(GoogleDefaults.AuthenticationScheme, opt =>
//{
//    opt.ClientId = builder.Configuration["Authentications:Google:ClientId"];
//    opt.ClientSecret = builder.Configuration["Authentications:Google:ClientSecret"];
//    opt.SaveTokens = true;
//});
#endregion

builder.Services.AddAuthorization(); // THIS IS NEEDED FOR [AUTHORIZE] to WORK!!!!!!!!!!!

// Automapper Config
builder.Services.AddAutoMapper(typeof(ReactionMappingProfile));

#region Swagger configuration
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BlogApp.API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
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
            new string[] { }
        }
    });
});
#endregion

#region CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
#endregion

#region Set Config to Models and Register service
// settings the values of jwt from configuration to the JWTSettings class
var jwtSection = builder.Configuration.GetSection("JWT");
builder.Services.Configure<JwtConfig>(jwtSection);

// settings the values of EmailConfiguration from configuration to the EmailConfig class
var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfig>();
builder.Services.AddSingleton(emailConfig);

// settings the values of cloudinary from configuration to the cloudinary config class
var cloudinaryConfig = builder.Configuration.GetSection("CloudinarySettings").Get<CloudinaryConfig>();
var account = new Account(cloudinaryConfig.CloudName, cloudinaryConfig.ApiKey, cloudinaryConfig.ApiSecret);
var cloudinary = new Cloudinary(account);
builder.Services.AddSingleton(cloudinary);

// settings the values of Google from configuration to the GoogleConfig class
// .Configure is used because the values for config might change in the future
var googleConfig = builder.Configuration.GetSection("Authentications:Google");
builder.Services.Configure<GoogleConfig>(googleConfig);
#endregion

#region Rate Limiting for API
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("ReadPolicy", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 60;
    });

    options.AddFixedWindowLimiter("WritePolicy", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 20;
    });

    options.AddFixedWindowLimiter("VotePolicy", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 30;
    });

    options.AddConcurrencyLimiter("ConcurrentPolicy", opt =>
    {
        opt.PermitLimit = 10; // Limits to 10 concurrent requests
    });

    //options.RejectionStatusCode = 429;

    // custom resposne for rate limit exceeded
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        await context.HttpContext.Response.WriteAsync("Too many requests. Please try later again... ", cancellationToken: token);
    };
});
#endregion

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog API v1"));
}

//if (!app.Environment.IsDevelopment())
//{
    app.UseErrorHandling();
//}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter(); // Using rate limiter for limiting api calls.

app.MapControllers();

app.Run();
