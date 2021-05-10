using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class CorrelatedFeatures
    {
        public string feature1 { get; }
        public string feature2 { get; }
        public double corrlation { get; }
        public Line lin_reg { get; }
        public double threshold { get; }
        public double cx { get; }
        public double cy { get; }
        public bool isHybrid { get; }
        public CorrelatedFeatures(string f1, string f2, double corr, Line reg, double thresh, double cx, double cy, bool isHybrid)
        {
            this.feature1 = f1;
            this.feature2 = f2;
            this.corrlation = corr;
            this.lin_reg = reg;
            this.threshold = thresh;
            this.cx = cx;
            this.cy = cy;
            this.isHybrid = isHybrid;
        }
    }
}
