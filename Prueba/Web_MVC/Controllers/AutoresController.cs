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

namespace Web_MVC.Controllers
{
    public class AutoresController : Controller
    {
        #region Get
        public async Task<ActionResult> Index()
        {
            var url = ConfigurationManager.AppSettings["API"] + "/Autores/List";
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync(url);
            json = json.Substring(27, json.Length - 28);

            List<Autores> autoresList = JsonConvert.DeserializeObject<List<Autores>>(json);

            return View(autoresList);
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
            var url = ConfigurationManager.AppSettings["API"] + "/Autores/Post";
            var httpClient = new HttpClient();
            
            var json = "[{ " + "\"aunombre\":\"" + autor.AUNOMBRE + "\",\"aufecnac\":\"" + autor.AUFECNAC + "\",\"auciupro\":\"" + autor.AUCIUPRO + "\",\"aucorreo\":\"" + autor.AUCORREO + "\"" + " }]";

            //var data = JsonSerializer.Serialize<Autores>(post);

            HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "aplication/json");


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
        public async Task<ActionResult> Update()
        {
            var url = ConfigurationManager.AppSettings["API"];
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync(url + "/Autores/List");
            json = json.Substring(27, json.Length - 28);

            List<Autores> autoresList = JsonConvert.DeserializeObject<List<Autores>>(json);

            return View(autoresList);
        }
        #endregion

    }
}