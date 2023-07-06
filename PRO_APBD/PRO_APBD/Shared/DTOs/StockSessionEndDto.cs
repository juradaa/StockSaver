using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO_APBD.Shared.DTOs
{
    public class StockSessionEndDto
    {
        public string From { get; set; }
        public string Symbol { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }
        public double AfterHours { get; set; }
        public double PreMarket { get; set; }

    }
}
