using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Infrastructure.Data;
public class LMSContextFactory : IDesignTimeDbContextFactory<LmsContext>
{
    public LmsContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetParent(Directory.GetCurrentDirectory())?.FullName;
        ArgumentNullException.ThrowIfNull(nameof(basePath));

        var configuration = new ConfigurationBuilder()
                                    .SetBasePath(Path.Combine(basePath!, "LMS.API")) 
                                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                    .Build();

        var builder = new DbContextOptionsBuilder<LmsContext>().UseSqlServer(configuration.GetConnectionString("LmsContext"));

        return new LmsContext(builder.Options);
    }
}

