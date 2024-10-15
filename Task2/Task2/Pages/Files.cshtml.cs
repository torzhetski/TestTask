using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Task2.Entities;

namespace Task2.Pages
{
    public class FilesModel : PageModel
    {
        private readonly ApplicationContext _context;

        public FilesModel (ApplicationContext context)
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
