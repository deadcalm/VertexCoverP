using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

namespace VertexCover
{
    class Methods
    {
        public static bool[,] GetCopyMatrix(bool[,] _matrix)
        {
            var N = _matrix.GetLength(0);
            var matrix = new bool[N, N];

            for (int i = 0; i < N; i++)
            {
                for (int j = i + 1; j < N; j++)
                {
                    var value = _matrix[i, j];
                    matrix[i, j] = value;
                    matrix[j, i] = value;
                }
            }

            return matrix;
        }
        public static int[] Greedy(bool[,] _matrix)
        {
            int n = _matrix.GetLength(0);

            var matrix = GetCopyMatrix(_matrix);
            var sums = new List<int>(n);
            for (int i = 0; i < n; i++)
                sums.Add(0);

            var result = new List<int>();

            while (true)
            {
                for (int i = 0; i < n; i++)
                {
                    int sum = 0;
                    for (int j = 0; j < n; j++)
                    {
                        sum += matrix[i, j] ? 1: 0;
                    }

                    sums[i] = sum;
                }

                int max = sums.Max();
                if (max == 0)
                {
                    break;
                }

                int index = sums.IndexOf(max);
                result.Add(index);

                for (int i = 0; i < n; i++)
                {
                    matrix[index, i] = false;
                    matrix[i, index] = false;
                }
            }

            return result.ToArray();
        }

        public static int[] Approximate(bool[,] matrix)
        {
            //создаём пустое мн-во вершин
            int count = matrix.GetLength(0);
            List<int> Vertex = new List<int>();

            //создаём список всех ребер в графе
            List<Edge> LEdge = new List<Edge>();
            for (int i = 0; i < count; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    if (matrix[i, j])
                    {
                        Edge E = new Edge(i, j);
                        LEdge.Add(E);
                    }

                }
            }
            //O (n^2)
            Random R = new Random();
            //выбрать произвольное ребро
            while (LEdge.Count != 0)
            {
                
                int rand = R.Next(0, LEdge.Count);

                //добавить вершины ребра в вершинное покрытие
                Vertex.Add(LEdge[rand].X);
                Vertex.Add(LEdge[rand].Y);

                //удалить из графа все ребра инцидентные вершинному покрытию
                List<Edge> LEdge2 = new List<Edge>();
                for (int i = 0; i < LEdge.Count; i++)
                {
                    if (LEdge[i].X == LEdge[rand].X || LEdge[i].X == LEdge[rand].Y || LEdge[i].Y == LEdge[rand].X || LEdge[i].Y == LEdge[rand].Y)
                    {
                        LEdge2.Add(LEdge[i]);
                    }
                }
                foreach (var E in LEdge2)
                {
                    LEdge.Remove(E);
                }
                //O (n^2)
            }

            return Vertex.ToArray();
        }

        public static bool Check(List<Edge> LEdge, List<int> Vertex)
        {
            List<Edge> LEdge2 = new List<Edge>(LEdge);//1
            List<Edge> LEdge3 = new List<Edge>();//1
            for (int i = 0; i < Vertex.Count; i++)//п: 1, с: n+2, и: n+1
            {
                for (int j = 0; j < LEdge2.Count; j++)//п: n+1, с: (n+1)*(n+2), и: (n+1)*(n+1)
                {
                    if ((LEdge2[j].X == Vertex[i])||(LEdge2[j].Y == Vertex[i])) //2*(n+1)*(n+1)
                    {
                        LEdge3.Add(LEdge2[j]);//(n*(n-1))/4
                    }
                }
            }
            foreach (var E in LEdge3) //(n*(n-1))/4
            {
                LEdge2.Remove(E);//(n*(n-1))/4
            }

            return LEdge2.Count == 0;//1
            // итог:(19n^2)/4+45n/4+13
        }

        public static int[] BruteForce(bool[,] matrix)
        {
            int count = matrix.GetLength(0);    //1
            List<Edge> LEdge = new List<Edge>();    //1
            for (int i = 0; i < count; i++) //п: 1, с: n+2, и: n+1
            {
                for (int j = i + 1; j < count; j++)//п: n+1, с: (n+1)*(n+2), и: (n+1)*(n+1)
                {
                    if (matrix[i, j])//(n+1)*(n+1)
                    {
                        Edge E = new Edge(i, j);//(n*(n-1))/2
                        LEdge.Add(E);//(n*(n-1))/2
                    }
                }
            }
            List<int> Vertex = new List<int>();//1
            List<int> minSet = new List<int>();//1
            for(int i = 0; i < count; i++)//п: 1, с: n+2, и: n+1
            {
                minSet.Add(i);//n+1
            }
            int q = 1 << count;//2
            for (int i = 0; i < q; i++)////п: 1, с: 2^n+2, и: 2^n+1
            {
                for (int j = 0; j < count; j++)//п: 2^n+1, с: (2^n+1)*(n+2), и: (2^n+1)*(n+1)
                {
                    if (((i >> j) & 1) == 1) //3*(2^n+1)*(n+1)
                    {
                        Vertex.Add(j);//(2^n+1)*(n+1)/2
                    }
                }
                if (Check(LEdge, Vertex))//(2^n+1)*((19n^2)/4+45n/4+13)
                {
                    if (Vertex.Count < minSet.Count)//(2^n+1)/2
                    {
                        minSet = Vertex;//(2^n+1)/2
                        Vertex = new List<int>();//(2^n+1)/2
                    }
                }
                Vertex.Clear();//n+1
            }
            return minSet.ToArray();//1
            //итог: 1/4(2^n+1)(19n^2+63n)+23*2^n+4n^2+13n+48
        }
    }
}
