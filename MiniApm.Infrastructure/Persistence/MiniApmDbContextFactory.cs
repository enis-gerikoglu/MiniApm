using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MiniApm.Infrastructure.Persistence;

public class MiniApmDbContextFactory : IDesignTimeDbContextFactory<MiniApmDbContext>
{
    public MiniApmDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MiniApmDbContext>();
        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=MiniApmDb;User Id=sa;Password=Sifre123!;TrustServerCertificate=True;");

        return new MiniApmDbContext(optionsBuilder.Options);
    }
}