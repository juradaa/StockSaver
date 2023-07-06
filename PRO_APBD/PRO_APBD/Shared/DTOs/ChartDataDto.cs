using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO_APBD.Shared.DTOs;

public class ChartDataDto
{

    public DateTime Date { get; set; }
    public Double Open { get; set; }
    public Double High { get; set; }
    public Double Low { get; set; }
    public Double Close { get; set; }
    public Double Volume { get; set; }

}
