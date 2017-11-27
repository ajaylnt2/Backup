using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestJsonSample
{
    public class TimeSeriesData
    {
        public string TagName { get; set; }
        public int TimeStamp { get; set; }
        public object Data { get; set; }
        public object Attributes { get; set; }
    }
}
