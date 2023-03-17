using Curso.Core.Configuration;
using Curso.Core.Data.Mappings;
using Curso.Core.Model.DataModels;
using DevSnap.CommonLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
//using curso.Core.Models.DataModels;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;

namespace Curso.Core.Data
{
    public class CoreDbContext : DbContext
    {
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var service = ConfigureSettings.GetService(ConfigureSettings.SecaVoceService);
            if (service.Repository.Provider == "SqlServer")
                options.UseSqlServer(service.Repository.ConnectionString);
            else
                options.UseMySql(service.Repository.ConnectionString,
                    new MySqlServerVersion(new Version(8, 0, 21)), // Replace with your server version and type.
                    mySqlOptions => mySqlOptions.CharSetBehavior(CharSetBehavior.NeverAppend));
        }

        //public DbSet<Address> Addresses { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<PermitionAccess> PermitionAccesses { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<CustomerVideo> CustomerVideos { get; set; }
        public DbSet<FileData> Files { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.AddConfiguration(new AnswerMap());
            builder.AddConfiguration(new QuestionMap());
            builder.AddConfiguration(new PermitionAccessMap());
            builder.AddConfiguration(new VideoMap());
            builder.AddConfiguration(new CustomerVideoMap());
            builder.AddConfiguration(new FileDataMap());




            //builder.Entity<PageAccess>()
            //    .HasKey(x => new { x.PageId, x.UserId });
            //builder.AddConfiguration(new AddressMap());

        }
    }
}