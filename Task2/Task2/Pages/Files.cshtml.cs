using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Task2.Entities;
using Task2.Interfaces;

namespace Task2.Pages
{
    public class FilesModel : PageModel
    {
        private readonly IApplicationContext _context;

        public FilesModel (IApplicationContext context)
        {
            _context = context;
        }

        public IList<UploadedFiles> UploadedFilesList { get; set; }

        public async Task OnGetAsync()
        {
            UploadedFilesList = await _context.Files.ToListAsync();
        }
    }
}
