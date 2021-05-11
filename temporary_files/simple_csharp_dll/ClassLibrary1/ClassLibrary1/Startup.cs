using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Startup
    {
        /* public async Task<object> Invoke(object input)
        {
            int v = (int)input;
            return Helper.AddSeven(v);
        }*/

        /* timeseries ts = new timeseries("C://Users//omerz//Desktop//אוניברסיטה//web app//file1.csv");
                timeseries anomaly = new timeseries("C://Users//omerz//Desktop//אוניברסיטה//web app//anomaly_flight.csv");
                Console.WriteLine(ts.getRowSize());

                HybridAnomalyDetector.learnNormal(ts);
                HybridAnomalyDetector.detect(anomaly);
                int a = 1; */

        public async Task<object> HybridCSV(dynamic input)
        {
            await Task.Delay(1);
            string csv_name = (string)input.csv_name;
            // string model_type = (string)input.model_type;
            // string passed_data_type = (string)input.passed_data_type;

            TimeSeries ts = new TimeSeries(csv_name);
            HybridAnomalyDetector.LearnNormal(ts);
            return 0;
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