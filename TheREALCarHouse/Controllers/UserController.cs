
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
    public class UserController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static UserController()
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
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        // GET: User/List
        public ActionResult List()
        {
            string url = "userdata/getusers";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<UserDto> SelectedUsers = response.Content.ReadAsAsync<IEnumerable<UserDto>>().Result;
                return View(SelectedUsers);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        // GET: User/Details/{id}
        public ActionResult Details(int id)
        {
            ShowUser ViewModel = new ShowUser();
            string url = "userdata/finduser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            
            if (response.IsSuccessStatusCode)
            {
                //add the API data to the user DTO
                UserDto SelectedUser = response.Content.ReadAsAsync<UserDto>().Result;
                ViewModel.user = SelectedUser;

                return View(ViewModel);

            } else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(User UserInfo)
        {
            Debug.WriteLine(UserInfo.UserFname);
            string url = "userdata/adduser";
            Debug.WriteLine(jss.Serialize(UserInfo));
            HttpContent content = new StringContent(jss.Serialize(UserInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if(response.IsSuccessStatusCode)
            {
                int userid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("List", new { id=userid});
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
           UpdateUser ViewModel = new UpdateUser();
            string url = "userdata/finduser" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //if great success, then proceed, if no success, redirect to the error page
            if (response.IsSuccessStatusCode)
            {
                //put data into the user dto
                UserDto SelectedUser = response.Content.ReadAsAsync<UserDto>().Result;
                ViewModel.user = SelectedUser;
                
                return View(ViewModel);
            } else
            {
                return RedirectToAction("List");
            }
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, User UserInfo)
        {
            Debug.WriteLine(UserInfo.UserFname);
            string url = "userdata/updateuser/" + id;
            Debug.WriteLine(jss.Serialize(UserInfo));
            HttpContent content = new StringContent(jss.Serialize(UserInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response.StatusCode);
          

                return RedirectToAction("Details", new { id = id });
            
        }

        // GET: User/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "userdata/finduser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into user data transfer object
                UserDto SelectedUser = response.Content.ReadAsAsync<UserDto>().Result;
                return View(SelectedUser);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: user/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "userdata/deleteuser/" + id;
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
    }
}
