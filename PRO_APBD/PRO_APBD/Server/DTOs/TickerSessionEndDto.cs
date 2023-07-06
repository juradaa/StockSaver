namespace PRO_APBD.Server.DTOs
{

    public class TickerSessionEndDto
    {
        public string status { get; set; }
        public string from { get; set; }
        public string symbol { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double close { get; set; }
        public double volume { get; set; }
        public double afterHours { get; set; }
        public double preMarket { get; set; }
    }

}
