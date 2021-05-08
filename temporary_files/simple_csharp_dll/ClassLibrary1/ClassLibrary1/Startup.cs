using DynamicExpresso;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using System.ComponentModel;

public class Startup
{
    public async Task<object> Invoke(object input)
    {
        int v = (int)input;
        return Helper.AddSeven(v);
    }

    public async Task<object> Regression(dynamic input)
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

        return jsonDict[jsonDict.Keys.First()][0];
    }
}

public class Result
{
    public int id { get; set; }
    public string value { get; set; }
    public string info { get; set; }
}