using System;

namespace VertexCover
{
    public class Result
    {
        public Result(TimeSpan time, int[] set, int countVert)
        {
            Time = time;
            Set = set;
            CountVert = countVert;
        }

        public TimeSpan Time { get; set; }
        public int[] Set { get; set; }
        public int CountVert { get; set; }
    }
}
