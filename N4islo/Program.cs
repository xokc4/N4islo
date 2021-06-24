using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Collections;

namespace N4islo
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFile = @"inputNumber.txt";
            IsInputFile(inputFile);
            Console.WriteLine("Загружаю число из файла...");
            int number = ReadFile(inputFile);                           //Считываю txt файл
            int[][] result = new int[ReturnGroupCount(number)][];       //Массив массивов для хранения последовательностей чисел
            result[0] = new int[] { 1 };                                //Первая последовательность = 1
            Stopwatch sv = new Stopwatch();                             //Таймер для замера времени выполнения задачи

            Console.WriteLine("Внимание! Число в файле = " + number);
            Console.WriteLine();
            Console.WriteLine(
                "1 - Показать только кол-во групп\n" +
                "2 - Выполнить алгоритм подсчета групп чисел методом перебора значений\n" +
                "с последующей архивацией или сохранением в файл\n");
                
            switch (Console.ReadLine())
            {
                #region case "1": //Показать только кол-во групп
                case "1":
                    int[] range = new int[number - 2];
                    sv.Start();
                    Console.WriteLine("Групп чисел: " + ReturnGroupCount(number));
                    sv.Stop();
                    PrintTime(sv);
                    break;
                #endregion

                #region case "2": //Выполнить алгоритм подсчета групп чисел методом перебора значений
                case "2":
                    sv.Start();
                    Console.WriteLine("Формирую массив чисел от 1 до вашего числа");
                    range = new int[number - 2];
                    range = Enumerable.Range(2, number).ToArray();

                    for (int i = 1; i < result.Length; i++)
                    {
                        result[i] = ReturnList(range);             //  Находим требуемую последовательность
                        range = range.Except<int>(result[i]).ToArray();    //  Вычитаем из нашей последовательности полученную последовательность
                    }
                    sv.Stop();
                    PrintTime(sv);

                    Console.WriteLine("Производим запись результатов в файл output2.txt");
                    string file = "output2.txt";
                    SaveToFile(file, result);

                    Console.WriteLine("Сохранение в файл завершено!\n");
                    Console.WriteLine("Заархивировать полученный файл? \n" +
                        "1 - Да  " +
                         "0 - Нет, просто выйти из программы");
                    switch (Console.ReadLine())
                    {
                        case "1":
                            Compressig(file, "archivedOutput2.zip");
                            Console.WriteLine("Файл заархивирован. Ищите его в папке с exe файлом");
                            break;
                        case "0":
                            break;
                    }
                    break;
                #endregion

                
            }
            Console.ReadKey();
        }
        static int ReadFile(string path)
        {
            using (StreamReader sr = new StreamReader(path, Encoding.Default))
            {
                return int.Parse(sr.ReadLine());
            }
        }//Считывание числа из файла
        static void IsInputFile(string path)
        {
            Random random = new Random();
            int r = random.Next(50, 1000);
            if (!File.Exists(path))
            {
                Console.WriteLine($"Файл данных inputNumber.txt не найден. Создаю файл по умолчанию");
                using (StreamWriter sw = new StreamWriter(new FileStream("inputNumber.txt", FileMode.Create, FileAccess.Write)))
                {
                    sw.WriteLine(r);
                }
            }
        }//Проверка существования файла ввода
        static void Compressig(string source, string output)
        {
            if (!File.Exists(source)) Console.WriteLine("Файл не существует! Создайте файл для архивации или проверьте правильность пути");
            else
            {
                using (FileStream fs = new FileStream(source, FileMode.Open))
                {
                    using (FileStream nf = File.Create(output))
                    {
                        using (GZipStream gs = new GZipStream(nf, CompressionMode.Compress))
                        {
                            fs.CopyTo(nf);
                        }
                    }
                }
            }
        }//Архивация файла стандартными средствами
        static void SaveToFile(string name, int[][] array)
        {
            string fileName = name;
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            using (StreamWriter sw = new StreamWriter(new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write)))
            {
                for (int i = 0; i < array.Length; i++)
                {
                    sw.WriteLine(PrintArray(array[i]));
                    sw.Flush();
                }
            }
        }//Сохранение результата вычисления в файл

        /// <summary>
        /// Вычисляет последовательность простых чисел методом "Решето Эрастофена"
        /// </summary>
        /// <param name="list">Исходный массив битов. ненужные элементы заменябтся на true</param>
        /// <returns>Возвращает массив, где false = простые числа</returns>
        static BitArray ErastofenBool(BitArray list)
        {
            BitArray arr = new BitArray(list);
            for (int i = 2; i * 2 < arr.Length; i++)
            {
                if (!list[i])
                {
                    for (int j = i * 2; j < arr.Length; j += i)
                    {
                        arr[j] = true;
                    }
                }
            }
            return arr;
        }

        /// <summary>
        /// Возвращает последовательность чисел не делящихся друг на друга
        /// </summary>
        /// <param name="array">Исходный массив для выделения последовательности</param>
        /// <returns></returns>
        static int[] ReturnList(int[] array)
        {
            int[] buffArray = new int[array.Length];
            array.CopyTo(buffArray, 0);
            bool flag = true;
            for (int i = 0; i * 2 < buffArray.Length; i++)
            {
                if (buffArray[i] != 0)
                {
                    for (int j = i; j < buffArray.Length; j++)
                    {
                        if (buffArray[j] % buffArray[i] == 0 && buffArray[j] / buffArray[i] > 1)
                        {
                            buffArray[j] = 0;
                            flag = false;
                        }
                    }
                }
            }
            if (!flag) return ReturnList(buffArray);
            else return buffArray.Where(x => x != 0).ToArray();        //  Записываем последовательнсть в массив, пропуская нули
        }
        static void PrintTime(Stopwatch sv)
        {
            Console.WriteLine("Время выполнения алгоритма вычисления: " + sv.Elapsed.TotalSeconds + " сек.");
        }//Метод выводящий строку с замером времени выполнения алгоритма в секундах.
        static string PrintArray(int[] array)
        {
            string result = null;
            foreach (int item in array)
            {
                result += item + " ";
            }
            return result;
        }//Преобразует одномерный массив в строку
        static int ReturnGroupCount(int number)
        {
            int count = 1;
            while (number > 1)
            {
                number /= 2;
                count++;
            }
            return count;
        }//Вычисляем количество групп чисел, не делящихся друг на друга
    }
}
