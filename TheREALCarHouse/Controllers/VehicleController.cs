
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
    public class VehicleController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;
        static VehicleController()
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
        // GET: Vehicle
        public ActionResult Index()
        {
            return View();
        }
        
        // GET: Vehicle/List
        public ActionResult List()
        {
            string url = "vehicledata/getvehicles";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<VehicleDto> SelectedVehicles = response.Content.ReadAsAsync<IEnumerable<VehicleDto>>().Result;
                return View(SelectedVehicles);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Vehicle/Details/5
        public ActionResult Details(int id)
        {
            ShowVehicle ViewModel = new ShowVehicle();
            string url = "vehicledata/findvehicle/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                //Add api data to vehicleDto
                VehicleDto SelectedVehicle = response.Content.ReadAsAsync<VehicleDto>().Result;
                ViewModel.vechicle = SelectedVehicle;

                return View(ViewModel);
           } else
            {
                return RedirectToAction("Error");
            }
           
        }

        // GET: Vehicle/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vehicle/Create
        [HttpPost]
        public ActionResult Create(Vehicle VehicleInfo)
        {
          Debug.WriteLine(VehicleInfo.VehicleMake);
            string url = "vehicledata/addvehicle";
            Debug.WriteLine(jss.Serialize(VehicleInfo));
            HttpContent content = new StringContent(jss.Serialize(VehicleInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if(response.IsSuccessStatusCode)
            {
                int vehicleid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("List", new { id = vehicleid });
            } else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Vehicle/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateVehicle ViewModel = new UpdateVehicle();
            string url = "vehicledata/findvehicle" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //if great success, then proceed, if no success, redirect to the error page
            if (response.IsSuccessStatusCode)
            {
                //put data into the user dto
                VehicleDto SelectedVehicle = response.Content.ReadAsAsync<VehicleDto>().Result;
                ViewModel.vehicle = SelectedVehicle;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("List");
            }
        }

        // POST: Vehicle/Edit/5
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

        // GET: Vehicle/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Vehicle/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
