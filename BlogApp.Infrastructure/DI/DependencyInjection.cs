using BlogApp.Application.Helpers.CloudinaryService.Config;
using BlogApp.Application.Helpers.CloudinaryService.Service;
using BlogApp.Application.Helpers.EmailService.Config;
using BlogApp.Application.Helpers.EmailService.Service;
using BlogApp.Application.Helpers.GoogleAuthService.Config;
using BlogApp.Application.Helpers.GoogleAuthService.Service;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Helpers.TokenHelper;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Application.Interface.IServices;
using BlogApp.Application.Mappings;
using BlogApp.Domain.Configs;
using BlogApp.Domain.Entities;
using BlogApp.Infrastructure.Persistence.Contexts;
using BlogApp.Infrastructure.Persistence.Health;
using BlogApp.Infrastructure.Redis_Cache.Service;
using BlogApp.Infrastructure.Repositories;
using BlogApp.Infrastructure.Services;
using BlogApp.Infrastructure.Services.HelperServices;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using StackExchange.Redis;
using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Google;
using BlogApp.Application.Interface.IServices.IPaymentService;
using BlogApp.Infrastructure.Services.PaymentService;

namespace BlogApp.Infrastructure.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Register DbContext
            //services.AddDbContext<AppDbContext>(options =>
            //    options.UseSqlServer(configuration.GetConnectionString("BlogDB"),
            //    b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)).UseLazyLoadingProxies(), ServiceLifetime.Transient);


            // enable retry on failure when connnecting db
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("BlogDB"),
                     sqlOptions => sqlOptions.EnableRetryOnFailure(3))
                                             .UseLazyLoadingProxies(), ServiceLifetime.Scoped
            );
            #endregion

            #region Identity Configuration
            services.AddIdentityCore<Users>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
              .AddRoles<IdentityRole>()
              .AddEntityFrameworkStores<AppDbContext>()
              .AddDefaultTokenProviders();
            #endregion

            #region Add Authentication with JWT Config
            services.AddAuthentication(options =>
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

                    // Custom responses
                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse(); // Skip default

                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";
                            var error = new Dictionary<string, string>
                            {
                                { "Unauthorized", "Invalid or missing auth token." }
                            };
                            var result = JsonSerializer.Serialize(
                                ApiResponse<string>.Failed(error, "Unable to access", HttpStatusCode.Unauthorized)
                            );

                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            context.Response.ContentType = "application/json";
                            var error = new Dictionary<string, string>
                            {
                                { "Forbidden", "You don’t have permission." }
                            };
                            var result = JsonSerializer.Serialize(
                                ApiResponse<string>.Failed(error, "Unable to access", HttpStatusCode.Forbidden)
                            );

                            return context.Response.WriteAsync(result);
                        }
                    };
                });
            //.AddGoogle(GoogleDefaults.AuthenticationScheme, opt =>
            //{
            //    opt.ClientId = builder.Configuration["Authentications:Google:ClientId"];
            //    opt.ClientSecret = builder.Configuration["Authentications:Google:ClientSecret"];
            //    opt.SaveTokens = true;
            //});
            #endregion

            #region Add Authorization
            services.AddAuthorization(); // THIS IS NEEDED FOR [AUTHORIZE] to WORK!!!!!!!!!!!
            #endregion

            #region Set Config to Models and Register service
            // settings the values of jwt from configuration to the JWTSettings class
            var jwtSection = configuration.GetSection("JWT");
            services.Configure<JwtConfig>(jwtSection);

            // settings the values of EmailConfiguration from configuration to the EmailConfig class
            var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfig>();
            services.AddSingleton(emailConfig);

            // settings the values of cloudinary from configuration to the cloudinary config class
            var cloudinaryConfig = configuration.GetSection("CloudinarySettings").Get<CloudinaryConfig>();
            var account = new Account(cloudinaryConfig.CloudName, cloudinaryConfig.ApiKey, cloudinaryConfig.ApiSecret);
            var cloudinary = new Cloudinary(account);
            services.AddSingleton(cloudinary);

            // settings the values of Google from configuration to the GoogleConfig class
            // .Configure is used because the values for config might change in the future
            var googleConfig = configuration.GetSection("Authentications:Google");
            services.Configure<GoogleConfig>(googleConfig);
            #endregion

            #region Register Repositories
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IBlogRepository, BlogRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddScoped<IBlogReactionRepository, BlogReactionRepository>();
            services.AddScoped<ICommentReactionRepository, CommentReactionRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPaymentLogsRepository, PaymentLogsRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            //services.AddScoped<IRefundsRepository, RefundsRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            #endregion

            #region Register Services
            services.AddTransient<ICloudinaryService, CloudinaryService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddScoped<IBlogReactionService, BlogReactionService>();
            services.AddScoped<ICommentReactionService, CommentReactionService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IGoogleAuthService, GoogleAuthService>();
            services.AddSingleton<IBackgroundEmailQueue, BackgroundEmailQueue>();
            services.AddHostedService<EmailBackgroundService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPaymentProviderFactory, PaymentProviderFactory>();
            services.AddScoped<EsewaPaymentService>();
            services.AddScoped<KhaltiPaymentService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            //services.AddScoped<IRefundService, RefundService>();
            #endregion

            #region Register Redis Distributed Cache Instance
            //// Register Redis Connection
            //builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
            //{
            //    var redisConString = builder.Configuration["RedisConfig:RedisConString"];
            //    return ConnectionMultiplexer.Connect(redisConString);
            //});

            // Register Redis Connection, written in more simpler way
            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(configuration["RedisConfig:RedisConString"]!)
            );

            ////if you dont need advanced redis features
            //builder.Services.AddStackExchangeRedisCache(opt =>
            //{
            //    string connection = builder.Configuration["RedisConfig:RedisConString"];
            //    opt.Configuration = connection;
            //});

            //// Register Redis Distributed Cache
            services.AddStackExchangeRedisCache(opts =>
            {
                opts.Configuration = configuration["RedisConfig:RedisConString"];
                //opts.InstanceName = "BlogApp"; // This adds a prefix to the keys in redis (not needed)
            });

            //// Register redis interface and service
            services.AddScoped<IRedisCache, RedisCache>();
            #endregion

            #region Add Health Check Configuration
            services.AddHealthChecks()
                .AddCheck<DbHealthCheck>("Database")
                .AddCheck<SmtpHealthCheck>("SMTP")
                .AddCheck<CloudinaryHealthCheck>("Cloudinary");
            #endregion

            #region Automapper Config
            services.AddAutoMapper(
                cfg => { },
                typeof(UserMappingProfile).Assembly
            );
            #endregion

            #region Swagger configuration
            services.AddSwaggerGen(opts =>
            {
                opts.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                opts.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("bearer", document)] = []
                });
            });
            #endregion

            #region CORS Policy
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            #endregion

            #region Rate Limiting for API
            services.AddRateLimiter(options =>
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

            return services;
        }
    }
}
