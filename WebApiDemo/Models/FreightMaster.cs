using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace WebApiDemo.Models
{
    public partial class FreightMaster
    {
        //Adding primary key
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //Adding Foreign Key constraints for Mode, SourceCity and DestinationCity
        public int ModeId { get; set; }
        public int SourceCityId { get; set; }
        public int DestinationCityId { get; set; }
        public int LoadUnLoadCharges { get; set; }
        public decimal INR_KG { get; set; }
        public decimal INR_CubeFeet { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [ForeignKey("ModeId")]
        public virtual Mode Mode { get; set; }

        [ForeignKey("SourceCityId")]
        public virtual City SourceCity { get; set; }

        [ForeignKey("DestinationCityId")]
        public virtual City DestinationCity { get; set; }
    }
}