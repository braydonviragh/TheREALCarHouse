using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarHouseThree.Models
{
    public class Post
    {
        [Key]
        public int PostID { get; set; }

        public double PostPrice { get; set; }


        [ForeignKey("User")]
        public int UserID { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Vehicle")]
        public int VehicleID { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
    public class PostDto
    {
        public int PostID { get; set; }

        public int PostPrice { get; set; }
  
        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleYear { get; set; }
        public string VehicleColour { get; set; }
        public string VehiclKMs { get; set; }

    }
}