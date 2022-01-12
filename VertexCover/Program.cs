using System;
using System.Linq;
using System.IO;
using System.Diagnostics;

namespace VertexCover
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string input = @"input.txt";
            string output = @"output.txt";

            string help = "Ввод из файла - 1\nТестирование - 2\nВыход - 3\n";

            Console.WriteLine(help);
            int.TryParse(Console.ReadLine(), out int result);
            Console.Clear();
            switch (result)
            {
                case 1: File(input); break;
                case 2: Test(output); break;
                default:
                    break;
            }
            Console.WriteLine("\nВыполнено!");
            Console.ReadKey();
        }

        private static void File(string input)
        {
            bool[,] matrix = ReadMatrix(input);
            Console.WriteLine("=========\nИсходные данные: \nn =  " + matrix.GetLength(0));
            PrintMatrix(matrix);

            var resultGreedy = Tester.StartAlgorithm(matrix, Methods.Greedy);
            var resultApprox = Tester.StartAlgorithm(matrix, Methods.Approximate);
            var resultAccuratee = Tester.StartAlgorithm(matrix, Methods.BruteForce);

            PrintSet(resultGreedy.Set, "Жадный алгоритм", resultGreedy.Time);
            PrintSet(resultApprox.Set, "Приближенный алгоритм", resultApprox.Time);
            PrintSet(resultAccuratee.Set, "Точный алгоритм", resultAccuratee.Time);
        }

        private static void Test(string output)
        {
            string help = "Тип матрицы:\n\tматрица рандомого графа - 1\n\tматрица полного графа - 2\n\tматрица кольцевого графа - 3\n";
            Console.WriteLine(help);

            Func<int, bool[,]> matrixType;

            int.TryParse(Console.ReadLine(), out int result);
            Console.Clear();
            Console.WriteLine(help);
            string res;
            switch (result)
            {
                case 1: matrixType = Tester.GenerateRandomMatrix; res = "рандомного"; break;
                case 2: matrixType = Tester.GenerateMatrixCompleteGraph; res = "полного"; break;
                case 3: matrixType = Tester.GenerateMatrixRingGraph; res = "кольцевого"; break;
                default: matrixType = Tester.GenerateRandomMatrix; res = "рандомного"; break;
            }

            Console.WriteLine($"Выбрана матрица {res} графа");

            var tests = new int[]
            {
                100, 100, 100, 100, 100,
                100, 100, 100, 100, 100,
                100, 100, 100, 100, 100,
                100, 100, 100, 100, 100,
            };

            using (var writer = new StreamWriter(output))
            {
                for (int i = 4; i < 15; i++)
                {
                    //var timeStart = DateTime.Now;
                    var s = new Stopwatch();
                    s.Start();
                    var r = Tester.Test(i, tests[i - 1], matrixType); // r - result
                    s.Stop();
                    //var timeEnd = DateTime.Now;
                    writer.WriteLine(
                        $"Тест №{i - 3} Количество тестов - {r.CountTests}:" +
                        $"\n\tКоличество вершин: {r.CountVertix} \n\tСреднее время работы: \n\t\tжадного - {r.TotalGreedyTime.TotalMilliseconds / r.CountTests}мс \n\t\tприближенного - {r.TotalApproxTime.TotalMilliseconds / r.CountTests}мс" +
                        $"\n\t\tточного(полный перебор) - {r.TotalAccurateTime.TotalMilliseconds / r.CountTests}мс \n\tСовпавших решений жадного с точным: {r.MatchedSolutionsGreedy * 100.0 / r.CountTests}%" +
                        $"\n\tСовпавших решений приближенного с точным: {r.MatchedSolutionsApprox * 100.0 / r.CountTests}%" +
                        $"\n\tСреднее относительное отклонение жадного алгоритма: {r.TotalRelativeDeviationGreedy / r.CountTests}" +
                        $"\n\tСреднее относительное отклонение приближенного алгоритма: {r.TotalRelativeDeviationApprox / r.CountTests}" +
                        $"");
                    //writer.WriteLine($"{i} {r.TotalGreedyTime.TotalMilliseconds / r.CountTests}");
                    Console.WriteLine($"{i - 3} {DateTime.Now} - {s.ElapsedMilliseconds}мс Тестов:{tests[i - 1]}");
                }
            }
        }

        private static void PrintSet(int[] set, string algorithmName, TimeSpan time)
        {
            Console.WriteLine($"\n=========\n{algorithmName}");
            Console.WriteLine($"Количество вершин: {set.Distinct().Count()}");
            Console.WriteLine($"Время работы: {time.Minutes}мин {time.Seconds}с {time.Milliseconds}мс");
            var count = set.Length;
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(set[i]);
            }
        }

        private static bool[,] ReadMatrix(string path)
        {
            bool[,] matrix;

            using (StreamReader reader = new StreamReader(path))
            {
                int count = int.Parse(reader.ReadLine());
                matrix = new bool[count, count];
                for (int i = 0; i < count; i++)
                {
                    var line = reader.ReadLine();

                    var values = line.Split(' ').Select(x => int.Parse(x) != 0).ToArray();

                    for (int j = 0; j < count; j++)
                    {
                        matrix[i, j] = values[j];
                    }
                }
            }

            return matrix;
        }

        private static void PrintMatrix(bool[,] matrix)
        {
            int count = matrix.GetLength(0);

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    Console.Write(Convert.ToInt32(matrix[i, j]) + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
