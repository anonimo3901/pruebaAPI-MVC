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
        string API = ConfigurationManager.AppSettings["API"];
        #region List-Get
        public async Task<ActionResult> Index()
        {
            var url = API + "/Libros/List";
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync(url);
            json = json.Substring(27, json.Length - 28);

            List<Libros> librosList = JsonConvert.DeserializeObject<List<Libros>>(json);

            return View(librosList);
        }
        public async Task<Libros> getLibro(int id)
        {
            var url = API + "/Libros/Get/" + id;
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
       

    }
}