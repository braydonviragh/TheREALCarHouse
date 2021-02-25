using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CarHouseThree.Models;

namespace TheREALCarHouse.Controllers
{
    public class VehicleDataController : ApiController
    {
        private TheRealCarHouseDataContext db = new TheRealCarHouseDataContext();
        //This code is mostly scaffolded from the base models and database context
        //New > WebAPIController with Entity Framework Read/Write Actions
        //Choose model "Vehicle"
        //Choose context "The Real Car House Data Context"
        //Note: The base scaffolded code needs many improvements for a fully
        //functioning MVP.


        /// <summary>
        /// Gets a list or Vehicles in the database alongside a status code (200 OK).
        /// </summary>
        /// <returns>A list of Vehicles including their ID, make, model, year, KM's, colour and, User Email & UserName and URL.</returns>
        /// <example>
        /// GET: api/VehicleData/GetVehicles
        /// </example>
        // GET: api/Vehicledata/GetVehicles
        public IEnumerable<VehicleDto> GetVehicles()
        {
            IEnumerable<Vehicle> vehicles = db.Vehicles.ToList();
            List<VehicleDto> VehicleDtos = new List<VehicleDto> { };
            foreach(var vehicle in vehicles)
            {
                VehicleDto newVehicle = new VehicleDto
                {
                    VehicleID = vehicle.VehicleID,

                    VehicleMake = vehicle.VehicleMake,
                    VehicleModel = vehicle.VehicleModel,
                    VehicleYear = vehicle.VehicleYear,
                    VehicleColour = vehicle.VehicleColour,
                    VehicleKMs = vehicle.VehicleKMs,
                    UserName = vehicle.User.UserFname,
                    UserEmail = vehicle.User.UserEmail
                  
                    
                };
                VehicleDtos.Add(newVehicle);
            }

            return (VehicleDtos);
        }
        /// <summary>
        /// Finds a particular Vehicle in the database with a 200 status code. If the Vehicle is not found, return 404.
        /// </summary>
        /// <param name="id">The Vehicle id</param>
        /// <returns>Information about the Vehicle, including Vehicle id, make, model, colour, year, user name and user email</returns>
        // <example>
        // GET: api/VehicleData/FindVehicle/5
        // </example>
        //for finding vehicles
        [HttpGet]
        [ResponseType(typeof(VehicleDto))]
        public IHttpActionResult FindVehicle(int id)
        {
            //find the Vehicle data
            Vehicle Vehicle = db.Vehicles.Find(id);
            //does db search and if results come back, 
            //We continue, if not, return error not found.
            if (Vehicle == null)
            {
                return NotFound();
            }

            //Put into Dto form
            
            VehicleDto VehicleDto = new VehicleDto
            {
                VehicleID = Vehicle.VehicleID,
                VehicleMake = Vehicle.VehicleMake,
                VehicleModel = Vehicle.VehicleModel,
                VehicleYear = Vehicle.VehicleYear,
                VehicleColour = Vehicle.VehicleColour,
                VehicleKMs = Vehicle.VehicleKMs,
                UserName = Vehicle.User.UserFname,
                UserEmail = Vehicle.User.UserEmail,
            };

            return Ok(VehicleDto);
        }







        /// <summary>
        /// Finds a particular Vehicle in the database with a 200 status code. If the Vehicle is not found, return 404.
        /// </summary>
        /// <param name="id">The Vehicle id</param>
        /// <returns>Information about the Vehicle, including Vehicle id, make, model, colour, year, user name and user email</returns>
        // <example>
        // GET: api/VehicleData/FindVehicle/5
        // </example>
        //for finding vehicles

        // GET: api/VehicleData/GetVehicle/5
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult GetVehicle(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return NotFound();
            }


            return Ok(vehicle);
        }
        /// <summary>
        /// Adds a Vehicle to the database.
        /// </summary>
        /// <param name="Vehicle">A Vehicle object. Sent as POST form data.</param>
        /// <returns>status code 200 if successful. 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/VehicleData/AddVehicle
        ///  FORM DATA: Vehicle JSON Object
        /// </example>
        //Add a vehicle 
        [ResponseType(typeof(Vehicle))]
        [HttpPost]
        public IHttpActionResult AddVehicle([FromBody] Vehicle vehicle)
        {
            //If the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //add then save onto db 
            db.Vehicles.Add(vehicle);
            db.SaveChanges();
            return Ok(vehicle.VehicleID);
        }

        // PUT: api/Vehicles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVehicle(int id, Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vehicle.VehicleID)
            {
                return BadRequest();
            }

            db.Entry(vehicle).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Vehicles
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult PostVehicle(Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Vehicles.Add(vehicle);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = vehicle.VehicleID }, vehicle);
        }

        /// <summary>
        /// Deletes a Vehicle in the database
        /// </summary>
        /// <param name="id">The id of the Vehicle to delete.</param>
        /// <returns>200 if successful. 404 if not successful.</returns>
        /// <example>
        /// POST: api/VehicleData/DeleteVehicle/5
        /// </example>
        // DELETE: api/Vehicles/5
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult DeleteVehicle(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            db.Vehicles.Remove(vehicle);
            db.SaveChanges();

            return Ok(vehicle);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        /// <summary>
        /// Finds a Vehicle in the system. Internal use only.
        /// </summary>
        /// <param name="id">The Vehicle id</param>
        /// <returns>TRUE if the Vehicle exists, false otherwise.</returns>
        private bool VehicleExists(int id)
        {
            return db.Vehicles.Count(e => e.VehicleID == id) > 0;
        }
    }
}