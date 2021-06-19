using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N4islo
{
    class Program
    {
        static void Main(string[] args)
        {
            string N = Console.ReadLine();//добавления значения
            uint n = uint.Parse(N);//конвертирования
            int[][] muss = GroupsNumbersArr(n);//работа с методом
            foreach(int[] i in muss)//перебор массива
            {
                Console.WriteLine(i);
            }
           


            Console.ReadKey();

        }
        public static int[][] GroupsNumbersArr(uint number)
        {

            /// Если переданное число ноль, то возвращается пустой список групп
            if (number == 0)
            {
                return Array.Empty<int[]>();
            }
            /// Если переданное число единица, то возвращается список групп с одной группой - единицей
            if (number == 1)
            {
                return new int[][] { new int[] { 1 } };
            }
            /// Создание массива для групп
            int[][] groups = new int[(int)Math.Log(number, 2) + 1][];
            groups[0] = new int[] { 1 };
            int indexGroup = 1; // Индекс добавляемой группы

            /// Создание массива чисел содержащего все числа от 1 до заданного
            /// Единица используется как маркер
            /// Вместо удаления элеменов их значение будет приравниваться нулю
            /// После сортировки 1 будет разделять удалённые элементы и оставшиеся
            int[] numbers = new int[number];
            for (int i = 0; i < number; i++)
            {
                numbers[i] = i + 1;
            }    
            
            int[] group = new int[number];//Массив с промежуточными данными


            int index1;//Цикл пока в массиве индекс единицы не последений
            while ((index1 = Array.BinarySearch(numbers, 1)) != number - 1) /// Проверка индекса единицы
            {

                Array.Copy(numbers, group, number);//Копия элементов в массив группы

                int countGroup = 0;// Количество элементов в группе
                                    
                for (int i = index1 + 1; i < number; i++)//Перебор элементов группы. i - индекс проверяемого элемента
                {
                    if (group[i] != 0) /// Пропуск удалённых элементов
                    {
                        /// Удаление из группы всех элементов кратных проверяемому, кроме его самого
                        for (int j = i + 1; j < number; j++)
                            if (group[j] % group[i] == 0)
                                group[j] = 0;

                        
                        numbers[i] = 0;//Удаление элемента из массива чисел

                        countGroup++;//Счётчик группы увеличивется
                    }

                }
                /// Сортировка массивов после удаления элементов
                Array.Sort(group);
                Array.Sort(numbers);

                
                
                int[] _gr = new int[countGroup];//Создание массива для добавления в группы
                Array.Copy(group, Array.BinarySearch(group, 1) + 1, _gr, 0, countGroup);//копирование в него значений старше 1

                /// Добавление группы в массив групп
                groups[indexGroup] = _gr;
                indexGroup++;

            }
            
            return groups;//Возврат списка групп
        }
    }
}
