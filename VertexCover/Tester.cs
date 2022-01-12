using System;
using System.Linq;

namespace VertexCover
{
    public static class Tester
    {
        public static TesterResult Test(int countVertix, int countTests, Func<int, bool[,]> matrixType)
        {
            var result = new TesterResult
            {
                CountTests = countTests,
                CountVertix = countVertix
            };

            for (int i = 0; i < countTests; i++)
            {
                var matrix = matrixType(countVertix);
                var greedyResult = StartAlgorithm(matrix, Methods.Greedy);
                var approxResult = StartAlgorithm(matrix, Methods.Approximate);
                var accurateResult = StartAlgorithm(matrix, Methods.BruteForce);

                var greedyCountVert = greedyResult.Set.Distinct().ToArray().Length;
                var approxCountVert = approxResult.Set.Distinct().ToArray().Length;
                var accurateCountVert = accurateResult.Set.Distinct().ToArray().Length;

                result.TotalAccurateTime += accurateResult.Time;
                result.TotalGreedyTime += greedyResult.Time;
                result.TotalApproxTime += approxResult.Time;
                
                if (greedyCountVert == accurateCountVert)
                {
                    result.MatchedSolutionsGreedy++;
                }
                if (approxCountVert == accurateCountVert)
                {
                    result.MatchedSolutionsApprox++;
                }

                result.TotalRelativeDeviationGreedy += Math.Abs(greedyCountVert - accurateCountVert) * 1.0 / accurateCountVert;
                result.TotalRelativeDeviationApprox += Math.Abs(approxCountVert - accurateCountVert) * 1.0 / accurateCountVert;
            }

            return result;
        }

        public static bool[,] GenerateRandomMatrix(int count)
        {
            Random random = new Random((int)DateTime.Now.Ticks);

            bool[,] matrix = new bool[count, count];

            for (int i = 0; i < count; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    bool value = random.Next() % 2 == 0;
                    matrix[i, j] = value;
                    matrix[j, i] = value;
                }
            }
            matrix[1, 0] = true;
            matrix[0, 1] = true;
            return matrix;
        }

        public static bool[,] GenerateMatrixRingGraph(int count)
        {
            bool[,] matrix = new bool[count, count];

            for (int i = 0; i < count; i++)
            {
                for (int j = i + 1; j < i + 2 && j < count; j++)
                {
                    matrix[i, j] = true;
                    matrix[j, i] = true;
                }
            }
            return matrix;
        }

        public static bool[,] GenerateMatrixCompleteGraph(int count)
        {
            bool[,] matrix = new bool[count, count];

            for (int i = 0; i < count; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    matrix[i, j] = true;
                    matrix[j, i] = true;
                }
            }
            return matrix;
        }

        public static Result StartAlgorithm(bool[,] matrix, Func<bool[,], int[]> algorithm)
        {
            var timeStart = DateTime.Now;
            var coverSet = algorithm(matrix);
            var timeEnd = DateTime.Now;
            var time = timeEnd - timeStart;

            int countVert = coverSet.Distinct().ToArray().Length;

            return new Result(time, coverSet, countVert);
        }
    }
}