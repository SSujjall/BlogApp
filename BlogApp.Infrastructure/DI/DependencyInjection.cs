﻿using BlogApp.Application.Helpers.CloudinaryService;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Entities;
using BlogApp.Infrastructure.Persistence;
using BlogApp.Infrastructure.Repositories;
using BlogApp.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogApp.Infrastructure.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("BlogDB"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)).UseLazyLoadingProxies(), ServiceLifetime.Transient);


            services.AddIdentity<Users, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddDbContext<AppDbContext>();

            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAuthRepository, AuthRepository>();
            services.AddTransient<IBlogRepository, BlogRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();

            services.AddTransient<ICloudinaryService, CloudinaryService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<ICommentService, CommentService>();

            return services;
        }
    }
}
