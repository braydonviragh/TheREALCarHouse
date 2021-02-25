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
        //This variable is the database access point


        private TheRealCarHouseDataContext db = new TheRealCarHouseDataContext();
        //This code is scaffolded from the base models and database context
        //New > WebAPIController with Entity Framework Read/Write Actions
        //Choose model "UserDto"
        //Choose context "The Real CarHouse Data Context"
        //Note: The base scaffolded code needs many improvements for a fully
        //functioning MVP.


        /// <summary>
        /// Gets a list or Users in the database
        /// </summary>
        /// <returns>A list of users including their ID, first name, last name, and email.</returns>
        /// <example>
        /// GET : api/userdata/getusers
        /// </example>

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
                    UserLname = user.UserLname,
                    UserEmail = user.UserEmail,
                };
                UserDtos.Add(newUser);

            }
            return (UserDtos);
        }

        //for finding users
        /// <summary>
        /// Gets a list of users in the database . 
        /// </summary>
        /// / <param name="id">The user id</param>
        /// <returns>A list of users including their ID, first name, last name, and email.</returns>

        /// <example>
        /// GET: api/UserData/GetUsers/1
        /// Retrieves all users

        /// </example>
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
                UserEmail = User.UserEmail
            };

            return Ok(UserDto);
        }
        /// <summary>
        /// Updates a user in the database given information about the user.
        /// </summary>
        /// <param name="id">The user id</param>
        /// <param name="user">A user object. Received as POST data.</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/userData/Updateuser/5
        /// FORM DATA: user JSON Object
        /// </example>
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
        /// <summary>
        /// Adds a user to the database.
        /// </summary>
        /// <param name="user">A user object. Sent as POST form data.</param>
        /// <returns>status code 200 if successful. 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/userData/Adduser
        ///  FORM DATA: user JSON Object
        /// </example>
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
        /// <summary>
        /// Deletes a user in the database
        /// </summary>
        /// <param name="id">The id of the user to delete.</param>
        /// <returns>200 if successful. 404 if not successful.</returns>
        /// <example>
        /// POST: api/userData/Deleteuser/5
        /// </example>
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

        /// <summary>
        /// Finds a user in the system. Internal use only.
        /// </summary>
        /// <param name="id">The user id</param>
        /// <returns>TRUE if the user exists, false otherwise.</returns>
        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserID == id) > 0;
        }
    }
}