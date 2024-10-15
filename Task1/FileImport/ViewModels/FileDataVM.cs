using FileImport.Interfaces;
using MySql.Data.MySqlClient;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace FileImport.ViewModels
{
    class FileDataVM : ObservableObject
    {
        private readonly IFileDialogService _fileDialogService;
        private string _connectionString = $"Server=localhost;Database=testdb;User Id=root;Password={Environment.GetEnvironmentVariable("SQLPassword")};";

        public FileDataVM(IFileDialogService fileDialogService)
        {
            _fileDialogService = fileDialogService;
        }

        private string _selectedFilePath;
        public string SelectedFilePath
        {
            get => _selectedFilePath;
            set => Set(ref _selectedFilePath, value);
        }

        private bool _isVisible = false;
        public bool IsVisible
        {
            get => _isVisible;
            set => Set(ref _isVisible, value);
        }

        private int _importedCount;
        public int ImportedCount
        {
            get => _importedCount;
            set => Set(ref _importedCount, value);
        }
        private int _totalLines;
        public int TotalLines
        {
            get => _totalLines;
            set => Set(ref _totalLines, value);
        }

        public ICommand SelectFileCommand => new RelayCommand(o =>
        {
            SelectFile();
        });

        public ICommand ImportFileCommand => new RelayCommand(async o =>
        {
            await ImportFileAsync();
        }, o =>
        {
            return !string.IsNullOrEmpty(SelectedFilePath);
        });
        public ICommand SumAndMedianCommand => new RelayCommand(async o => 
        {
            await GetSumAndMedianAsync();
        });


        private void SelectFile()
        {
            string filePath = _fileDialogService.OpenFileDialog("Text files (*.txt)|*.txt|All files (*.*)|*.*");
            if (filePath != null)
            {
                SelectedFilePath = filePath;
            }
        }

        private async Task ImportFileAsync()
        {

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                await CreateTableAsync(connection);
                string[] lines = File.ReadAllLines(_selectedFilePath);
                TotalLines = lines.Length;
                ImportedCount = 0;
                IsVisible = true;


                using (MySqlCommand command = connection.CreateCommand())
                {
                    try
                    {
                        command.CommandText = "INSERT INTO ImportedData (ImportDate, LatinString, CyrillicString, EvenInteger, DecimalNumber) VALUES (@date, @latin, @cyrillic, @evenInt, @decimalNum)";
                        command.Parameters.Add("@date", MySqlDbType.Date);
                        command.Parameters.Add("@latin", MySqlDbType.VarChar);
                        command.Parameters.Add("@cyrillic", MySqlDbType.VarChar);
                        command.Parameters.Add("@evenInt", MySqlDbType.Int32);
                        command.Parameters.Add("@decimalNum", MySqlDbType.Decimal);

                        foreach (string line in lines)
                        {
                            string[] parts = line.Split("||", StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length == 5)
                            {
                                command.Parameters["@date"].Value = DateOnly.Parse(parts[0], new CultureInfo("ru-RU"));
                                command.Parameters["@latin"].Value = parts[1];
                                command.Parameters["@cyrillic"].Value = parts[2];
                                command.Parameters["@evenInt"].Value = int.Parse(parts[3]);
                                command.Parameters["@decimalNum"].Value = decimal.Parse(parts[4]);

                                await command.ExecuteNonQueryAsync();
                                ImportedCount++;
                            }
                        }
                        MessageBox.Show("Импорт данных прошел успешно!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при импорте данных: {ex.Message}");
                    }
                    finally
                    {
                        connection.Close();
                        IsVisible = false;

                    }
                }
            }
        }

        private async Task CreateTableAsync(MySqlConnection connection)
        {
            try
            {
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS ImportedData (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        ImportDate DATE,
                        LatinString VARCHAR(50),
                        CyrillicString VARCHAR(50),
                        EvenInteger INT,
                        DecimalNumber DECIMAL(10, 8)
                    );";

                using (MySqlCommand createTableCommand = new MySqlCommand(createTableQuery, connection))
                {
                    await createTableCommand.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании таблицы: {ex.Message}");
            }
        }

        private async Task GetSumAndMedianAsync()
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand("CalculateSumAndMedian", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        MySqlParameter totalSumParam = new MySqlParameter("@totalSum", MySqlDbType.Int64)
                        {
                            Direction = System.Data.ParameterDirection.Output
                        };
                        MySqlParameter medianDecimalParam = new MySqlParameter("@medianDecimal", MySqlDbType.Decimal)
                        {
                            Direction = System.Data.ParameterDirection.Output,
                            Precision = 10,
                            Scale = 8
                        };


                        command.Parameters.Add(totalSumParam);
                        command.Parameters.Add(medianDecimalParam);

                        await command.ExecuteNonQueryAsync();

                        long totalSum = (long)totalSumParam.Value;
                        decimal medianDecimal = (decimal)medianDecimalParam.Value;


                        MessageBox.Show($"Сумма чисел в строках: {totalSum}\nМедиана дробных значений в строках: {medianDecimal}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при выполнении хранимой процедуры: " + ex.Message);
                }
            }
        }
    } 
}

