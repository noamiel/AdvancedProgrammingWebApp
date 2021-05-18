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
            // System.Console.WriteLine(csv_learn);
            // string passed_data_type = (string)input.passed_data_type;

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

        public async Task<object> DetectCSV(dynamic input)
        {
            await Task.Delay(1);
            string csv_name = (string)input.csv_name;
            string model_type = (string)input.model_type;
            // System.Console.WriteLine(csv_name);
            // return csv_name;
            // string passed_data_type = (string)input.passed_data_type;

            TimeSeries ts = new TimeSeries(csv_name);
            List<AnomalyReport> l;
            if (model_type == "hybrid")
                l = HybridAnomalyDetector.Detect(ts);
            else
                l = RegressionAnomalyDetector.Detect(ts);
            // System.Console.WriteLine(l.Count());
            return new AnomalyDiffReport(l).detectedTimes;
            // return l;
        }

    }
}
