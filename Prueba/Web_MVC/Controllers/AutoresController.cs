using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using API.Models;
using Newtonsoft.Json;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace Web_MVC.Controllers
{
    public class AutoresController : Controller
    {
        string API = ConfigurationManager.AppSettings["API"] + "/Autores";
        #region List-Get
        public async Task<ActionResult> Index()
        {
            var url = API + "/List";
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync(url);
            json = json.Substring(27, json.Length - 28);

            List<Autores> autoresList = JsonConvert.DeserializeObject<List<Autores>>(json);

            return View(autoresList);
        }
        public async Task<Autores> getAutor(int id)
        {
            var url = API + "/Get/" + id;
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync(url);
            json = json.Substring(27, json.Length - 28);

            return JsonConvert.DeserializeObject<Autores>(json);

        }
        public async Task<ActionResult> Details(int id)
        {
            Autores autor = await getAutor(id);

            return View(autor);
        }
        #endregion

        #region Post
        public ActionResult Create()
        {
            ViewBag.Message = "";

            return View();
        }
        public async Task<ActionResult> Post(Autores autor)
        {
            var url = API + "/Post";
            var httpClient = new HttpClient();
            var data = JsonConvert.SerializeObject(autor,Formatting.Indented);
            HttpContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");

            var httpResponse = await httpClient.PostAsync(url, content);


            if (httpResponse.IsSuccessStatusCode)
            {
                ViewBag.mensaje1 = "Guardado con Exito";
            }
            else
            {
                ViewBag.mensaje2 = "No guardado";
            }

            
            return View("Create", autor);
        }
        #endregion

        #region Put
        public async Task<ActionResult> Update(int id)
        {
            
            Autores autor = await getAutor(id);

            ViewBag.Message = "";
            return View(autor);
        }
        public async Task<ActionResult> Put(Autores autor)
        {
            var url = API + "/Put";
            var httpClient = new HttpClient();
            var data = JsonConvert.SerializeObject(autor, Formatting.Indented);
            HttpContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");

            var httpResponse = await httpClient.PutAsync(url, content);


            if (httpResponse.IsSuccessStatusCode)
            {
                ViewBag.mensaje1 = "Actualizado con Exito";
            }
            else
            {
                ViewBag.mensaje2 = "No Actualizado";
            }


            return View("Update", autor);
        }
        #endregion

        #region Delete
        public async Task<ActionResult> Delete(int id)
        {
            ViewBag.Message = "";
            var url = API + "/Delete/" + id;
            var httpClient = new HttpClient();
            var httpResponse = await httpClient.DeleteAsync(url);


            if (httpResponse.IsSuccessStatusCode)
            {
                ViewBag.mensaje1 = "Eliminado con Exito";
            }
            else
            {
                ViewBag.mensaje2 = "No Eliminado";
            }


            return View("Index");
        }
        #endregion

    }
}