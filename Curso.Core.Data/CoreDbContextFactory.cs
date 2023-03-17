using Curso.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;

namespace Curso.Core.Data
{
    public class CoreDbContextFactory : IDesignTimeDbContextFactory<CoreDbContext>
    {
        public CoreDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CoreDbContext>();
            var service = ConfigureSettings.GetService(ConfigureSettings.SecaVoceService);

            if (service.Repository.Provider == "SqlServer")
                optionsBuilder.UseSqlServer(service.Repository.ConnectionString);
            else
                optionsBuilder.UseMySql(service.Repository.ConnectionString,
                    new MySqlServerVersion(new Version(8, 0, 21)), // Replace with your server version and type.
                    mySqlOptions => mySqlOptions.CharSetBehavior(CharSetBehavior.NeverAppend));

            return new CoreDbContext(optionsBuilder.Options);
        }
    }
}