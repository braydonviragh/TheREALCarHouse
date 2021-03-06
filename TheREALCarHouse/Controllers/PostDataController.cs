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
    public class PostDataController : ApiController
    {
        private TheRealCarHouseDataContext db = new TheRealCarHouseDataContext();

        //This code is mostly scaffolded from the base models and database context
        //New > WebAPIController with Entity Framework Read/Write Actions
        //Choose model "Post"
        //Choose context "Varsity Data Context"
        //Note: The base scaffolded code needs many improvements for a fully
        //functioning MVP.


        /// <summary>
        /// Gets a list or Posts in the database alongside a status code (200 OK).
        /// </summary>
        /// <returns>A list of Posts including their ID, the user information and the vehicle information</returns>
        /// <example>
        /// GET: api/PostData/GetPosts
        // GET: api/PostData
        public IEnumerable<PostDto> GetPosts()
        {
            IEnumerable<Post> posts = db.Posts.ToList();
            List<PostDto> PostDtos = new List<PostDto> { };

            foreach(var post in posts)
            {
                PostDto newPost = new PostDto
                {
                    PostID = post.PostID,
                    PostPrice = (int)post.PostPrice,
                    UserName = post.User.UserFname + " " + post.User.UserLname,
                    UserEmail = post.User.UserEmail,
                    VehicleMake = post.Vehicle.VehicleMake,
                    VehicleModel = post.Vehicle.VehicleModel,
                    VehicleYear = post.Vehicle.VehicleYear,
                    VehicleColour = post.Vehicle.VehicleColour,
                    VehicleKMs = post.Vehicle.VehicleKMs
                };
                PostDtos.Add(newPost);
            }
            
            
            
            return (PostDtos);
        }


        /// <summary>
        /// Finds a particular Post in the database with a 200 status code. If the Post is not found, return 404.
        /// </summary>
        /// <param name="id">The Post id</param>
        /// <returns>The post with the matching id. Includes the user information and the vehicle information</returns>
        // <example>
        // GET: api/PostData/FindPost/5
        // </example>
        //for finding Posts
        [HttpGet]
        [ResponseType(typeof(PostDto))]

        public IHttpActionResult FindPost(int id)
        {
            //find post data
            Post Post = db.Posts.Find(id);
            //need to do db search for Posts based off of id

            if (Post == null)
            {
                return NotFound();

            }
            //Now put into PostDto form

            PostDto PostDto = new PostDto
            {
                PostID = Post.PostID,
                PostPrice = (int)Post.PostPrice,
                UserName = Post.User.UserFname,
                UserEmail = Post.User.UserEmail,
                VehicleMake = Post.Vehicle.VehicleMake,
                VehicleModel = Post.Vehicle.VehicleModel,
                VehicleYear = Post.Vehicle.VehicleYear,
                VehicleColour = Post.Vehicle.VehicleColour,
                VehicleKMs = Post.Vehicle.VehicleKMs,

            };

            return Ok(PostDto);
        }
        /// <summary>
        /// Updates a Post in the database given information about the Post.
        /// </summary>
        /// <param name="id">The Post id</param>
        /// <param name="Post">A Post object. Received as POST data.</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/PostData/UpdatePost/5
        /// FORM DATA: Post JSON Object
        /// </example>
        /// 

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePost(int id, [FromBody] Post Post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Post.PostID)
            {
                return BadRequest();
            }

            db.Entry(Post).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
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
        /// <summary>
        /// Adds a Post to the database.
        /// </summary>
        /// <param name="Post">A Post object. Sent as POST form data.</param>
        /// <returns>status code 200 if successful. 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/Posts/AddPost
        ///  FORM DATA: Post JSON Object
        /// </example>
        [ResponseType(typeof(Post))]
        [HttpPost]
        public IHttpActionResult AddPost([FromBody] Post Post)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Posts.Add(Post);
            db.SaveChanges();

            return Ok(Post.PostID);
        }



        // GET: api/PostData/5
        [ResponseType(typeof(Post))]
        public IHttpActionResult GetPost(int id)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        // PUT: api/PostData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPost(int id, Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != post.PostID)
            {
                return BadRequest();
            }

            db.Entry(post).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
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

        // POST: api/PostData
        [ResponseType(typeof(Post))]
        public IHttpActionResult PostPost(Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Posts.Add(post);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = post.PostID }, post);
        }

        // DELETE: api/PostData/5
        [HttpPost]
        public IHttpActionResult DeletePost(int id)
        {
            Post Post = db.Posts.Find(id);
            if (Post == null)
            {
                return NotFound();
            }

            db.Posts.Remove(Post);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PostExists(int id)
        {
            return db.Posts.Count(e => e.PostID == id) > 0;
        }
    }
}