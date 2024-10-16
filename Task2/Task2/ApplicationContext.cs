using Microsoft.EntityFrameworkCore;
using Task2.Entities;
using Task2.Interfaces;

namespace Task2
{
    public class ApplicationContext : DbContext, IApplicationContext
    {
        public DbSet<ClassOfAccount> Classes { get; set; } = null;
        public DbSet<MainAccauntNubmer> AccauntNumbers { get; set; } = null;
        public DbSet<MainData> Data { get; set; } = null;
        public DbSet<UploadedFiles> Files { get; set; } = null;


        public ApplicationContext(DbContextOptions<ApplicationContext> options)
       : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MainData>().HasKey(o => o.SubAccountNumber);
            modelBuilder.Entity<MainAccauntNubmer>().HasKey(o => o.AccountNumber);

            base.OnModelCreating(modelBuilder);
        }

    }
}
