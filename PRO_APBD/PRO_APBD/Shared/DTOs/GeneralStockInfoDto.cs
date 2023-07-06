using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO_APBD.Shared.DTOs;

public class GeneralStockInfoDto
{
    public string Ticker { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public string Country { get; set; }
    public string HomepageUrl { get; set; }
    public string  Currency { get; set; }
}
