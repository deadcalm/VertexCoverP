using System;

namespace VertexCover
{
    public class TesterResult
    {
        public TimeSpan TotalGreedyTime { get; set; }
        public TimeSpan TotalAccurateTime { get; set; }
        public TimeSpan TotalApproxTime { get; set; }

        public int MatchedSolutionsGreedy { get; set; }
        public int MatchedSolutionsApprox { get; set; }
        public double TotalRelativeDeviationGreedy { get; set; }
        public double TotalRelativeDeviationApprox { get; set; }

        public int CountVertix { get; set; }
        public int CountTests { get; set; }
    }
}
