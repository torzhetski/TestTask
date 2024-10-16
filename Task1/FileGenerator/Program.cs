class Program
{
    static void Main(string[] args)
    {
        //создаем директорию если ее еще нет
        string directoryPath = "GeneratedFiles";
        Directory.CreateDirectory(directoryPath);
        //запускаем создание фалов
        FileCreator fileCreator = new FileCreator();
        fileCreator.Create(directoryPath);
        Console.WriteLine("Все файлы созданы! Для слияния нажмите Enter");
        Console.ReadLine();
        //запускаем слияние файлов с удалением строк с указанным фильтром
        FileMerger fileMerger = new FileMerger();
        string filterString = "abc";
        string outputFilePath = Path.Combine(directoryPath, "MergedFile_"+filterString+".txt");
        fileMerger.MergeFiles(directoryPath, outputFilePath, filterString);
    }
}
