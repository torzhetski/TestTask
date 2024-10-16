using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Task2.DTOs;
using Task2.Entities;
using Task2.Interfaces;

namespace Task2.Pages
{
    public class DataModel : PageModel
    {
        private readonly IApplicationContext _context;

        public DataModel(IApplicationContext context)
        {
            _context = context;
        }

        public IList<MainData> Data { get; set; }
        public IList<MainAccauntNubmer> MainNumber { get; set; }
        public IList<ClassOfAccount> Classes { get; set; }
        public ClassOfAccount CurrentClass { get; set; } = new();
        public int CurrentMainNumber { get; set; }

        public async Task OnGetAsync()
        {
            Data = await _context.Data
                .Include(o => o.ClassOfAccount)
                .ToListAsync();
            MainNumber = await _context.AccauntNumbers.ToListAsync();
            Classes = await _context.Classes.ToListAsync();
        }

        public async Task<MainDataDTO> MainNumberSummary(int mainNumb)
        {
            var summary = new MainDataDTO();

            summary.InSaldoActive = Math.Round(await _context.Data
                .Where(data => data.MainAccauntNuberId == mainNumb)
                .SumAsync(data => data.InSaldoActive),2);

            summary.InSaldoPassive = Math.Round(await _context.Data
                .Where(data => data.MainAccauntNuberId == mainNumb)
                .SumAsync(data => data.InSaldoPassive),2);

            summary.OutSaldoActive = Math.Round(await _context.Data
                .Where(data => data.MainAccauntNuberId == mainNumb)
                .SumAsync(data => data.OutSaldoActive),2);

            summary.OutSaldoPassive = Math.Round(await _context.Data
                .Where(data => data.MainAccauntNuberId == mainNumb)
                .SumAsync(data => data.OutSaldoPassive),2);

            summary.TurnoverDebit = Math.Round(await _context.Data
                .Where(data => data.MainAccauntNuberId == mainNumb)
                .SumAsync(data => data.TurnoverDebit),2);

            summary.TurnoverCredit = Math.Round(await _context.Data
                .Where(data => data.MainAccauntNuberId == mainNumb)
                .SumAsync(data => data.TurnoverCredit),2);

            return summary;
        }

        public async Task<MainDataDTO> ClassSummary(int accauntClassId)
        {
            var summary = new MainDataDTO();

            summary.InSaldoActive = Math.Round(await _context.Data
                .Where(data => data.ClassOfAccountId == accauntClassId)
                .SumAsync(data => data.InSaldoActive), 2);

            summary.InSaldoPassive = Math.Round(await _context.Data
                .Where(data => data.ClassOfAccountId == accauntClassId)
                .SumAsync(data => data.InSaldoPassive), 2);

            summary.OutSaldoActive = Math.Round(await _context.Data
                .Where(data => data.ClassOfAccountId == accauntClassId)
                .SumAsync(data => data.OutSaldoActive), 2);

            summary.OutSaldoPassive = Math.Round(await _context.Data
                .Where(data => data.ClassOfAccountId == accauntClassId)
                .SumAsync(data => data.OutSaldoPassive), 2);

            summary.TurnoverDebit = Math.Round(await _context.Data
                .Where(data => data.ClassOfAccountId == accauntClassId)
                .SumAsync(data => data.TurnoverDebit), 2);

            summary.TurnoverCredit = Math.Round(await _context.Data
                .Where(data => data.ClassOfAccountId == accauntClassId)
                .SumAsync(data => data.TurnoverCredit), 2);

            return summary;
        }
    }
}

