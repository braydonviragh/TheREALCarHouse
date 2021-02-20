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
                  
                    
                };
                VehicleDtos.Add(newVehicle);
            }

            return (VehicleDtos);
        }

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
            };

            return Ok(VehicleDto);
        }









        // GET: api/Vehicles/5
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

        private bool VehicleExists(int id)
        {
            return db.Vehicles.Count(e => e.VehicleID == id) > 0;
        }
    }
}