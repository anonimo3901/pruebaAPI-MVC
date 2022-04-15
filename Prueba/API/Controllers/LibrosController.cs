﻿using Microsoft.AspNetCore.Http;
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
    public class LibrosController : ControllerBase
    {
        DataTable tbl = new DataTable();
        Conexion conexion = new Conexion();
        string sql = "";

        [HttpGet]
        [Route("List")]
        public IActionResult List()
        {
            List<Libros> listData = new List<Libros>();

            try
            {
                sql = "SELECT * FROM LIBROS";

                tbl = conexion.QueryData(sql);
                if (tbl.Rows.Count > 0)
                {
                    foreach (DataRow row in tbl.Rows)
                    {
                        listData.Add(new Libros()
                        {
                            LICONSECUTIVO = Convert.ToInt32(row["LICONSECUTIVO"]),
                            LITITULO = row["LITITULO"].ToString(),
                            LIANO = Convert.ToInt32(row["LIANO"]),
                            LIGENERO = row["LIGENERO"].ToString(),
                            LINUMPAG = Convert.ToInt32(row["LINUMPAG"]),
                            LIAUTOR = Convert.ToInt32(row["LIAUTOR"])
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
            }
        }

        [HttpGet]
        [Route("Get/{LICONSECUTIVO:int}")]
        public IActionResult Get(int LICONSECUTIVO)
        {
            List<Libros> listData = new List<Libros>();
            Libros libro = new Libros();
            try
            {
                sql = "SELECT * FROM LIBROS"; 
                tbl = conexion.QueryData(sql);
                if (tbl.Rows.Count > 0)
                {
                    foreach (DataRow row in tbl.Rows)
                    {
                        listData.Add(new Libros()
                        {
                            LICONSECUTIVO = Convert.ToInt32(row["LICONSECUTIVO"]),
                            LITITULO = row["LITITULO"].ToString(),
                            LIANO = Convert.ToInt32(row["LIANO"]),
                            LIGENERO = row["LIGENERO"].ToString(),
                            LINUMPAG = Convert.ToInt32(row["LINUMPAG"]),
                            LIAUTOR = Convert.ToInt32(row["LIAUTOR"])
                        });
                    }
                    libro = listData.Where(item => item.LICONSECUTIVO == LICONSECUTIVO).FirstOrDefault();
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = libro });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "Libro no encontrado" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, response = libro });
            }
        }

        [HttpPost]
        [Route("Post")]
        public IActionResult Post([FromBody] Libros libro)
        {
            try
            {
                int result = 0;
                bool validacion;
                validacion = validaciones(0, libro.LITITULO, libro.LIANO, libro.LIGENERO, libro.LINUMPAG, libro.LIAUTOR, "Post");

                if (validacion != false)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "Datos incompletos o mal ingresados" });
                }

                sql = "INSERT INTO LIBROS (LITITULO, LIANO, LIGENERO, LINUMPAG, LIAUTOR) VALUES " +
                    "('" + libro.LITITULO + "', '" + libro.LIANO + "', '" + libro.LIGENERO + "', '" + libro.LINUMPAG + "', '" + libro.LIAUTOR + "')";

                result = conexion.execute(sql);
                if (result == 1)
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "Guardado" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "Libro no guardado" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("Put")]
        public IActionResult Put([FromBody] Libros libro)
        {
            try
            {
                int result = 0;
                bool validacion;
                validacion = validaciones(libro.LICONSECUTIVO, libro.LITITULO, libro.LIANO, libro.LIGENERO, libro.LINUMPAG, libro.LIAUTOR, "Put");

                if (validacion != false)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "Datos incompletos o mal ingresados" });
                }

                sql = "UPDATE LIBROS SET LITITULO = '" + libro.LITITULO + "', LIANO = '" + libro.LIANO + "',LIGENERO = '" + libro.LIGENERO + "', " +
                    "LINUMPAG = '" + libro.LINUMPAG + "', LIAUTOR = '" + libro.LIAUTOR + "' WHERE LICONSECUTIVO = '" + libro.LICONSECUTIVO + "'";

                result = conexion.execute(sql);
                if (result == 1)
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "Eliminado" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "Libro no encontrado" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Delete/{LICONSECUTIVO:int}")]
        public IActionResult Delete(int LICONSECUTIVO)
        {
            try
            {
                int result = 0;
                sql = "DELETE FROM LIBROS WHERE LICONSECUTIVO = '" + LICONSECUTIVO + "'";

                result = conexion.execute(sql);
                if (result == 1)
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "Eliminado" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "Libro no encontrado" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }
        public bool validaciones(int? LICONSECUTIVO, string? LITITULO, int? LIANO, string? LIGENERO, int? LINUMPAG, int? LIAUTOR, string? Http)
        {
            bool val = true;

            if (Http == "Put")
            {
                if (LICONSECUTIVO != null)
                {
                    if (LICONSECUTIVO <= 0)
                    {
                        val = false;
                    }
                }
                else
                {
                    val = false;
                }
            }

            if (LITITULO != null)
            {
                if (LITITULO == "")
                {
                    val = false;
                }
            }
            else
            {
                val = false;
            }

            if (LIANO != null)
            {
                //if (LIANO == 0)
                //{
                //    val = false;
                //}
            }
            else
            {
                val = false;
            }

            if (LIGENERO != null)
            {
                if (LIGENERO == "")
                {
                    val = false;
                }
            }
            else
            {
                val = false;
            }

            if (LINUMPAG != null)
            {
                if (LINUMPAG <= 0)
                {
                    val = false;
                }
            }
            else
            {
                val = false;
            }

            if (LIAUTOR != null)
            {
                if (LIAUTOR <= 0)
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