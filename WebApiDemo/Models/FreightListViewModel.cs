using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;

namespace WebApiDemo.Models
{
    public class FreightListViewModel
    {
        public int Id { get; set; }
        public string FreightFor { get; set; }
        public string Mode { get; set; }
        public string SourceCity { get; set; }
        public string DestinationCity { get; set; }
    }
}