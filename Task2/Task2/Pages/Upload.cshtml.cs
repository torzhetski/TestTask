using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using Task2.Entities;
using Task2.Interfaces;

namespace Task2.Pages
{
    public class UploadModel : PageModel
    {
        private readonly IApplicationContext _context;

        public UploadModel(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnPostAsync(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                ModelState.AddModelError("File", "Please upload a valid Excel file.");
                return RedirectToPage("Files");
            }
            if (_context.Files.Any(o => o.Name == excelFile.FileName))
            {
                ModelState.AddModelError("File", "This file has already been uploaded.");
                return Page();
            }
            else
            {
                _context.Files.Add( new UploadedFiles() 
                {
                    Name = excelFile.FileName, 
                    CreateDate = DateTime.Now 
                });

                await _context.SaveChangesAsync();

                using (var stream = new MemoryStream())
                {
                    await excelFile.CopyToAsync(stream);
                    await UploadAsync(stream, 9);
                }

                return RedirectToPage("Data");
            }

            
        }

        /// <summary>
        /// метод загружающий данные из файла формата .xlsx(не работает для старого формата .xls)
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="startRow"></param>
        /// <returns></returns>
        private async Task UploadAsync(MemoryStream stream, int startRow)
        {
            stream.Position = 0;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                ClassOfAccount classOfAccount = new();
                MainAccauntNubmer mainNumber = new();
                MainData mainData = new();

                for (int row = startRow; row <= rowCount; row++)
                {
                    if (worksheet.Cells[row, 1].Value == null)
                        continue;

                    // так как строки с именем класса обьедененные проверяем на обьединение
                    if (worksheet.Cells[row, 1].Merge)
                    {
                        classOfAccount = new();
                        classOfAccount.Name = worksheet.Cells[row, 1].Value.ToString();
                        _context.Classes.Add(classOfAccount);
                        continue;
                    }
                    // получаем номер главного счета
                    int subAccountNumber;
                    if (int.TryParse(worksheet.Cells[row, 1].Value.ToString(), out subAccountNumber))
                    {
                        int mainNumb = subAccountNumber / 100;
                        if (mainNumb != 0)
                        {
                            if (mainNumber.AccountNumber != mainNumb)
                            {
                                mainNumber = new();
                                mainNumber.AccountNumber = mainNumb;
                                _context.AccauntNumbers.Add(mainNumber);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                    await _context.SaveChangesAsync();

                    //заполняем основную информацию
                    mainData = new();
                    mainData.SubAccountNumber = subAccountNumber;
                    mainData.InSaldoActive = double.TryParse(worksheet.Cells[row, 2].Text, out var temp1) ? temp1 : 0;
                    mainData.InSaldoPassive = double.TryParse(worksheet.Cells[row, 3].Text, out var temp2) ? temp2 : 0;
                    mainData.TurnoverDebit = double.TryParse(worksheet.Cells[row, 4].Text, out var temp3) ? temp3 : 0;
                    mainData.TurnoverCredit = double.TryParse(worksheet.Cells[row, 5].Text, out var temp4) ? temp4 : 0;
                    mainData.OutSaldoActive = double.TryParse(worksheet.Cells[row, 6].Text, out var temp5) ? temp5 : 0;
                    mainData.OutSaldoPassive = double.TryParse(worksheet.Cells[row, 7].Text, out var temp6) ? temp6 : 0;
                    mainData.ClassOfAccountId = classOfAccount.Id;
                    mainData.MainAccauntNuberId = mainNumber.AccountNumber;

                    _context.Data.Add(mainData);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
