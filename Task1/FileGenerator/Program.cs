class Program
{
    static void Main(string[] args)
    {
        string directoryPath = "GeneratedFiles";
        Directory.CreateDirectory(directoryPath);

        FileCreator fileCreator = new FileCreator();
        fileCreator.Create(directoryPath);
        Console.WriteLine("Все файлы созданы! Для слияния нажмите Enter");
        Console.ReadLine();

        
        FileMerger fileMerger = new FileMerger();
        string filterString = "abc";
        string outputFilePath = Path.Combine(directoryPath, "MergedFile_"+filterString+".txt");
        fileMerger.MergeFiles(directoryPath, outputFilePath, filterString);
    }
}
