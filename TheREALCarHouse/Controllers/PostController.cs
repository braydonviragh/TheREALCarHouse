﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using CarHouseThree.Models;
using CarHouseThree.Models.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace TheREALCarHouse.Controllers
{

    public class PostController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static PostController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            //change this to match your own local port number
            client.BaseAddress = new Uri("https://localhost:44383/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));


            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);

        }

        // GET: Post/List
        public ActionResult List()
        {
            string url = "postdata/getposts";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<PostDto> SelectedPosts = response.Content.ReadAsAsync<IEnumerable<PostDto>>().Result;
                return View(SelectedPosts);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Post/Details/{id}

        public ActionResult Details(int id)
        {
            ShowPost ViewModel = new ShowPost();
            string url = "postdata/findpost/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                //Add this API data to POST DTO
                PostDto SelectedPost = response.Content.ReadAsAsync<PostDto>().Result;
                ViewModel.post = SelectedPost;

                return View(ViewModel);
            }else
            {
                return RedirectToAction("Error");
            }
        }


        // GET: Post
        public ActionResult Index()
        {
            return View();
        }


        // GET: Post/Create
        public ActionResult Create()
        {
            return View();
        }

        /// POST: Post/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Post PostInfo)
        {
            
            string url = "postdata/addpost";
            Debug.WriteLine(jss.Serialize(PostInfo));
            HttpContent content = new StringContent(jss.Serialize(PostInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int Postid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = Postid });
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        // GET: Post/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Post/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Post/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "postdata/findpost/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Post data transfer object
                PostDto SelectedPost = response.Content.ReadAsAsync<PostDto>().Result;
                return View(SelectedPost);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Post/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "postdata/deletepost/" + id;
            //post body is empty
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        public ActionResult Error()
        {
            return View();
        }
    }
    
}
