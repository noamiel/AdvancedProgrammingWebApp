namespace ClassLibrary1
{
    internal class CorrelatedFeatures
    {
        public string Feature1 { get; }
        public string Feature2 { get; }
        public double Correlation { get; }
        public Line LinReg { get; }
        public double Threshold { get; }
        public double Cx { get; }
        public double Cy { get; }
        public bool IsHybrid { get; }

        public CorrelatedFeatures(string f1, string f2, double corr, Line reg, double thresh, double cx, double cy, bool isHybrid)
        {
            Feature1 = f1;
            Feature2 = f2;
            Correlation = corr;
            LinReg = reg;
            Threshold = thresh;
            Cx = cx;
            Cy = cy;
            IsHybrid = isHybrid;
        }
    }
}