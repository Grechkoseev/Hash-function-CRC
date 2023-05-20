namespace Lab_5;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(
            "Программа реализует вычисляющую хэш-функцию CRC для сообщения длиной 1 байт с порождающим многочленом 1+x+x^2+x^3\n");

        var generatingPolynomial = new List<int> { 0, 0, 0, 0, 0, 1, 1, 1 };
        var hashFunction = new HashFunctionCrc(generatingPolynomial);
        var keyValuePairsList = new List<KeyValuePair<int, int>>();
        
        for (var  i = 0; i < 256; i++)
        {
            var hash = hashFunction.GetHash(i);

            if (keyValuePairsList.Any(x => x.Key == hash))
            {
                var item = keyValuePairsList.FirstOrDefault(x => x.Key == hash);
                var changedItem = new KeyValuePair<int, int>(hash, item.Value + 1);
                keyValuePairsList.Remove(item);
                keyValuePairsList.Add(changedItem);
            }
            else
            {
                keyValuePairsList.Add(new KeyValuePair<int, int>(hash, 0));
            }

            Console.WriteLine("Хэш {0} равен {1}", i, hash);
        }
        
        Console.WriteLine();

        foreach (var keyValuePair in keyValuePairsList)
        {
            Console.WriteLine("Хэш {0} имеет {1} совпадений", keyValuePair.Key, keyValuePair.Value);
        }

        Console.ReadKey();
    }
}