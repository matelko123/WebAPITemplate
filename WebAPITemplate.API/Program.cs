using Microsoft.EntityFrameworkCore;
using WebAPITemplate.API.Data;
using WebAPITemplate.API.Extensions;
using WebAPITemplate.API.Interceptors;
using WebAPITemplate.API.Middlewares;
using WebAPITemplate.API.Repositories;
using WebAPITemplate.API.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.

    builder.Services.AddControllers();
    
    builder.Services.AddEndpointsApiExplorer();

    // Extensions
    builder.AddSecurityServices();
    builder.Services.RegisterMapsterConfiguration();

    // DbContext
    builder.Services.AddDbContext<AppDbContext>(
        options => options
            .UseSqlServer(builder.Configuration.GetConnectionString("Default"))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking),
        ServiceLifetime.Singleton);
        
    builder.Services.AddDbContextFactory<AppDbContext>(
        options => options
            .UseSqlServer(builder.Configuration.GetConnectionString("Default"))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking),
        ServiceLifetime.Scoped);

    builder.Services.AddSingleton<UpdateAuditableEntitiesInterceptor>();
    
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IUserService, UserService>();
    
    builder.Services.AddScoped<ErrorHandlerMiddleware>();
}

WebApplication app = builder.Build();
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<ErrorHandlerMiddleware>();
    
    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}