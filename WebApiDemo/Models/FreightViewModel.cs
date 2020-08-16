using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiDemo.Models
{
    public class FreightViewModel
    {
        public FreightViewModel()
        {
            freightDetailsViewModel = new List<FreightDetailsViewModel>();
        }
        public int Id { get; set; }
        public int ModeId { get; set; }
        public int SourceCityId { get; set; }
        public int DestinationCityId { get; set; }
        public int LoadUnLoadCharges { get; set; }
        public decimal INR_KG { get; set; }
        public decimal INR_CubeFeet { get; set; }
        public bool IsActive { get; set; }
        public List<FreightDetailsViewModel> freightDetailsViewModel { get; set; }
    }
}