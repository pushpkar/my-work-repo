using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using WebApiDemo.Models;

namespace WebApiDemo.Models.Context
{
    public partial class FreightDBContext : DbContext
    {
        public FreightDBContext() : base("FreightDBConnection") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
        public virtual DbSet<FreightMaster> FreightMasters { get; set; }
        public virtual DbSet<FreightDetail> FreightDetails { get; set; }
        public virtual DbSet<Capacity> Capacities { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Mode> Modes { get; set; }

       

    }
}