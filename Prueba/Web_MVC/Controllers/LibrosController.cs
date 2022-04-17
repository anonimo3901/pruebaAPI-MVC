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
    public class LibrosController : Controller
    {
        //URL de la API tomada de la Key en el archivo de configuracion
        string API = ConfigurationManager.AppSettings["API"] + "/Libros";
        #region List-Get
        public async Task<string> List()
        {
            var url = API + "/List";
            var httpClient = new HttpClient();

            var json = await httpClient.GetStringAsync(url);

            if (json.Length > 0)
            {
                json = json.Substring(27, json.Length - 28);
            }

            return json;
        }
        public async Task<ActionResult> Index()
        {
            List<Libros> librosList = new List<Libros>();
            var json = await List();
            if (json.Length != 0)
            {
                librosList = JsonConvert.DeserializeObject<List<Libros>>(json);
            }

            return View(librosList);
        }
        //Metodo para ser utilizado tanto en "Get" como envio a la vista "Update"
        public async Task<Libros> getLibro(int id)
        {
            var url = API + "/Get/" + id;
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync(url);
            json = json.Substring(27, json.Length - 28);

            return JsonConvert.DeserializeObject<Libros>(json);

        }
        public async Task<ActionResult> Details(int id)
        {
            Libros libro = await getLibro(id);

            return View(libro);
        }
        #endregion

        #region Post
        public ActionResult Create()
        {
            ViewBag.Message = "";

            return View();
        }
        public async Task<ActionResult> Post(Libros libro)
        {
            var url = API + "/Post";
            var httpClient = new HttpClient();
            var data = JsonConvert.SerializeObject(libro, Formatting.Indented);
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


            return View("Create", libro);
        }
        #endregion

        #region Put
        public async Task<ActionResult> Update(int id)
        {

            //Combo Autores
            List<SelectListItem> lst = new List<SelectListItem>();
            List<Autores> autoresList = await ListAutores();

            if (autoresList.Count > 0)
            {
                foreach (var autor in autoresList)
                {
                    lst.Add(new SelectListItem() { Text = autor.AUCONSECUTIVO.ToString() + " - " +autor.AUNOMBRE.ToString(), Value = autor.AUCONSECUTIVO.ToString() });
                }
            }
            

            ViewBag.Autores = autoresList;

            Libros libro = await getLibro(id);

            ViewBag.Message = "";
            return View(libro);
        }
        public async Task<ActionResult> Put(Libros libro)
        {
            var url = API + "/Put";
            var httpClient = new HttpClient();
            var data = JsonConvert.SerializeObject(libro, Formatting.Indented);
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


            return View("Update", libro);
        }
        #endregion

        #region Delete
        public async Task<ActionResult> Delete(int id)
        {
            ViewBag.Message = "";
            var url = API + "/Delete/" + id;
            var httpClient = new HttpClient();
            var httpResponse = await httpClient.DeleteAsync(url);

            List<Libros> librosList = new List<Libros>();

            if (httpResponse.IsSuccessStatusCode)
            {
                ViewBag.mensaje1 = "Eliminado con Exito";
            }
            else
            {
                ViewBag.mensaje2 = "No Eliminado";
            }

            var json = await List();
            if (json.Length != 0)
            {
                librosList = JsonConvert.DeserializeObject<List<Libros>>(json);
            }

            return View("Index", librosList);
        }
        #endregion

        #region methods
        public async Task<List<Autores>> ListAutores()
        {
            var url = ConfigurationManager.AppSettings["API"] + "/Autores/List";
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync(url);
            json = json.Substring(27, json.Length - 28);

            List<Autores> autoresList = JsonConvert.DeserializeObject<List<Autores>>(json);

            return autoresList;
        }
        #endregion
    }
}