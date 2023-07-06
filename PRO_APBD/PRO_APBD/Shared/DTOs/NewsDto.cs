using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO_APBD.Shared.DTOs
{
    public class NewsDto
    {
        public string PublisherName { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public string AritcleUrl { get; set; }
    }
}
