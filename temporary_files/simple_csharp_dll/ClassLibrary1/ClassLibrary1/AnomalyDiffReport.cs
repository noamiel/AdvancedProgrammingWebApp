using System;
using System.Collections.Generic;

namespace ClassLibrary1
{
    public class AnomalyDiffReport
    {
        public List<AnomalyReport> anomaliesVector;
        public List<AnomalyTimeDiff> detectedTimes;  //
        // public List<AnomalyTimeDiff> givenTimes;     // real times to compare to
        public Dictionary<string, Tuple<int, int>> lastTimeRange;

        // write `detectedTimes`
        public AnomalyDiffReport(List<AnomalyReport> aVec)
        {
            anomaliesVector = aVec;
            lastTimeRange = new Dictionary<string, Tuple<int, int>>();
            detectedTimes = new List<AnomalyTimeDiff>();
            // givenTimes = gtimes;

            // fill `detectedTimes`
            foreach (AnomalyReport ar in aVec)
            {
                if (!lastTimeRange.ContainsKey(ar.Description))
                    lastTimeRange.Add(ar.Description, new Tuple<int, int>((int)ar.TimeStep, (int)ar.TimeStep - 1));

                if (lastTimeRange[ar.Description].Item2 + 1 == ar.TimeStep)
                { // the same range continues
                    lastTimeRange[ar.Description] = new Tuple<int, int>(lastTimeRange[ar.Description].Item1, lastTimeRange[ar.Description].Item2 + 1);
                }
                else
                { // close up the old range and get start with a new one
                    detectedTimes.Add(new AnomalyTimeDiff(ar.Description, lastTimeRange[ar.Description].Item1, lastTimeRange[ar.Description].Item2));
                    lastTimeRange[ar.Description] = new Tuple<int, int>((int)ar.TimeStep, (int)ar.TimeStep);
                }
            }
            foreach (KeyValuePair<string, Tuple<int, int>> pair in lastTimeRange)
            {
                detectedTimes.Add(new AnomalyTimeDiff(pair.Key, lastTimeRange[pair.Key].Item1, lastTimeRange[pair.Key].Item2));
            }
        }

        /* private bool IsAnomalyTrue(AnomalyTimeDiff detected)
        {
            for (AnomalyTimeDiff atd : givenTimes)
                if (detected.areOverlapping(atd)) return true;  // if `detected` overlaps any anomaly time, return true
            return false;
        }*/
    }
}