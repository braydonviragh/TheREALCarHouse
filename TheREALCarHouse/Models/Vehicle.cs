using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CarHouseThree.Models
{
    public class Vehicle
    {
        [Key]
        public int VehicleID { get; set; }

        public string VehicleMake { get; set; }

        public string VehicleModel { get; set; }
        public string VehicleYear { get; set; }
        public string VehicleColour { get; set; }
        public string VehicleKMs { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public virtual User User { get; set; }


    }
    public class VehicleDto
    {
        public int VehicleID { get; set; }

        public string VehicleMake { get; set; }

        public string UserName { get; set; }

        public string VehicleModel { get; set; }
        public string VehicleYear { get; set; }
        public string VehicleColour { get; set; }
        public string VehicleKMs { get; set; }

    }


}
