using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using FrontPruebaNxs.WebFront.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FrontPruebaNxs.WebFront.Controllers
{
    public class AutorController : Controller
    {
        string Baseurl = "http://localhost:8175/";
        private readonly INotyfService _notyf;

        public AutorController(INotyfService notyf)
        {
            _notyf = notyf;
        }

        // GET: AutorController
        public async Task<ActionResult> Index()
        {
            List<Autor> AutorInfo = new List<Autor>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllAutors using HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/Autores");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Autor list
                    AutorInfo = JsonConvert.DeserializeObject<List<Autor>>(Response);
                }
                //returning the Autor list to view
                ViewData["AutorInfo"] = AutorInfo;
                
                return View(AutorInfo);
            }
            return View();
        }

        // GET: AutorController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            Autor autor = new Autor();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllAutors using HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/Autores/"+id);
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Autor list
                    autor = JsonConvert.DeserializeObject<Autor>(Response);
                }
                //returning the Autor list to view
                ViewData["Autor"] = autor;

                return View(autor);
            }
            return View();
        }

        // GET: AutorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AutorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                string nombre = collection.ElementAt(0).Value.ElementAt(0).ToString();
                DateTime fechanacimiento = DateTime.Parse(collection.ElementAt(1).Value.ElementAt(0).ToString());
                string ciudad = collection.ElementAt(2).Value.ElementAt(0).ToString();
                string email  = collection.ElementAt(3).Value.ElementAt(0).ToString();

                var autor = new Autor { Nombre=nombre, Fechanacimiento=fechanacimiento, Ciudad=ciudad, Email=email };
                
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    
                    string a = JsonConvert.SerializeObject(autor);
                    var requestContent = new StringContent(a, Encoding.UTF8, "application/json");
                    HttpResponseMessage Res = await client.PostAsync("api/Autores", requestContent);
                   if (Res.IsSuccessStatusCode)
                    {
                        var Response = Res.Content.ReadAsStringAsync().Result;
                        autor = JsonConvert.DeserializeObject<Autor>(Response);
                    }
                    _notyf.Success("Creado Con éxito");
                    return RedirectToAction(nameof(Index), autor);
                }
                _notyf.Success("Error al crear Autor");
                return View();
                
            }
            catch
            {
                _notyf.Success("Error al crear Autor catch");
                return View();
            }
        }

        // GET: AutorController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Autor autor = new Autor();
            using (var client = new HttpClient())
            {
                 client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Autores/" + id);
                if (Res.IsSuccessStatusCode)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    autor = JsonConvert.DeserializeObject<Autor>(Response);
                }
                ViewData["Autor"] = autor;

                return View(autor);
            }
            return View();
        }

        // POST: AutorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, IFormCollection collection)
        {
            try
            {
                int ida = Int32.Parse(collection.ElementAt(0).Value.ElementAt(0).ToString());
                string nombre = collection.ElementAt(1).Value.ElementAt(0).ToString();
                DateTime fechanacimiento = DateTime.Parse(collection.ElementAt(2).Value.ElementAt(0).ToString());
                string ciudad = collection.ElementAt(3).Value.ElementAt(0).ToString();
                string email = collection.ElementAt(4).Value.ElementAt(0).ToString();

                var autor = new Autor {id=ida, Nombre = nombre, Fechanacimiento = fechanacimiento, Ciudad = ciudad, Email = email };

                using (var client = new HttpClient())
                {
                  
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    
                    string a = JsonConvert.SerializeObject(autor);
                    var requestContent = new StringContent(a, Encoding.UTF8, "application/json");
                    HttpResponseMessage Res = await client.PutAsync("api/Autores/"+id, requestContent);
          
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api
                        var Response = Res.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Autor list
                        autor = JsonConvert.DeserializeObject<Autor>(Response);
                    }
                    //returning the Autor list to view
                    _notyf.Success("Editado con éxito");
                    return RedirectToAction(nameof(Index), autor);
                }
                _notyf.Error("Error al editar after usig");
                return View(autor);

            }
            catch
            {
                _notyf.Error("Error al editar");
                return View();
            }
        }

        // GET: AutorController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            Autor autor = new Autor();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllAutors using HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/Autores/" + id);
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Autor list
                    autor = JsonConvert.DeserializeObject<Autor>(Response);
                }
                //returning the Autor list to view
                ViewData["Autor"] = autor;
                //_notyf.Success("Eliminado");
                return View(autor);
            }
            return View();
        }

        // POST: AutorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {

                string autor = "";
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //string a = JsonConvert.SerializeObject(autor, (Formatting)id);
                    //var requestContent = new StringContent(a, Encoding.UTF8, "application/json");
                    HttpResponseMessage Res = await client.DeleteAsync("api/Autores/" + id);

                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api
                        var Response = Res.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Autor list
                       autor = JsonConvert.DeserializeObject<string>(Response);
                    }
                    //returning the Autor list to view

                    return RedirectToAction(nameof(Index), autor);
                }
                return View();

            }
            catch
            {
                return View();
            }
        }
    }
}
