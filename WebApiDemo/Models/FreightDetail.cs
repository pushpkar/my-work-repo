using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiDemo.Models
{
    public partial class FreightDetail
    {
        //Adding a primary key
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string RateCriteria { get; set; }
        public int Rate { get; set; }
        public int FreightMasterId { get; set; }
        public int CapacityId { get; set; }

        //Adding Foreign Key constraints for Freight Master and Capacity
        [ForeignKey("FreightMasterId")]
        public virtual FreightMaster FreightMasters { get; set; }

        [ForeignKey("CapacityId")]
        public virtual Capacity Capacities { get; set; }

    }
}