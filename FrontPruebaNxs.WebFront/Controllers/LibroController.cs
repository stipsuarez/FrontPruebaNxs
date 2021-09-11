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
    public class LibroController : Controller
    {
        string Baseurl = "http://localhost:8175/";
        private readonly INotyfService _notyf;

        public LibroController(INotyfService notyf)
        {
            _notyf = notyf;
        }
        // GET: LibroController
        public async Task<ActionResult> Index()
        {
            List<Libro> libroInfo = new List<Libro>();
            using (var client = new System.Net.Http.HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAlllibros using HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/Libros");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the libro list
                    libroInfo = JsonConvert.DeserializeObject<List<Libro>>(Response);
                }
                //returning the libro list to view
                ViewData["LibroInfo"] = libroInfo;
               
                return View(libroInfo);
            }
            
            return View();
        }

        // GET: LibroController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            Libro libro = new Libro();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAlllibros using HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/Libros/" + id);
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the libro list
                    libro = JsonConvert.DeserializeObject<Libro>(Response);
                }
                //returning the libro list to view
                ViewData["libro"] = libro;

                return View(libro);
            }
            return View();
        }

        // GET: LibroController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LibroController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                string titulo = collection.ElementAt(1).Value.ElementAt(0).ToString();
                int noPaginas= Int32.Parse(collection.ElementAt(2).Value.ElementAt(0).ToString());
                int ano = Int32.Parse(collection.ElementAt(3).Value.ElementAt(0).ToString());
                string genero = collection.ElementAt(4).Value.ElementAt(0).ToString();
                int idautor = Int32.Parse(collection.ElementAt(5).Value.ElementAt(0).ToString());
                var libro = new Libro { Titulo= titulo, Nopaginas = noPaginas, Ano= ano, Genero= genero, Idautor=idautor};

                string resp;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();

                    string a = JsonConvert.SerializeObject(libro);
                    var requestContent = new StringContent(a, Encoding.UTF8, "application/json");
                    HttpResponseMessage Res = await client.PostAsync("api/Libros", requestContent);
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    resp = Res.Content.ReadAsStringAsync().Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        libro = JsonConvert.DeserializeObject<Libro>(Response);
                        _notyf.Success("Creado Con éxito");
                       // ViewData["msj"] = libro;
                        return RedirectToAction(nameof(Index), libro);
                    }
                                     
                }
                //_notyf.Success("Error... al crear");
                if (resp.Contains("autor"))
                {
                    _notyf.Error("El autor no está registrado");
                }
                else
                {
                    _notyf.Error("No es posible registrar el libro, se alcanzó el máximo permitido.");
                }
                
                //_notyf.Error(resp);
                // ViewData["msj"] = resp;
                return View(libro);

            }
            catch (Exception e
)            {
                _notyf.Error("Error al Crear: "+e);
                return View();
            }
        }

        // GET: LibroController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Libro libro = new Libro();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Libros/" + id);
                if (Res.IsSuccessStatusCode)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    libro = JsonConvert.DeserializeObject<Libro>(Response);
                }
                ViewData["libro"] = libro;

                return View(libro);
            }
            return View();
        }

        // POST: LibroController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, IFormCollection collection)
        {
            try
            {
                int idl = Int32.Parse(collection.ElementAt(0).Value.ElementAt(0).ToString());
                string titulo = collection.ElementAt(1).Value.ElementAt(0).ToString();
                int noPaginas = Int32.Parse(collection.ElementAt(2).Value.ElementAt(0).ToString());
                int ano = Int32.Parse(collection.ElementAt(3).Value.ElementAt(0).ToString());
                string genero = collection.ElementAt(4).Value.ElementAt(0).ToString();
                int idautor = Int32.Parse(collection.ElementAt(5).Value.ElementAt(0).ToString());
                var libro = new Libro {id=idl, Titulo = titulo, Nopaginas = noPaginas, Ano = ano, Genero = genero, Idautor = idautor };

                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    var idj = new { id = id };
                    var jsonString = JsonConvert.SerializeObject(idj);
                    string a = JsonConvert.SerializeObject(libro);
                    var requestContent = new StringContent(a, Encoding.UTF8, "application/json");
                    HttpResponseMessage Res = await client.PutAsync("api/Libros/" + id, requestContent);

                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api
                        var Response = Res.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the libro list
                        libro = JsonConvert.DeserializeObject<Libro>(Response);
                    }
                    //returning the libro list to view
                    _notyf.Success("Editado con éxito");
                    return RedirectToAction(nameof(Index), libro);
                }
                _notyf.Error("Error al editar");
                return View();

            }
            catch (Exception e)
            {
                _notyf.Error("Error al editar");
                return View();
            }
        }

        // GET: LibroController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            Libro libro = new Libro();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAlllibros using HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/Libros/" + id);
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the libro list
                    libro = JsonConvert.DeserializeObject<Libro>(Response);
                }
                //returning the libro list to view
                ViewData["libro"] = libro;
                //_notyf.Success("Eliminado con Éxito!");
                return View(libro);
            }
            return View();
        }

        // POST: LibroController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {

                string libro = "";
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = await client.DeleteAsync("api/Libros/" + id);

                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api
                        var Response = Res.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the libro list
                        libro = JsonConvert.DeserializeObject<string>(Response);
                    }
                    //returning the libro list to view
                    _notyf.Success("Eliminado con Éxito!");
                    return RedirectToAction(nameof(Index), libro);
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
