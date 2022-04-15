using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using API.Models;
using API.Data;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        DataTable tbl = new DataTable();
        Conexion conexion = new Conexion();
        string sql = "";

        [HttpGet]
        [Route("List")]
        public IActionResult List()
        {
            List<Autores> listData = new List<Autores>();

            try
            {
                sql = "SELECT * FROM AUTORES";

                tbl = conexion.QueryData(sql);
                if (tbl.Rows.Count > 0)
                {
                    foreach (DataRow row in tbl.Rows)
                    {
                        listData.Add(new Autores()
                        {
                            AUCONSECUTIVO = Convert.ToInt32(row["AUCONSECUTIVO"]),
                            AUNOMBRE = row["AUNOMBRE"].ToString(),
                            AUFECNAC = row["AUFECNAC"].ToString(),
                            AUCIUPRO = row["AUCIUPRO"].ToString(),
                            AUCORREO = row["AUCORREO"].ToString()
                        });
                    }
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = listData });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "No se han encontrado registros" });
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, response = listData });
                //throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("Get/{AUCONSECUTIVO:int}")]
        public IActionResult Get(int AUCONSECUTIVO)
        {
            List<Autores> listData = new List<Autores>();
            Autores autor = new Autores();
            try
            {
                sql = "SELECT * FROM AUTORES"; //WHERE AUCONSECUTIVO = " + AUCONSECUTIVO + "
                tbl = conexion.QueryData(sql);
                if (tbl.Rows.Count > 0)
                {
                    foreach (DataRow row in tbl.Rows)
                    {
                        listData.Add(new Autores()
                        {
                            AUCONSECUTIVO = Convert.ToInt32(row["AUCONSECUTIVO"]),
                            AUNOMBRE = row["AUNOMBRE"].ToString(),
                            AUFECNAC = row["AUFECNAC"].ToString(),
                            AUCIUPRO = row["AUCIUPRO"].ToString(),
                            AUCORREO = row["AUCORREO"].ToString()
                        });
                    }
                    autor = listData.Where(item => item.AUCONSECUTIVO == AUCONSECUTIVO).FirstOrDefault();
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = autor });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "Autor no encontrado" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, response = autor });
            }
        }

        [HttpPost]
        [Route("Post")]
        public IActionResult Post([FromBody] Autores autor)
        {
            try
            {
                int result = 0;
                bool validacion;
                validacion = validaciones(0, autor.AUNOMBRE, autor.AUFECNAC, autor.AUCIUPRO, autor.AUCORREO, "Post");

                if (validacion != false)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "Datos incompletos o mal ingresados" });
                }

                sql = "INSERT INTO AUTORES (AUNOMBRE, AUFECNAC, AUCIUPRO, AUCORREO) VALUES ('" + autor.AUNOMBRE + "',TO_DATE('" + autor.AUFECNAC + "','MM/dd/YYYY'), " +
                    "'" + autor.AUCIUPRO + "', '" + autor.AUCORREO + "')";

                result = conexion.execute(sql);
                if (result == 1)
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "Guardado" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "Autor no guardado" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("Put")]
        public IActionResult Put([FromBody] Autores autor)
        {
            try
            {
                int result = 0;
                bool validacion;
                validacion = validaciones(autor.AUCONSECUTIVO, autor.AUNOMBRE, autor.AUFECNAC, autor.AUCIUPRO, autor.AUCORREO, "Put");

                if (validacion != false)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "Datos incompletos o mal ingresados" });
                }

                sql = "UPDATE AUTORES SET AUNOMBRE = '" + autor.AUNOMBRE + "', AUFECNAC = TO_DATE('" + autor.AUFECNAC + "', 'MM/dd/YYYY'), AUCIUPRO = '" + autor.AUCIUPRO + "', " +
                    "AUCORREO = '" + autor.AUCORREO + "' WHERE AUCONSECUTIVO = '" + autor.AUCONSECUTIVO + "'";

                result = conexion.execute(sql);
                if (result == 1)
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "Eliminado" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "Autor no encontrado" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Delete/{AUCONSECUTIVO:int}")]
        public IActionResult Delete(int AUCONSECUTIVO)
        {
            try
            {
                int result = 0;
                sql = "DELETE FROM AUTORES WHERE AUCONSECUTIVO = '" + AUCONSECUTIVO + "'";

                result = conexion.execute(sql);
                if (result == 1)
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "Eliminado" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "Autor no encontrado" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        public bool validaciones(int? AUCONSECUTIVO, string? AUNOMBRE, string? AUFECNAC, string? AUCIUPRO, string? AUCORREO, string? Http)
        {
            bool val = true;

            if (Http == "Put")
            {
                if (AUCONSECUTIVO != null)
                {
                    if (AUCONSECUTIVO <= 0)
                    {
                        val = false;
                    }
                }
                else
                {
                    val = false;
                }
            }

            if (AUNOMBRE != null)
            {
                if(AUNOMBRE == "")
                {
                    val = false;
                }
            }
            else
            {
                val = false;
            }

            if (AUFECNAC != null)
            {
                if (AUFECNAC == "")
                {
                    val = false;
                }
            }
            else
            {
                val = false;
            }

            if (AUCIUPRO != null)
            {
                if (AUCIUPRO == "")
                {
                    val = false;
                }
            }
            else
            {
                val = false;
            }

            if (AUCORREO != null)
            {
                if (AUCORREO == "")
                {
                    val = false;
                }
            }
            else
            {
                val = false;
            }

            return val;
        }
    }
}
