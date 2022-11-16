using Microsoft.EntityFrameworkCore;
using WebAPITemplate.API.Interceptors;
using WebAPITemplate.Shared.Models.Entities;

namespace WebAPITemplate.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new UpdateAuditableEntitiesInterceptor());
        base.OnConfiguring(optionsBuilder);
    }
}