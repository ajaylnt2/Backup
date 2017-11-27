using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestJsonSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.GenerateJSON();
        }

        public void GenerateJSON()
        {
            List<TimeSeriesData> _tsData = new List<TimeSeriesData>();
            for (int i = 0; i < 500; i++)
            {
                var _timeseries = new TimeSeriesData();
                _timeseries.TagName = i % 2 == 0 ? "Alpha" : "Beta";

                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                _timeseries.TimeStamp = Convert.ToInt32((DateTime.Now.AddDays(i).ToUniversalTime() - epoch).TotalSeconds);

                data d1 = new data();
                d1.Pressure = (100 + i);
                d1.UOM = "PSI";
                _timeseries.Data = d1;

                Attributes a1 = new Attributes();
                a1.EnergyType = (i % 2 == 0 ? "Water" : "Air");

                _timeseries.Attributes = a1;


                _tsData.Add(_timeseries);

            }

            var serialize = JsonConvert.SerializeObject(_tsData);
            System.IO.File.WriteAllText(@"C:\Users\90001850\Documents\SampleJsonData.txt", serialize);
        }
    }
}
