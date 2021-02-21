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
        [ResponseType(typeof(Post))]
        public IHttpActionResult DeletePost(int id)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return NotFound();
            }

            db.Posts.Remove(post);
            db.SaveChanges();

            return Ok(post);
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