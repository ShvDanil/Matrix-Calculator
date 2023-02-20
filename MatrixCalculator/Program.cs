using System;
using System.Globalization;
/*
Информация: Это консольное приложение "Калькулятор матриц", которое может выполнять над матрицами следующие операции:
1) нахождение следа матрицы;
2) транспонирование матрицы;
3) сумма двух матриц;
4) разность двух матриц;
5) произведение двух матриц;
6) умножение матрицы на число;
7) нахождение определителя матрицы;
8) решение СЛАУ с помощью метода Крамера.
Данное консольное приложение разработано для C# .NET 5.0. Тип проекта - Console Application .NET 5.0. 
Дисциплина: "Программирование".
Группа: БПИ21"рандомное число от 1 до 11".
Студент: Анонимный аноним.
Дата дедлайна: Вторник, 6 октября, 23:59.
*/
namespace MatrixCalculator
{
    /*
        Основной класс Program.
        Содержит все методы, необходимые для работы консольного приложения "Калькулятор матриц". 
    */
    class Program
    {
        /// <summary>
        /// Данный метод предлагает пользователю нажать "ENTER" для продолжения действий.
        /// </summary>
        private static void ContinueButton()
        {
            ConsoleKeyInfo localKeyToContinue;
            // Цикл с пост-условием, который предлагает нажать "ENTER" и продолжить действия.
            do
            {
                Console.WriteLine("Нажмите \"ENTER\", чтобы продолжить.");
                localKeyToContinue = Console.ReadKey();
                Console.WriteLine();

                // Сообщение пользователю о нажатии неверной кнопки и предложение повторить действие.
                if (localKeyToContinue.Key != ConsoleKey.Enter)
                {
                    Console.WriteLine("Вы нажали неправильную кнопку!");
                }
            } while (localKeyToContinue.Key != ConsoleKey.Enter);
        }
      
        /// <summary>
        /// Данный метод проверяет имя пользователя на корректность.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <returns>
        /// Возвращает bool значение: true - если имя содержит только буквы кириллицы, false - в противном случае.
        /// </returns>
        private static bool UserNameCheck(string userName)
        {
            // Константа, содержащая все буквы кириллицы в нижнем регистре.
            const string lowerCaseCyrillicCharacters = "абвгдеёжзийклмнопрстуфхцчшщьыъэюя";
            
            var flag = true;
            // Цикл, проверяющий каждый элемент имени пользователя на принадлежность кириллице.
            for (var i = 0; i < userName.Length; i++)
            {
                if (!lowerCaseCyrillicCharacters.Contains(userName.ToLower()[i]))
                {
                    flag = false;
                    break;
                }
            }

            if (string.IsNullOrEmpty(userName))
                return !flag;
            
            return flag;
        }

        /// <summary>
        /// Данный метод запрашивает имя пользователя, пока оно не будет корректным, и приводит его к следующему виду:
        /// первая буква заглавная, остальные - строчные.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        private static void UserNameInput(ref string userName)
        {
            // Цикл с пост-условием, который просит пользователя ввести его имя, пока оно не будет корректным.
            do
            {
                userName = Console.ReadLine();
                if (UserNameCheck(userName) == false)
                {
                    Console.Write("Ваш ввод некорректен! Попробуйте заново! Введите ваше имя: ");
                    continue;
                }
                break;

            } while (true);
            
            userName = userName.ToUpper()[0] + userName.ToLower()[1..];
        }
        
        /// <summary>
        /// Данный метод приветствует пользователя и кратко рассказывает о возможностях калькулятора.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        private static void UserGreeting(ref string userName)
        {
            Console.Write("Здравствуйте! Рад приветствовать вас в консольном приложении \"Калькулятор матриц\".\n" +
                          "Для начала предлагаю познакомиться.\nВведите ваше имя (оно должно содержать только " +
                          "буквы русского алфавита): ");

            // Ввод имени пользователя.
            UserNameInput(ref userName);
            
            Console.WriteLine($"\nПриятно познакомиться, {userName}! Теперь расскажу, что способен делать данный " +
                              "калькулятор.\nКратко: вы выбираете операцию, которую хотите выполнить, а затем " +
                              "указываете тип ввода, при котором матрицы будут либо вводиться вами, либо " +
                              "генерироваться компьютером.\n");
            
            ContinueButton();
        }

        /// <summary>
        /// Данный метод показывает операции, которые доступны для выполнения.
        /// </summary>
        private static void CalculatorOperationsDescription()
        {
            Console.WriteLine("Для начала хочется напомнить: матрицей размера m x n называется упорядоченная " +
                              "прямоугольная таблица, содержащая m строк и n столбцов.");

            Console.WriteLine("Теперь расскажу про доступные операции над матрицами в этом калькуляторе:\n" +
                              "1) нахождение следа матрицы;\n" +
                              "2) транспонирование матрицы;\n" +
                              "3) сумма двух матриц;\n" +
                              "4) разность двух матриц;\n" +
                              "5) произведение двух матриц;\n" +
                              "6) умножение матрицы на число;\n" +
                              "7) нахождение определителя матрицы;\n" +
                              "8) решение СЛАУ (системы линейных алгебраических уравнений) методом Крамера.\n");

            ContinueButton();
        }

        /// <summary>
        /// Данный метод предлагает пользователю выбрать операцию, которая будет выполнена над матрицами.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <returns>
        /// Возвращает целое число от 1 до 8 (в типе byte для экономии памяти), которое является номером операции,
        /// выбранной пользователем. 
        /// </returns>
        private static byte UsersOperationChoice(string userName)
        {
            Console.Write($"{userName}, cейчас вам необходимо выбрать одну из восьми доступных операций.\nДля этого " +
                          "нажмите цифру от 1 до 8 (без незначащих нулей и пробелов перед цифрой), " +
                          "соответствующую номеру операции, которую вы хотите выполнить: ");
            
            byte operation;
            // Цикл с пост-условием, который просит ввести номер операции, пока он не будет корректен.
            do
            {
                var operationString = Console.ReadLine();
                if (!byte.TryParse(operationString, out operation) || (operation is (< 1 or > 8)) || 
                    operationString.StartsWith("0") || operationString.StartsWith(" "))
                {
                    Console.Write("Вы указали неправильную операцию, повторите попытку: ");
                    continue;
                }
                break;
                
            } while (true);
            
            return operation;
        }

        /// <summary>
        /// Данный метод предлагает пользователю варианты ввода данных для выполнения основных операций.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <returns>
        /// Возвращает целое значение (типа byte) "1", если пользователь будет вводить данные вручную, "2", если
        /// данные будут генерироваться компьютером автоматически по заданным размерам.
        /// </returns>
        private static byte InputChoice(string userName)
        {
            Console.WriteLine($"\n{userName}, cейчас вам необходимо выбрать один из двух вариантов ввода данных:\n" +
                              "1) введение матриц самостоятельно, вручную (укажите цифру 1);\n" +
                              "2) генерация матриц компьютером (укажите цифру 2);\n");
                          Console.Write("Укажите цифру 1 или 2 (без незначащих нулей и пробелов перед ней): ");
            
            byte inputChoice;
            // Цикл с пост-условием, который предлагает ввести вариант ввода, пока он не будет корректным.
            do
            {
                var inputChoiceString = Console.ReadLine();
                if (!byte.TryParse(inputChoiceString, out inputChoice) || inputChoice is (< 1 or > 2) || 
                    inputChoiceString.StartsWith("0") || inputChoiceString.StartsWith(" "))
                {
                    Console.Write("Вы ввели неправильную цифру, повторите попытку: ");
                    continue;
                }
                break;
                
            } while (true);
            
            return inputChoice;
        }

        /// <summary>
        /// Данный метод вычисляет самый длинный элемент в матрице..
        /// </summary>
        /// <param name="array">Двумерный массив.</param>
        /// <returns>Возвращает значение самого длинного элемента.</returns>
        private static int MaxLengthInMatrix(double[,] array)
        {
            var maxLength = 1;
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i, j].ToString(CultureInfo.InvariantCulture).Length > maxLength)
                        maxLength = array[i, j].ToString(CultureInfo.InvariantCulture).Length;
                }
            }

            return maxLength;
        }
        
        /// <summary>
        /// Данный метод выводит на экран матрицу.
        /// </summary>
        /// <param name="array">Двумерный массив.</param>
        private static void PrintMatrix(double[,] array)
        {
            // Вычисление значения самого длинного элемента матрицы.
            var maxLength = MaxLengthInMatrix(array);

            for (var i = 0; i < array.GetLength(0); i++, Console.WriteLine())
            {
                for (var j = 0; j < array.GetLength(1); j++)
                {
                    Console.Write(
                        Math.Round(array[i, j], 3).ToString(CultureInfo.InvariantCulture).PadRight(maxLength + 3)
                                 );
                }
            }
        }

        /// <summary>
        /// Данный метод получает размер квадратной матрицы. 
        /// </summary>
        /// <param name="userOperation">Операция, выбранная пользователем, над которой делают преобразования.</param>
        /// <returns>Возвращает размерность квадратной матрицы.</returns>
        private static (byte n, byte n1, byte n2) SquareMatrixSize(byte userOperation)
        {
            // Ввод размеров квадратной матрицы.
            // Доступные размеры: от 1 до 15 (для определителя и метода Крамера максимум 10).
            if (userOperation == 7)
                Console.Write("Введите размер вашей квадратной матрицы n x n (от 1 до 10): ");    
            else if (userOperation == 8)
                Console.Write("Введите размер вашей квадратной матрицы n x n (от 1 до 9): ");
            else
                Console.Write("Введите размер вашей квадратной матрицы n x n (от 1 до 15): ");
            do
            {
                byte n;
                var nString = Console.ReadLine();
                if (!byte.TryParse(nString, out n) || n is (< 1 or > 15) ||
                    (userOperation == 7 && n > 10) || (userOperation == 8 && n > 9) ||
                    nString.StartsWith(" ") || nString.StartsWith("0"))
                {
                    Console.Write("Неверный ввод! Введите число n заново: ");
                    continue;
                }
                return (n, n, n);
            } while (true);
        }

        /// <summary>
        /// Данный метод получает количество столбцов при умножении матриц.
        /// </summary>
        /// <param name="m">Число строк первой матрицы.</param>
        /// <param name="n">Число столбцов первой матрицы и строк первой матрицы.</param>
        /// <returns>Возвращает количество столбцов при умножении матриц.</returns>
        private static (byte m, byte n, byte k) MultiplicationInputCase(byte m, byte n)
        {
            byte k;
            Console.Write("Введите количество столбцов k матрицы (от 1 до 15): ");
            do
            {
                var kString = Console.ReadLine();
                if (!byte.TryParse(kString, out k) || k is (< 1 or > 15) || kString.StartsWith(" ") ||
                    kString.StartsWith("0"))
                {
                    Console.Write("Неверный ввод! Введите число k заново: ");
                    continue;
                }
                return (m, n, k);
            } while (true);
        }
        
        /// <summary>
        /// Данный метод получает размер прямоугольной матрицы.
        /// </summary>
        /// <param name="userOperation">Операция, выбранная пользователем, над которой делают преобразования.</param>
        /// <returns>Возвращает размерность прямоугольной матрицы.</returns>
        private static (byte m, byte n, byte k) RectangularMatrixSize(byte userOperation)
        {
            // Ввод количества строк прямоугольной матрицы. Доступные размеры: от 1 до 15.
            Console.Write("Введите количество строк m прямоугольной матрицы (от 1 до 15): ");
            byte m;
            do
            {
                var mString = Console.ReadLine();
                if (!byte.TryParse(mString, out m) || m is (< 1 or > 15) || mString.StartsWith(" ") || 
                    mString.StartsWith("0"))
                {
                    Console.Write("Неверный ввод! Введите число m заново: ");
                    continue;
                }
                break;
            } while (true);
            // Ввод количества столбцов первой прямоугольной матрицы и количества строк второй прямоугольной
            // матрицы (данное условие необходимо при умножении матриц) или же ввод столбцов прямоугольной матрицы.
            // Доступные размеры по-прежнему от 1 до 15.
            var tmpString = (userOperation == 5) ? 
                ("Введите количество столбцов n матрицы A и строк n матрицы B (от 1 до 15): ") : 
                ("Введите количество столбцов n матрицы (от 1 до 15): ");
            Console.Write(tmpString);
            byte n;
            do
            {
                var nString = Console.ReadLine();
                if (!byte.TryParse(nString, out n) || n is (< 1 or > 15) || nString.StartsWith(" ") ||
                    nString.StartsWith("0"))
                {
                    Console.Write("Неверный ввод! Введите число n заново: ");
                    continue;
                }
                break;
            } while (true);
            // Данный ввод необходим только для операции умножения, и он запрашивает ввод количества столбцов
            // прямоугольной матрицы, размеров от 1 до 15.
            if (userOperation == 5)
                return MultiplicationInputCase(m, n);
            return (m, n, n);
        }

        /// <summary>
        /// Данный метод запрашивает данные о размерах матриц.
        /// </summary>
        /// <param name="userOperation">Операция, выбранная пользователем, над которой делают преобразования.</param>
        /// <returns>
        /// Возвращает кортеж из трех элементов (типа byte), которые показывают введенную размерность матриц (значения
        /// варьируются от 1 до 15 или от 1 до 10).
        /// </returns>
        private static (byte m, byte n, byte k) MatrixSizeInput(byte userOperation)
        {
            var size = (userOperation is (1 or 7 or 8)) ? 
                SquareMatrixSize(userOperation) : 
                RectangularMatrixSize(userOperation);
            
            return size;
        }
        
        /// <summary>
        /// Данный метод запрашивает ввод матрицы размера m x n.
        /// </summary>
        /// <param name="m">Размерность матрицы (количество строк).</param>
        /// <param name="n">Размерность матрицы (количество столбцов).</param>
        /// <returns>
        /// Возвращает матрицу с элементами типа double, значения которых находятся в диапаозне от -500 до 500.
        /// </returns>
        private static double[,] MatrixKeyboardInput(byte m, byte n)
        {
            var array = new double[m, n];
            Console.WriteLine("Введите элементы матрицы от -500 до 500 через пробел построчно (без незначащих " +
                              "нулей и пробелов перед ними):");
            for (var i = 0; i < m; i++)
            {
                // Цикл с пост-условием, который работает, пока не будет введено верное количество строк с корректными
                // элементами в нем, соответствующие размерности массива.
                do
                {
                    var stringArrayWithElements = Console.ReadLine().Split(' ');
                    if (stringArrayWithElements.Length != n)
                    {
                        Console.WriteLine("Неверное количество элементов в введенной строке! Повторите операцию:");
                    }
                    else
                    {
                        var flagInt = 0;
                        for (var j = 0; j < stringArrayWithElements.Length; j++)
                        {
                            if (!double.TryParse(stringArrayWithElements[j], out array[i, j]) || 
                                ((stringArrayWithElements[j].StartsWith("0") && stringArrayWithElements[j] != "0")) || 
                                stringArrayWithElements[j].StartsWith(" ") )
                            {
                                Console.WriteLine("Есть некорректный элемент в строчке. Попробуйте снова!");
                                break;
                            }
                            double.TryParse(stringArrayWithElements[j], out array[i, j]);
                            if (array[i, j] is (< -500 or > 500))
                            {
                                Console.WriteLine("Значение элемента в строчке не входит в заданный диапазон. " +
                                                  "Попробуйте снова!"); 
                                break;
                            }
                            flagInt++;
                        }
                        if (flagInt == stringArrayWithElements.Length)
                            break;
                    }
                } while (true);
            }
            Console.WriteLine("\nВаш ввод корректен! Введенная матрица верна!\n");
            return array;
        }

        /// <summary>
        /// Данный метод генерирует матрицу размерности m x n.
        /// </summary>
        /// <param name="m">Размерность матрицы (количество строк).</param>
        /// <param name="n">Размерность матрицы (количество столбцов).</param>
        /// <returns>
        /// Возвращает матрицу с элементами типа double, с элементами типа double от -499,(9) до +499,(9).
        /// </returns>
        private static double[,] MatrixGeneration(byte m, byte n)
        {
            var array = new double[m, n];
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                {
                    var rand = new Random();
                    array[i, j] = Math.Round((rand.Next(-499, 500) + rand.NextDouble()), 3);
                }
            }
            
            Console.WriteLine("\nВаш ввод корректен! Матрица успешно сгенерирована компьютером.\n");
            
            return array;
        }
        
        /// <summary>
        /// Данный метод выполняет операцию поиска следа матрицы и выводит результат на экран.
        /// </summary>
        /// <param name="inputChoice">Тип ввода данных.</param>
        /// <param name="userOperation">Операция, которая выполняется над матрицами.</param>
        private static void MatrixTrace(byte inputChoice, byte userOperation)
        {
            // Описание операции.
            Console.WriteLine("Эта операция ищет след матрицы. Вот немного теории про то, что такое след матрицы:\n" +
                              "След матрицы - это сумма элементов квадратной матрицы, расположенных на главной " +
                              "диагонали. Значит, матрица квадратная, а ее размер n x n.");
            Console.WriteLine($"\nВаш способ задания матрицы: {inputChoice}.");
            
            var n = MatrixSizeInput(userOperation).Item1;
            // Создание массива на основании пользовательского выбора ввода данных.
            var array = (inputChoice == 1) ? MatrixKeyboardInput(n, n) : MatrixGeneration(n, n);
            
            Console.WriteLine("Исходная матрица:");
            PrintMatrix(array);
            
            // Поиск следа матрицы.
            double total = 0;
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                {
                    if (i == j)
                        total += array[i, j];
                }
            }
            
            // Вывод результата на экран.
            Console.WriteLine($"\nСлед данной матрицы = {total:F3}\n");
        }

        /// <summary>
        /// Данный метод выполняет операцию транспонирования матрицы и выводит результат на экран.
        /// </summary>
        /// <param name="inputChoice">Тип ввода данных.</param>
        /// <param name="userOperation">Операция, которая выполняется над матрицами.</param>
        private static void MatrixTransposition(byte inputChoice, byte userOperation)
        {
            // Описание операции.
            Console.WriteLine("Эта операция делает транспонирование матрицы. Вот немного теории на этот счет:\n" +
                              "Транспонированием матрицы называется операция, переводящая все строки в столбцы с " +
                              "сохранением порядка следования элементов.\nЕсли матрица имеет размер m x n, то она " +
                              "переходит в матрицу вида n x m.");
            Console.WriteLine($"\nВаш способ задания матрицы: {inputChoice}.");

            // Создание деконструктора для распаковки элементов кортежа с размерами массива.
            (var m, var n, var k) = MatrixSizeInput(userOperation);
            
            // Создание массива на основании пользовательского выбора ввода данных.
            var array = (inputChoice == 1) ? MatrixKeyboardInput(m, n) : MatrixGeneration(m, n);
            
            Console.WriteLine("Исходная матрица:");
            PrintMatrix(array);
            
            // Вывод транспонироанной матрицы на экран.
            var maxLength = MaxLengthInMatrix(array);
            Console.WriteLine("\nТранспонированная матрица будет выглядеть следующим образом:");
            for (var i = 0; i < array.GetLength(1); i++, Console.WriteLine())
            {
                for (var j = 0; j < array.GetLength(0); j++)
                {
                    Console.Write(
                        Math.Round(array[j, i], 3).ToString(CultureInfo.InvariantCulture).PadRight(maxLength + 3)
                                 );
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Данный метод выполняет операцию суммирования матриц и выводит результат на экран.
        /// </summary>
        /// <param name="inputChoice">Тип ввода данных.</param>
        /// <param name="userOperation">Операция, которая выполняется над матрицами.</param>
        private static void MatrixSum(byte inputChoice, byte userOperation)
        {
            // Описание операции.
            Console.WriteLine("Эта операция суммирует две матрицы. Что называется суммой двух матриц?\nОбратимся к " +
                              "определению: матрица С называется суммой матриц А и B, если матрицы А, B и С " +
                              "одинаковых размеров, а также если соответствующие элементы матрицы С равны сумме " +
                              "соответствующих элементов матриц А и B.");
            Console.WriteLine($"\nВаш способ задания матрицы: {inputChoice}. Вы выбираете одинаковый размер для " +
                              "каждой матрицы.");
            
            // Создание деконструктора для распаковки элементов кортежа с размерами массивов.
            (var m, var n, var k) = MatrixSizeInput(userOperation);
            
            // Создание массивов на основании пользовательского выбора ввода данных.
            var array1 = (inputChoice == 1) ? MatrixKeyboardInput(m, n) : MatrixGeneration(m, n);
            var array2 = (inputChoice == 1) ? MatrixKeyboardInput(m, n) : MatrixGeneration(m, n);
            
            Console.WriteLine("Исходная матрица 1:");
            PrintMatrix(array1);
            Console.WriteLine();
            Console.WriteLine("Исходная матрица 2:");
            PrintMatrix(array2);
            
            // Вывод суммы матриц на экран.
            var resultArray = new double[m, n];
            for (var i = 0; i < array1.GetLength(0); i++)
            {
                for (var j = 0; j < array1.GetLength(1); j++)
                {
                    resultArray[i, j] = Math.Round((array1[i, j] + array2[i, j]), 3);
                }
            }
            Console.WriteLine("\nСумма матриц А и B равна:");
            PrintMatrix(resultArray);
            Console.WriteLine();
        }

        /// <summary>
        /// Данный метод выполняет операцию разности матриц и выводит результат на экран.
        /// </summary>
        /// <param name="inputChoice">Тип ввода данных.</param>
        /// <param name="userOperation">Операция, которая выполняется над матрицами.</param>
        private static void MatrixDifference(byte inputChoice, byte userOperation)
        {
            // Описание операции.
            Console.WriteLine("Эта операция находит разность двух матриц. Что называется разностью двух матриц?\n" +
                              "Обратимся к определению: матрица С называется разностью матриц А и B, если матрицы " +
                              "А, B и С одинаковых размеров, а также если соответсвующие элементы матрицы С равны " +
                              "разности соответствующих элементов матриц А и B.");
            Console.WriteLine($"\nВаш способ задания матрицы: {inputChoice}. Вы выбираете одинаковый размер для " +
                              "каждой матрицы.");
            
            // Создание деконструктора для распаковки элементов кортежа с размерами массивов.
            (var m, var n, var k) = MatrixSizeInput(userOperation);
            
            // Создание массивов на основании пользовательского выбора ввода данных.
            var array1 = (inputChoice == 1) ? MatrixKeyboardInput(m, n) : MatrixGeneration(m, n);
            var array2 = (inputChoice == 1) ? MatrixKeyboardInput(m, n) : MatrixGeneration(m, n);
            
            Console.WriteLine("Исходная матрица 1:");
            PrintMatrix(array1);
            Console.WriteLine();
            Console.WriteLine("Исходная матрица 2:");
            PrintMatrix(array2);
            
            // Вывод разности матриц на экран.
            var resultArray = new double[m, n];
            for (var i = 0; i < array1.GetLength(0); i++)
            {
                for (var j = 0; j < array1.GetLength(1); j++)
                {
                    resultArray[i, j] = Math.Round((array1[i, j] - array2[i, j]), 3);
                }
            }
            Console.WriteLine("\nРазность матриц А и B равна:");
            PrintMatrix(resultArray);
            Console.WriteLine();
        }
        
        /// <summary>
        /// Данный метод выполняет операцию умножения матриц и выводит результат на экран.
        /// </summary>
        /// <param name="inputChoice">Тип ввода данных.</param>
        /// <param name="userOperation">Операция, которая выполняется над матрицами.</param>
        private static void MatrixMultiplication(byte inputChoice, byte userOperation)
        {
            // Описание операции.
            Console.WriteLine("Эта операция находит произведение двух матриц. Что называется произведением двух " +
                              "матриц?\nОбратимся к определению: произведением матрицы А размера m x n на матрицу " +
                              "B размера n x k называется матрица С размера m x k такая, что элемент матрицы C " +
                              "стоящий в i-ой строке и j-ом столбце, т.е. элемент [C]ij, равен сумме произведений " +
                              "элементов i-ой строки матрицы A на соответствующие элементы j-ого столбца матрицы B.");
            Console.WriteLine($"\nВаш способ задания матрицы: {inputChoice}.");
            
            // Создание деконструктора для распаковки элементов кортежа с размерами массивов.
            (var m, var n, var k) = MatrixSizeInput(userOperation);
            
            // Создание массивов на основании пользовательского выбора ввода данных.
            var array1 = (inputChoice == 1) ? MatrixKeyboardInput(m, n) : MatrixGeneration(m, n);
            var array2 = (inputChoice == 1) ? MatrixKeyboardInput(n, k) : MatrixGeneration(n, k);
            
            Console.WriteLine("Исходная матрица 1:");
            PrintMatrix(array1);
            Console.WriteLine();
            Console.WriteLine("Исходная матрица 2:");
            PrintMatrix(array2);

            // Вывод произведения матриц на экран.
            var resultArray = new double[m, k];
            for (var i = 0; i < array1.GetLength(0); i++)
            {
                for (var j = 0; j < array2.GetLength(1); j++)
                {
                    double total = 0;
                    for (var l = 0; l < array1.GetLength(1); l++)
                        total += array1[i, l] * array2[l, j];
                    resultArray[i, j] = Math.Round(total, 3);
                }
            }
            Console.WriteLine($"\nПроизведение матрицы А на матрицу B - это матрица C размера {m} x {k}:");
            PrintMatrix(resultArray);
            Console.WriteLine();
        }
        
        /// <summary>
        /// Данный метод выполняет операцию умножения матрицы на число и выводит результат на экран.
        /// </summary>
        /// <param name="inputChoice">Тип ввода данных.</param>
        /// <param name="userOperation">Операция, которая выполняется над матрицами.</param>
        private static void NumberAndMatrixMultiplication(byte inputChoice, byte userOperation)
        {
            // Описание операции.
            Console.WriteLine("Эта операция умножает число на матрицу. Как устроено данное умножение?\nКаждый " +
                              "элемент матрицы умножается на число, в результате чего получается новая матрица " +
                              "тех же размеров, но уже с новыми значениями.");
            Console.WriteLine($"\nВаш способ задания матрицы: {inputChoice}.");
            // Создание деконструктора для распаковки элементов кортежа с размерами массивов.
            (var m, var n, var k) = MatrixSizeInput(userOperation);
            double[,] array; double userNumber;
            // Создание массива и числа на основании пользовательского выбора ввода данных.
            if (inputChoice == 1)
            {
                Console.Write("Введите число от -500 до 500, на которое будет умножаться матрица: ");
                do
                {
                    var userNumberString = Console.ReadLine();
                    if (!double.TryParse(userNumberString, out userNumber) || userNumber is (< -500 or > 500) ||
                        userNumberString.StartsWith("0") || userNumberString.StartsWith(" "))
                    {
                        Console.Write("Вы ввели неправильное число, повторите попытку: "); continue;
                    } 
                    break;
                } while (true);
                array = MatrixKeyboardInput(m, n);
            }
            else
            {
                var rand = new Random();
                userNumber = Math.Round(rand.Next(-499, 499) + rand.NextDouble(), 3);
                array = MatrixGeneration(m, n);
            }
            Console.WriteLine($"Число, на которое будет умножаться матрица: {userNumber}\nИсходная матрица:");
            PrintMatrix(array);
            // Вывод матрицы, умноженной на число, на экран.
            var resultArray = new double[m, n];
            for (var i = 0; i < array.GetLength(0); i++)
            for (var j = 0; j < array.GetLength(1); j++)
                resultArray[i, j] = Math.Round((array[i, j] * userNumber), 3);

            Console.WriteLine("\nМатрица, умноженная на число равна:");
            PrintMatrix(resultArray);
            Console.WriteLine();
        }

        /// <summary>
        /// Данный метод является циклом для каждого элемента матрицы.
        /// </summary>
        /// <param name="array">Двумерный массив.</param>
        /// <param name="tmpArray">Двумерный массив размера p x q.</param>
        /// <param name="p">Количество строк в новой матрице.</param>
        /// <param name="q">Количество столбцов в новой матрице.</param>
        /// <param name="n">Размерность квадратной матрицы (n x n).</param>
        private static void DeterminantLoop(double[,] array, double[, ] tmpArray, int p, int q, int n) 
        { 
            // Объявляение двух переменных счетчиков (i - по строкам, j - по столбцам).
            int i = 0, j = 0;
            // Проход циклом по каждому элементу матрицы.
            for (var row = 0; row < n; row++) 
            { 
                for (var column = 0; column < n; column++) 
                { 
                    // Добавление элементов не из данной строки и столбца во временную матрицу.
                    if (row != p && column != q) 
                    { 
                        tmpArray[i, j] = array[row, column];
                        j++;
                        if (j == n - 1) 
                        { 
                            j = 0; 
                            i++; 
                        } 
                    } 
                } 
            } 
        } 
        
        /// <summary>
        /// Данный метод выполняет рекурсивную функцию поиска определителя матрицы.
        /// </summary>
        /// <param name="array">Двумерный массив.</param>
        /// <param name="n">Размерность матрицы (n x n).</param>
        /// <returns>Возвращает значение определителя матрицы.</returns>
        private static double DeterminantSearch(double[,] array, int n)
        {
            var determinant = 0.0;
            
            if (n == 1)
                return array[0, 0];
            
            // Дополнительный массив, хранящий кофакторы (или миноры).
            var tmpArray = new double[n, n];
            // Объявление переменной знака.
            var sign = 1; 
            
            // Проход циклом по каждому элементу первой строки.
            for (var i = 0; i < n; i++) 
            { 
                DeterminantLoop(array, tmpArray, 0, i, n); 
                determinant += sign * array[0, i] * DeterminantSearch(tmpArray, n - 1);
                sign = -sign; 
            } 
            
            return determinant; 
        }
        
        /// <summary>
        /// Данный метод выполняет операцию поиска определителя матрицы и выводит его на экран.
        /// </summary>
        /// <param name="inputChoice">Тип ввода данных.</param>
        /// <param name="userOperation">Операция, которая выполняется над матрицами.</param>
        private static void MatrixDeterminant(byte inputChoice, byte userOperation)
        {
            // Описание операции.
            Console.WriteLine("Эта операция ищет определитель матрицы. Что такое определитель матрицы?\n" +
                              "Определителем (детерминантом) квадратной матрицы порядка n называют сумму n! " +
                              "слагаемых. По сути определитель n-ого порядка является суммой произведений элементов " +
                              "матрицы, стоящих в разных строках (столбцах) по всем способам так сделать.\n" +
                              "Данный калькулятор ищет определитель для квадратной матрицы n = {1, 2,..., 10}.");
            Console.WriteLine($"\nВаш способ задания матрицы: {inputChoice}.");

            // Создание деконструктора для распаковки элементов кортежа с размерами массива.
            (var m, var n, var k) = MatrixSizeInput(userOperation);
            
            // Создание массива на основании пользовательского выбора ввода данных.
            var array = (inputChoice == 1) ? MatrixKeyboardInput(n, n) : MatrixGeneration(n, n);
            
            Console.WriteLine("Исходная матрица:");
            PrintMatrix(array);
            
            // Поиск детерминанта (определителя) и его вывод на экран.
            Console.Write("\nДетерминант для данной матрицы det A = ");
            
            var determinant = 0.0;
            switch (n)
            {
                case 1:
                    determinant = array[0, 0];
                    break;
                case 2:
                    determinant = array[0, 0] * array[1, 1] - array[0, 1] * array[1, 0];
                    break;
                case 3:
                    determinant = (array[0, 0] * array[1, 1] * array[2, 2]) - (array[0, 0] * array[1, 2] * array[2, 1])
                    - (array[0, 1] * array[1, 0] * array[2, 2]) + (array[0, 1] * array[1, 2] * array[2, 0]) +
                    (array[0, 2] * array[1, 0] * array[2, 1]) - (array[0, 2] * array[1, 1] * array[2, 0]);
                    break;
                case >= 4:
                    determinant = DeterminantSearch(array, n);
                    break;
            }
            Console.WriteLine($"{determinant:F3}\n");
        }

        /// <summary>
        /// Данный метод либо получает введенные пользователем свободные члены уравнения для метода Крамера, либо
        /// свободные члены генерируются автоматически.
        /// </summary>
        /// <param name="inputChoice">Тип ввода данных.</param>
        /// <param name="n">Размерность квадратной матрицы.</param>
        /// <returns>Возвращает массив свободных членов уравнений.</returns>
        private static double[] FreeMembersInputOrGenerator(byte inputChoice, int n)
        {
            var freeMembers = new double[n];
            
            if (inputChoice == 1)
            {
                // Цикл для построчного ввода свободных членов. 
                for (var i = 0; i < n; i++)
                {
                    do
                    {
                        Console.Write($"Введите {i + 1} свободный член от -500 до 500 " +
                                      "(без незначащих нулей и пробелов перед ним): ");
                        var numberString = Console.ReadLine();
                        if (!double.TryParse(numberString, out freeMembers[i]) || freeMembers[i] is (< -500 or > 500)
                        || numberString.StartsWith(" ") || numberString.StartsWith("0"))
                        {
                            Console.WriteLine("Вы ввели неверное число! Попробуйте снова!");
                            continue;
                        }
                        double.TryParse(numberString, out freeMembers[i]);
                        break;
                    } while (true);
                }
                Console.WriteLine("\nВы ввели верные свободные члены!\n");
            }
            else
            {
                var rand = new Random();
                // Цикл для генерации свободных членов.
                for (var i = 0; i < n; i++)
                {
                    freeMembers[i] = Math.Round((rand.Next(-499, 500) + rand.NextDouble()), 3);
                }
                Console.WriteLine("\nСвободные члены успешно сгенерированы!\n");
            }

            return freeMembers;
        }

        /// <summary>
        /// Данный метод выводит на экран систему линейных уравнений, исходя из данных, полученных от пользователя.
        /// </summary>
        /// <param name="array">Массив с коэффициентами неизвестных.</param>
        /// <param name="freeMembers">Свободные члены уравнений.</param>
        /// <param name="n">Размерность квадратного массива.</param>
        private static void OutputOfSlae(double[,] array, double[] freeMembers, int n)
        {
            const int aAsciiValue = 97;
            Console.WriteLine("Полученная система уравнений:");
            // Данные циклы выводят систему линейных уравнений.
            for (var i = 0; i < n; i++, Console.WriteLine())
            {
                for (var j = 0; j < n; j++)
                {
                    if (j == 0)
                    {
                        Console.Write($"{array[i, j]}{(char)(aAsciiValue + j)} ");
                    }
                    else
                    {
                        if (array[i, j] > 0)
                        {
                            Console.Write($"+ {Math.Abs(array[i, j])}{(char)(aAsciiValue + j)} ");
                        }
                        else if (array[i, j] < 0)
                        {
                            Console.Write($"- {Math.Abs(array[i, j])}{(char)(aAsciiValue + j)} ");
                        }
                    }
                }

                Console.Write($"= {freeMembers[i]}");
            }
        }

        /// <summary>
        /// Данный метод выводит на экран корни, полученные в результате решения СЛАУ методом Крамера.
        /// </summary>
        /// <param name="array">Массив с коэффициентами неизвестных.</param>
        /// <param name="freeMembers">Свободные члены уравнений.</param>
        /// <param name="n">Размерность квадратного массива.</param>
        /// <param name="determinant">Значение определителя.</param>
        private static void OutputSlaeRoots(double[,] array, double[] freeMembers, int n, double determinant)
        {
            const int aAsciiValue = 97;
            for (var count = 0; count < n; count++)
            {
                // Переменная flag отвечает за итерацию в цикле по свободным членам.
                var flag = 0;
                
                // Временный массив, в котором столбец count принимает значения свободных коэффициентов.
                var tmpArray = new double[n, n];
                
                // Циклы, которые создают новый временный массив.
                for (var i = 0; i < n; i++)
                {
                    for (var j = 0; j < n; j++)
                    {
                        if (j == count)
                            tmpArray[i, count] = freeMembers[flag];
                        else
                            tmpArray[i, j] = array[i, j];
                    }
                    flag++;
                }
                // Вывод неизвестной и ее значения в результате решения СЛАУ.
                Console.WriteLine($"{(char)(aAsciiValue + count)} = " +
                                  $"{(DeterminantSearch(tmpArray, n) / determinant):F3}");
            }
            Console.WriteLine();
        }
        
        /// <summary>
        /// Данный метод выполняет решение СЛАУ путем использования метода Крамера.
        /// </summary>
        /// <param name="inputChoice">Тип ввода данных.</param>
        /// <param name="userOperation">Операция, которая выполняется над матрицами.</param>
        private static void KramerMethod(byte inputChoice, byte userOperation)
        {
            // Описание операции.
            Console.WriteLine("Эта операция решает СЛАУ с помощью метода Крамера. Что такое метод Крамера?\n" +
                              "Это способ решения систем линейных алгебраических уравнений с числом уравнений " +
                              "равным числу неизвестных с ненулевым главным определителем матрицы коэффициентов " +
                              "системы. То есть метод Крамера работает с квадратными матрицами размера n = " +
                              "{1, 2,..., 9}, так как ищется определитель.");
            Console.WriteLine($"\nВаш способ задания матрицы: {inputChoice}.");

            // Создание деконструктора для распаковки элементов кортежа с размерами массива.
            (var m, var n, var k) = MatrixSizeInput(userOperation);
            
            // Создание массива на основании пользовательского выбора ввода данных.
            var array = (inputChoice == 1) ? MatrixKeyboardInput(n, n) : MatrixGeneration(n, n);

            // Создание массива со свободными членами уравнения.
            var freeMembers = FreeMembersInputOrGenerator(inputChoice, n);
            
            // Вывод полученной системы уравнений на экран.
            OutputOfSlae(array, freeMembers, n);

            // Переменная, находящая определитель введенной среди коэффициентов при неизвестных.
            var determinant = DeterminantSearch(array, n);
            
            // Вывод корней уравнения, или сообщение об их отсутствии.
            if (determinant == 0)
            {
                Console.WriteLine("\nОпределитель det A = 0. Поэтому для данной системы уравнений не существует " +
                                  "решений.\n");
            }
            else
            {
                Console.WriteLine("\nКорни данной системы следующие:");
                OutputSlaeRoots(array, freeMembers, n, determinant);
            }
        }
        
        /// <summary>
        /// Данный метод выполняет операцию, выбранную пользователем.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="userOperation">Операция, которая выполняется над матрицами.</param>
        /// <param name="inputChoice">Тип ввода данных.</param>
        private static void OperationExecution(string userName, byte userOperation, byte inputChoice)
        {
            Console.WriteLine($"\nИтак, {userName}, вы выбрали операцию №{userOperation}.");
            
            // Оператор switch переключается между операциями над матрицами, выполняет ту, которую выбрал пользователь. 
            switch (userOperation)
            {
                case 1:
                    MatrixTrace(inputChoice, userOperation);
                    break;
                case 2:
                    MatrixTransposition(inputChoice, userOperation);
                    break;
                case 3:
                    MatrixSum(inputChoice, userOperation);
                    break;
                case 4:
                    MatrixDifference(inputChoice, userOperation);
                    break;
                case 5:
                    MatrixMultiplication(inputChoice, userOperation);
                    break;
                case 6:
                    NumberAndMatrixMultiplication(inputChoice, userOperation);
                    break;
                case 7:
                    MatrixDeterminant(inputChoice, userOperation);
                    break;
                case 8:
                    KramerMethod(inputChoice, userOperation);
                    break;
            }
        }

        /// <summary>
        /// Данный метод выводит на экран сообщение, в котором прощается с пользователем. 
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        private static void UserFarewell(string userName)
        {
            Console.WriteLine($"{userName}, искренне благодарю вас за использование моего калькулятора!\nНадеюсь, " +
                              "вы получили желаемые результаты или просто поупражнялись в различных операциях с " +
                              "матрицами.\nЖелаю вам всего наилучшего и до скорых встреч!");
        }

        /// <summary>
        /// Данный метод получает кнопку, которую выбрал пользователь после выполнения операции над матрицами.
        /// </summary>
        /// <param name="flag">Флаг типа bool, который отвечает за повторение операций или вывода сообщения о
        /// некорректности нажатой кнопки.</param>
        /// <param name="stopButton">Кнопка, которая отвечает за выход из калькулятора и имеет тип bool.</param>
        private static void RepeatOrExitButton(ref bool flag, ref bool stopButton)
        {
            var keyToExit = Console.ReadKey();
            Console.WriteLine();
            
            // Условие, которое выполняет различные действия при нажатии разных кнопок.
            if (keyToExit.Key == ConsoleKey.H)
            {
                Console.WriteLine();
                CalculatorOperationsDescription();
            }
            else if (keyToExit.Key == ConsoleKey.Enter)
            {
                flag = true;
            }
            else if (keyToExit.Key == ConsoleKey.Backspace)
                stopButton = true;
            else
            {
                Console.WriteLine("Вы нажали неверную кнопку. Повторите действие:");
                flag = false;
            }
        }
        
        /// <summary>
        /// Точка входа в программу. Основной метод Main().
        /// </summary>
        static void Main()
        {
            var userName = String.Empty;
            
            // Приветствие пользователя.
            UserGreeting(ref userName);
            
            // Инструкции операций, доступных пользователю.
            CalculatorOperationsDescription();
            
            // Выполнение операций над матрицами.
            var flag = true;
            var stopButton = false;
            do
            {
                if (flag)
                {
                    // Выбор пользователем операции, которую он хочет выполнить.
                    var userOperation = UsersOperationChoice(userName);
                    
                    // Выбор пользователем типа ввода данных.
                    var inputChoice = InputChoice(userName);
                    
                    // Выполнение операции, которую выбрал пользователь.
                    OperationExecution(userName, userOperation, inputChoice);
                    
                    Console.WriteLine("Нажмите \"ENTER\", чтобы снова выбрать операцию и повторить действие над " +
                                      "матрицами.\nЕсли вы забыли доступные операции, а листать вверх лень, нажмите " +
                                      "\"h\" или \"H\".\nНажмите \"BACKSPACE\", чтобы выйти из калькулятора.");
                }

                // Предложение пользователю повторить операцию, получить подсказки или же выйти из калькулятора.
                RepeatOrExitButton(ref flag, ref stopButton);
                if (stopButton)
                    break;
            } while (true);

            // Прощание с пользователем.
            UserFarewell(userName);
        }
    }
}