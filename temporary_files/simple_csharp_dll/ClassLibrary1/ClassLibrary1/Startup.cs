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
            string csv_name = (string)input.csv_name;
            string model_type = (string)input.model_type;
            // string passed_data_type = (string)input.passed_data_type;

            TimeSeries ts = new TimeSeries(csv_name);
            if (model_type == "hybrid")
                HybridAnomalyDetector.LearnNormal(ts);
            else
                RegressionAnomalyDetector.LearnNormal(ts);
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