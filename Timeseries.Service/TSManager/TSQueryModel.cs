﻿namespace TSManager
{
    public enum TimeStampUnit
    {
        Ms,
        S,
        Mi,
        H,
        D,
        W,
        Mm,
        Y
    }
    public class Tag
    {
        public string[] Name { get; set; }
        public int limit { get; set; }
        public object Aggregations { get; set; }
        public object Filters { get; set; }
        public object Group { get; set; }
        public string Order { get; set; }
        public string Aggregation { get; set; }
    }

    public class TsQueryModel
    {
        public string Start { get; set; }
        public string End { get; set; }
        public Tag Tags { get; set; }

    }
}
