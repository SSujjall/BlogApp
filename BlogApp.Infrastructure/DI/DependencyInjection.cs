using BlogApp.Application.Helpers.CloudinaryService.Service;
using BlogApp.Application.Helpers.EmailService.Service;
using BlogApp.Application.Helpers.GoogleAuthService.Service;
using BlogApp.Application.Helpers.TokenHelper;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Entities;
using BlogApp.Infrastructure.Persistence.Contexts;
using BlogApp.Infrastructure.Persistence.Health;
using BlogApp.Infrastructure.Redis_Cache.Service;
using BlogApp.Infrastructure.Repositories;
using BlogApp.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace BlogApp.Infrastructure.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<AppDbContext>(options =>
            //    options.UseSqlServer(configuration.GetConnectionString("BlogDB"),
            //    b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)).UseLazyLoadingProxies(), ServiceLifetime.Transient);

            // Register DbContext
            // enable retry on failure when connnecting db
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("BlogDB"),
                     sqlOptions => sqlOptions.EnableRetryOnFailure(3))
                                             .UseLazyLoadingProxies(), ServiceLifetime.Scoped
            );

            #region Identity Configuration
            services.AddIdentity<Users, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            }).AddEntityFrameworkStores<AppDbContext>()
              .AddDefaultTokenProviders();
            #endregion

            #region Register Repositories
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAuthRepository, AuthRepository>();
            services.AddTransient<IBlogRepository, BlogRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddScoped<IBlogReactionRepository, BlogReactionRepository>();
            services.AddScoped<ICommentReactionRepository, CommentReactionRepository>();
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
            #endregion

            #region Add Health Check Configuration
            services.AddHealthChecks()
                .AddCheck<DbHealthCheck>("Database")
                .AddCheck<SmtpHealthCheck>("SMTP")
                .AddCheck<CloudinaryHealthCheck>("Cloudinary");
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

            return services;
        }
    }
}
