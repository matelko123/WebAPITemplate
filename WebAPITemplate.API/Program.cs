using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;
using WebAPITemplate.API.Data;
using WebAPITemplate.API.Interceptors;
using WebAPITemplate.API.Middlewares;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.

    builder.Services.AddControllers();
    
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
        .AddNegotiate();

    builder.Services.AddAuthorization(options =>
    {
        // By default, all incoming requests will be authorized according to the default policy.
        options.FallbackPolicy = options.DefaultPolicy;
    });
    
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