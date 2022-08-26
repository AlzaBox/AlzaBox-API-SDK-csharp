using Microsoft.EntityFrameworkCore;

namespace AlzaBox.API.WebExample.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<__EFMigrationsHistory> __EFMigrationsHistory { get; set; }

    public DbSet<ChangeStatusRequest> ChangeStatusRequests { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}