using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarHouseThree.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string UserFname { get; set; }
        public string UserLname { get; set; }
        public string UserEmail { get; set; }


        //A user can have many vehicles
        public ICollection<Vehicle> Vehicles { get; set; }

        //A user can have many posts
        public ICollection<Post> Posts { get; set; }

    }


    public class UserDto
    {
        public int UserID { get; set; }
        public string UserFname { get; set; }
        public string UserLname { get; set; }
        public string UserEmail { get; set; }

    }
    
}