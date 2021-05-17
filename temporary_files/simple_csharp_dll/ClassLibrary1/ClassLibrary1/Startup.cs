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
                
            return l;
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
            return l;
        }

        public async Task<object> Regression(dynamic input)
        {
            await Task.Delay(3000);
            IDictionary<string, object> d = (IDictionary<string, object>)(input);
            Dictionary<string, List<double>> jsonDict = new Dictionary<string, List<double>>();
            foreach (KeyValuePair<string, object> entry in d)
            {
                object[] anArray = (object[])entry.Value;
                jsonDict[entry.Key] = new List<double>();
                foreach (var item in anArray)
                    jsonDict[entry.Key].Add(double.Parse(item.ToString()));
            }

            return jsonDict[jsonDict.Keys.First()][0];
        }

        public async Task<object> Hybrid(dynamic input)
        {
            IDictionary<string, object> d = (IDictionary<string, object>)(input);
            Dictionary<string, List<double>> jsonDict = new Dictionary<string, List<double>>();
            foreach (KeyValuePair<string, object> entry in d)
            {
                object[] anArray = (object[])entry.Value;
                jsonDict[entry.Key] = new List<double>();
                foreach (var item in anArray)
                    jsonDict[entry.Key].Add(double.Parse(item.ToString()));
            }
            await Task.Delay(3000);
            return jsonDict[jsonDict.Keys.First()][0];
        }
    }
}