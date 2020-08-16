using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiDemo.Models
{
    public class FreightDetailsViewModel
    {
        public int Id { get; set; }
        public string RateCriteria { get; set; }
        public int Rate { get; set; }
        public int FreightMasterId { get; set; }
        public int CapacityId { get; set; }
    }
}