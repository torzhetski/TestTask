class FileCreator
{
    private static readonly Random random = new Random();
    private static string cyrillicAlphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя";
    private static string latinAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    public void Create(string directoryPath)
    {
        for (int i = 0; i < 100; i++)
        {
            string filePath = Path.Combine(directoryPath, $"File_{i + 1}.txt");
            //if (!File.Exists(filePath)) можно добавлять если мы не хотим делать снова одно и то же дествие 
            using (StreamWriter writer = new StreamWriter(filePath,false))
            {
                for (int j = 0; j < 100000; j++)
                {
                    writer.WriteLine(GenerateRandomLine());
                }
            }
            Console.WriteLine($"File_{i+1} Создан");
        }
    }

    private string GenerateRandomLine()
    {
        DateOnly randomDate = GenerateRandomDate(5);
        string randomLatinString = GenerateRandomString(10, latinAlphabet);
        string randomCyrillicString = GenerateRandomString(10, cyrillicAlphabet);
        int randomEvenNumber = GenerateRandomEvenNumber(1, 100000000);
        decimal randomDecimal = GenerateRandomDecimal(1, 20);

        return $"{randomDate:dd.MM.yyyy}||{randomLatinString}||{randomCyrillicString}||{randomEvenNumber}||{randomDecimal:F8}||";
    }

    private DateOnly GenerateRandomDate(int yearRange)
    {
        DateTime start = DateTime.Now.AddYears(-yearRange);
        int range = (DateTime.Today - start).Days;
        return DateOnly.FromDateTime(start.AddDays(random.Next(range)));
    }

    private string GenerateRandomString(int length, string chars)
    {
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private int GenerateRandomEvenNumber(int minValue, int maxValue)
    {
        int number;
        do
        {
            number = random.Next(minValue, maxValue + 1);
        } while (number % 2 != 0);
        return number;
    }

    private decimal GenerateRandomDecimal(int minValue, int maxValue)
    {
        return Math.Round((decimal)random.NextDouble() * (maxValue - minValue) + minValue, 8);
    }
}