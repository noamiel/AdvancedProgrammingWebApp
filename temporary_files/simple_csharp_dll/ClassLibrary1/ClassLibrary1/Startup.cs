using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Startup
    {

        public async Task<object> LearnCSV(dynamic input)
        {
            await Task.Delay(1);
            string csv_learn = (string)input.csv_learn;
            string csv_anomaly = (string)input.csv_anomaly;
            string model_type = (string)input.model_type;

            TimeSeries ts_learn = new TimeSeries(csv_learn);
            TimeSeries ts_anomaly = new TimeSeries(csv_anomaly);
            List<AnomalyReport> l;
            if (model_type == "hybrid")
            {
                HybridAnomalyDetector.LearnNormal(ts_learn);
                l = HybridAnomalyDetector.Detect(ts_anomaly);
            }
            else
            {
                RegressionAnomalyDetector.LearnNormal(ts_learn);
                l = RegressionAnomalyDetector.Detect(ts_anomaly);
            }
            return new AnomalyDiffReport(l).detectedTimes;
        }
    }
}
