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
    public class UserDataController : ApiController
    {
        private TheRealCarHouseDataContext db = new TheRealCarHouseDataContext();

        // GET: api/Users/GetUsers
        public IEnumerable<UserDto> GetUsers()
        {
            IEnumerable<User> users = db.Users.ToList();
            List<UserDto> UserDtos = new List<UserDto> { };
            foreach(var user in users)
            {
                UserDto newUser = new UserDto
                {
                    UserID = user.UserID,
                    UserFname = user.UserFname,
                    UserLname = user.UserLname
                };
                UserDtos.Add(newUser);

            }
            return (UserDtos);
        }

        //for finding users
        [HttpGet]
        [ResponseType(typeof(UserDto))]
        public IHttpActionResult FindUser(int id)
        {
            //find the user data
            User User = db.Users.Find(id);
            //does db search and if results come back, 
            //We continue, if not, return error not found.
            if (User == null)
            {
                return NotFound();
            }

            //Put into Dto form
            UserDto UserDto = new UserDto
            {
                UserID = User.UserID,
                UserFname = User.UserFname,
                UserLname = User.UserLname,
            };

            return Ok(UserDto);
        }

        //API action needed to update user
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateUser(int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != user.UserID)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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



            // GET: api/Users/GetUser/5
            [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        //Add a user 
        [ResponseType(typeof(User))]
        [HttpPost]
        public IHttpActionResult AddUser([FromBody] User user)
        {
            //If the model is valid
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //add then save onto db 
            db.Users.Add(user);
            db.SaveChanges();
            return Ok(user.UserID);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserID)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.UserID }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserID == id) > 0;
        }
    }
}