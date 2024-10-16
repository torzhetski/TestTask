using Microsoft.EntityFrameworkCore;
using Task2.Entities;

namespace Task2.Interfaces
{
    public interface IApplicationContext
    {
        DbSet<MainAccauntNubmer> AccauntNumbers { get; set; }
        DbSet<ClassOfAccount> Classes { get; set; }
        DbSet<MainData> Data { get; set; }
        DbSet<UploadedFiles> Files { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}