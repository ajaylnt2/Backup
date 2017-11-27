namespace TSManager
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
        public string Aggregation { get; set; }
        public string month { get; set; }
        public int TimeStamp { get; set; }
    }

    public class TsQueryModel
    {
        public Tag Tags { get; set; }
        public string month { get; set; }
        public int order { get; set; }
        public string limit { get; set; }
    }
}
