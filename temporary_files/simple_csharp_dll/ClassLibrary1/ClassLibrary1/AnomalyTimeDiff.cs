namespace ClassLibrary1
{
    public class AnomalyTimeDiff
    {
        public readonly string description;
        public readonly long firstTimeStep;
        public readonly long lastTimeStep;

        public AnomalyTimeDiff(string description, long firstTimeStep, long lastTimeStep)
        {
            this.description = description;
            this.firstTimeStep = firstTimeStep;
            this.lastTimeStep = lastTimeStep;
        }

        public int TimeDiff()
        {
            return (int)(lastTimeStep - firstTimeStep);
        }

        public bool AreOverlapping(AnomalyTimeDiff other)
        {
            // if this started before other
            if (firstTimeStep < other.firstTimeStep) return (lastTimeStep >= other.firstTimeStep);
            if (firstTimeStep > other.firstTimeStep) return (firstTimeStep <= other.lastTimeStep);
            return true;
        }
    }
}