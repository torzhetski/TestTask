using FileImport.Interfaces;
using Microsoft.Win32;

namespace FileImport.Services
{
    public class FileDialogService : IFileDialogService
    {
        public string OpenFileDialog(string filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = filter;

            return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : null;
        }
    }
}
