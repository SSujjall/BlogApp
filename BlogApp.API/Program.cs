using BlogApp.Infrastructure.DI;
using BlogApp.Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// USE ENVIRONMENT VARIABLES (Necessary for using secrets in production through environment variables)
builder.Configuration.AddEnvironmentVariables();

// Dependency Injection for DB Connection, Services and Repos.
builder.Services.AddInfrastructure(builder.Configuration);

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
