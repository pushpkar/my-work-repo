using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApiDemo.Models.Context
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<FreightDBContext>
    {
        protected override void Seed(FreightDBContext context)
        {
            base.Seed(context);

            //Insert some data in City and Capacity table
            var cities = new List<City> { new City { Name = "Weedville", IsActive = true }, new City { Name = "Brogan", IsActive = true }, new City { Name = "Curtice", IsActive = true }, new City { Name = "Norvelt", IsActive = true } };
            var capacity = new List<Capacity> { new Capacity { Name = "1 T", IsActive = true }, new Capacity { Name = "2 T", IsActive = true }, new Capacity { Name = "3 T", IsActive = true }, new Capacity { Name = "4 T", IsActive = true } };
            var mode = new List<Mode> { new Mode { Name = "Rail", IsActive = true }, new Mode { Name = "Air", IsActive = true }, new Mode { Name = "Road" , IsActive = true}};

            context.Cities.AddRange(cities); // Addining cities list
            context.Capacities.AddRange(capacity); // Addining cities list
            context.Modes.AddRange(mode); // Addining mode list
            context.SaveChanges(); // Save into database
        }
    }
}