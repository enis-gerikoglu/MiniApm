using Microsoft.EntityFrameworkCore;
using MiniApm.Domain.Entities;


namespace MiniApm.Infrastructure.Persistence;

public class MiniApmDbContext : DbContext
{
    public MiniApmDbContext(DbContextOptions<MiniApmDbContext> options) : base(options)
    {
    }
    public DbSet<SystemMetric> SystemMetrics => Set<SystemMetric>();
}