using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Pierres.Models
{
  public class PierresContextFactory : IDesignTimeDbContextFactory<PierresContext>
  {
    PierresContext IDesignTimeDbContextFactory<PierresContext>.CreateDbContext(string[] args)
    {
      IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

      DbContextOptionsBuilder<PierresContext> builder = new DbContextOptionsBuilder<PierresContext>();

      builder.UseMySql(configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(configuration["ConnectionStrings:DefaultConnection"]));

      return new PierresContext(builder.Options);
    }
  }
}