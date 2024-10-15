class FileMerger
{
    public void MergeFiles(string directoryPath, string outputFilePath, string filterString)
    {
        int deletedLinesCount = 0;
        //if (!File.Exists(outputFilePath)) можно добавлять если мы не хотим делать снова одно и то же дествие 
            using (StreamWriter writer = new StreamWriter(outputFilePath))
        {
            foreach (string filePath in Directory.GetFiles(directoryPath, "File_*.txt"))
            {
                foreach (string line in File.ReadLines(filePath))
                {
                    if (!line.Contains(filterString))
                    {
                        writer.WriteLine(line);
                    }
                    else
                    {
                        deletedLinesCount++;
                    }
                }
            }
        }
        Console.WriteLine($"Объединение завершено. Удалено строк: {deletedLinesCount}");
    }
}
