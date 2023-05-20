namespace Lab_5;

internal class HashFunctionCrc
{
    private readonly List<int> _generatingPolynomial;
    private readonly int _n;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="generatingPolynomial"></param>
    public HashFunctionCrc(List<int> generatingPolynomial)
    {
        _generatingPolynomial = new List<int>(generatingPolynomial);

        for (var i = 0; i < _generatingPolynomial.Count; i++)
        {
            if (_generatingPolynomial[i] == 1)
            {
                _n = _generatingPolynomial.Count - i;
                break;
            }
        }
    }

    /// <summary>
    /// Переводим десятичное число в список с двоичным его представлением
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    private static List<int> ConvertNumberToBinaryList(int number)
    {
        var binaryList = new List<int>(8);
        var binaryNumber = Convert.ToString(number, 2);

        for (var i = binaryList.Capacity; i > 0; i--)
        {
            binaryList.Add(i <= binaryNumber.Length ? int.Parse(binaryNumber[^i].ToString()) : 0);
        }

        return binaryList;
    }

    /// <summary>
    /// Выполнение деления многочленов столбиком
    /// </summary>
    /// <param name="number" число от 0 до 255, хэш которого будет вычислен></param>
    /// <returns></returns>
    public int GetHash(int number)
    {
        var binaryNumber = ConvertNumberToBinaryList(number);
        var dividend = new List<int>(binaryNumber);
        var tempQuotient = new List<int>();
        var tempDividend = new List<int>();
        var divisor = new List<int>(_generatingPolynomial);
        var result = string.Empty;

        while (divisor[0] == 0)
        {
            divisor.RemoveAt(0);
        }

        for (var i = 0; i < _n; i++)
        {
            dividend.Add(0);
        }

        for (var j = 0; j < dividend.Count;)
        {
            tempQuotient.Clear();
            tempQuotient = Xor(tempDividend, divisor);
            DeleteFirstZeros(ref tempQuotient);
            tempDividend = GetTempDividend(ref j, dividend, tempQuotient);
        }

        result = tempQuotient.Aggregate(result, (current, i) => current + i);

        return result == string.Empty ? 0 : int.Parse(result);
    }

    /// <summary>
    /// Побитовый xor двух списков, содержащих биты
    /// Метод используется при делении столбиком
    /// </summary>
    /// <param name="firstList"></param>
    /// <param name="secondList"></param>
    /// <returns>Список битов после побитового xor-a двух списков с битами</returns>
    private static List<int> Xor(List<int> firstList, List<int> secondList)
    {
        return firstList.Select((t, i) => t ^ secondList[i]).ToList();
    }

    /// <summary>
    /// Удаление первых нулей списка
    /// Метод используется при делении столбиком
    /// </summary>
    /// <param name="list"></param>
    private static void DeleteFirstZeros(ref List<int> list)
    {
        while (list.Count > 0 && list[0] == 0)
        {
            list.RemoveAt(0);
        }
    }

    /// <summary>
    /// Дополняем результат xor-a следующими битами из входной последовательности
    /// Метод используется при делении столбиком
    /// </summary>
    /// <param name="i"></param>
    /// <param name="fullDividend"></param>
    /// <param name="quotient"></param>
    /// <returns></returns>
    private List<int> GetTempDividend(ref int i, List<int> fullDividend, List<int> quotient)
    {
        if (quotient.Count == 0)
        {
            while (quotient.Count == 0 && fullDividend.Count > i)
            {
                if (fullDividend[i] != 0)
                {
                    quotient.Add(fullDividend[i]);
                }

                i++;
            }

            while (quotient.Count != _n && fullDividend.Count > i)
            {
                quotient.Add(fullDividend[i]);
                i++;
            }
        }
        else
        {
            while (quotient.Count != _n && fullDividend.Count > i)
            {
                quotient.Add(fullDividend[i]);
                i++;
            }
        }

        var result = new List<int>();
        result.AddRange(quotient);

        return result;
    }
}