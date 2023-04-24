using HUSI_SIISA.Models.Request;
using HUSI_SIISA.Models.Response;
using HUSI_SIISA.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NLog;
using System.Data;
using HUSI_SIISA.DBContext;

namespace HUSI_SIISA.Controllers
{
    /// <summary>
    /// Implementacion del Servicio Inventarios. 
    /// Las operaciones:    actualizarDevoluciones
    ///                     actualizarpedidos
    ///                     actualizarRemisiones
    ///                     cargarDevoluciones
    ///                     cargarRemisiones
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class InventarioController : ControllerBase
    {
        InventarioResponse inventarioResponse = new();
        private static Logger logSahico = LogManager.GetCurrentClassLogger();

        // POST: api/Inventario/CrearSolicitud
        /// <summary>
        /// Operacion Creacion de Solicitud
        /// </summary>
        /// <param name="inventarioRequest">Estructura con los parametros para consumo del servicio</param>
        /// <returns>Estructura de datos para InventarioResponse</returns>
        /// <remarks>
        /// Sample request:
        ///     POST api/Inventario/CrearSolicitud
        ///     {
        ///         "atencion": "",
        ///         "orden": "",
        ///         "idUbicacion": "",
        ///         "idEsquema": ""
        ///     }
        /// </remarks>
        [HttpPost]
        [Route("CrearSolicitud")]
        public ActionResult CrearSolicitud([FromBody] InventarioRequest inventarioRequest)
        {
            //recepcion
            Utilidades utilLocal = new Utilidades();
            logSahico.Info("Mensaje de PEDIDO  Recibido:" + inventarioRequest);


            //validacion de ddatos
            inventarioResponse = ValidarEntrada(inventarioRequest);
            if (inventarioResponse.Resultado.Equals("02"))
            {
                logSahico.Info($" No se puede procesar el mensaje porque falta informacion ::	{inventarioResponse.Mensaje}");
                return NotFound(inventarioResponse);
            }

            try
            {
                int idSolicitudS = 0;
#pragma warning disable CS8604 // Posible argumento de referencia nulo
                var productosAgrupados = inventarioRequest.Productos.GroupBy(x => x.DiaCiclo);
#pragma warning restore CS8604 // Posible argumento de referencia nulo
                foreach (var group in productosAgrupados)
                {
                    var listProductos = new List<Utilities.Producto>();
                    var diaCiclo = 0;
                    var fechaDispensasion = DateTime.Now;

                    Console.WriteLine("dia cilo" + group.Key + ":");
                    foreach (var p in group)
                    {
                        Console.WriteLine("* " + p.IdProducto);
                        var newP = new Utilities.Producto()
                        {
                            Cantidad = p.Cantidad,
                            IdPresentacion = p.IdPresentacion,
                            IdProducto = p.IdProducto,
                            ValCosto = p.ValCosto,
                            ValorC = p.ValorC
                        };
                        listProductos.Add(newP);
                        diaCiclo = p.DiaCiclo;
                        fechaDispensasion = p.FechaDispensacion;
                    }

                    logSahico.Info("Creando pedido :: diaCiclo " + diaCiclo + " fecha dispensacion " + fechaDispensasion.ToString("yyyy-MM-dd HH:mm:ss"));

                    DBConnection conn = new();
                    using (SqlConnection conexion = new(conn.getCs()))
                    {
                        conexion.Open(); // abro la conection
                        SqlCommand cmd = new("CrearSolicitudesSAHICO", conexion); // Creo el comando que se ejecutara en este caso el SP
                        cmd.CommandType = CommandType.StoredProcedure; // Especifico que el comando sera un Store Procedure
                        SqlParameter paramCodRetorno = new("@idSolicitudSAHI", SqlDbType.Int);
                        paramCodRetorno.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(paramCodRetorno);

                        string xmlProductos = utilLocal.GetXMLFromObject(listProductos);

                        // Declaro los parametros que seran pasado al SP
                        cmd.Parameters.AddWithValue("@idSolicitudSAHICO", inventarioRequest.IdSolicitudSAHICO);
                        cmd.Parameters.AddWithValue("@IdAtencion", inventarioRequest.IdAtencion);
                        cmd.Parameters.AddWithValue("@idTipoSolicitud", inventarioRequest.IdTipoSolicitud);
                        cmd.Parameters.AddWithValue("@idUbicacionRec ", inventarioRequest.IdUbicacionRec);
                        cmd.Parameters.AddWithValue("@idUbicacionEnt ", inventarioRequest.IdUbicacionEnt);
                        cmd.Parameters.AddWithValue("@idPersonal", inventarioRequest.IdPersonal);
                        cmd.Parameters.AddWithValue("@fecRegistro", inventarioRequest.FecRegistro);
                        cmd.Parameters.AddWithValue("@fechaDispensacion", fechaDispensasion.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@diaCiclo", diaCiclo);
                        cmd.Parameters.AddWithValue("@xmlProductos", xmlProductos);

                        cmd.ExecuteNonQuery(); // ejecuto el SP
                        idSolicitudS += Convert.ToInt32(cmd.Parameters["@idSolicitudSAHI"].Value);
                    }
                }

                if (idSolicitudS == 0)
                {
                    inventarioResponse.ResultadoTransaccion = "00";
                    inventarioResponse.Mensaje = "El consecutivo: " + inventarioRequest.IdSolicitudSAHICO + " ya fue enviado.";

                    return BadRequest(inventarioResponse);
                }
                else
                {
                    inventarioResponse.ResultadoTransaccion = "01";
                    inventarioResponse.ConsecutivoRegistro = idSolicitudS;
                    inventarioResponse.Mensaje = "Resultado exitoso";

                    return Ok(inventarioResponse);
                }
            }
            //excepcion
            catch (SqlException sqlEx2)
            {
                ErrorResponse errorResponse = new();
                logSahico.Error("Error en Base de Datos :: " + sqlEx2.Message);
                logSahico.Info("Se ha presentado una Excepcion:" + sqlEx2.InnerException);
                logSahico.Info("Se ha presentado una Excepcion:" + sqlEx2.StackTrace);
                errorResponse.Codigo = 500;
                errorResponse.Mensaje = "Error con la operacion en la base de datos, comuníquese con el administrador.";
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
            catch (Exception ex3)
            {
                ErrorResponse errorResponse = new();
                logSahico.Error("Error en Base de Datos :: " + ex3.Message);
                logSahico.Info("Se ha presentado una Excepcion:" + ex3.InnerException);
                logSahico.Info("Se ha presentado una Excepcion:" + ex3.StackTrace);
                errorResponse.Codigo = 500;
                errorResponse.Mensaje = "Se ha presentado una Excepcion General No controlada, comuníquese con el administrador.";
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        // POST: api/Inventario/CrearDevolucion
        /// <summary>
        /// Operacion Creacion de Solicitud de Devolucion
        /// </summary>
        /// <param name="inventarioRequest">Estructura con los parametros para consumo del servicio</param>
        /// <returns>Estructura de datos para InventarioResponse</returns>
        /// <remarks>
        /// Sample request:
        ///     POST api/Inventario/CrearDevolucion
        ///     {
        ///         "atencion": "",
        ///         "orden": "",
        ///         "idUbicacion": "",
        ///         "idEsquema": ""
        ///     }
        /// </remarks>
        [HttpPost]
        [Route("CrearDevolucion")]
        public ActionResult CrearDevolucion([FromBody] InventarioRequest inventarioRequest)
        {
            //recepcion
            Utilidades utilLocal = new Utilidades();
            logSahico.Info("Mensaje de DEVOLUCION  Recibido:" + inventarioRequest);


            //validacion de ddatos
            inventarioResponse = ValidarEntrada(inventarioRequest);
            inventarioResponse.ConsecutivoRegistro = 0;
            if (inventarioResponse.Resultado.Equals("02"))
            {
                logSahico.Info($" No se puede procesar el mensaje porque falta informacion :: {inventarioResponse.Mensaje}");
                return NotFound(inventarioResponse);
            }

            try
            {
                int idSolicitudS = 0;
                DBConnection conn = new();
                using (SqlConnection conexion = new(conn.getCs()))
                {
                    conexion.Open(); // abro la conection
                    SqlCommand cmd = new("CrearDevolucionesSAHICO", conexion); // Creo el comando que se ejecutara en este caso el SP
                    cmd.CommandType = CommandType.StoredProcedure; // Especifico que el comando sera un Store Procedure
                    SqlParameter paramCodRetorno = new SqlParameter("@idDevolucionSAHI", SqlDbType.Int);
                    paramCodRetorno.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(paramCodRetorno);

#pragma warning disable CS8604 // Posible argumento de referencia nulo
                    string xmlProductos = utilLocal.GetXMLFromObject(inventarioRequest.Productos);
#pragma warning restore CS8604 // Posible argumento de referencia nulo

                    // Declaro los parametros que seran pasado al SP
                    cmd.Parameters.AddWithValue("@idDevolucionSAHICO", inventarioRequest.IdDevolucionSAHICO);
                    cmd.Parameters.AddWithValue("@idAtencion", inventarioRequest.IdAtencion);
                    cmd.Parameters.AddWithValue("@idTipoProducto", inventarioRequest.IdTipoProducto);
                    cmd.Parameters.AddWithValue("@idUbicacionRec", inventarioRequest.IdUbicacionRec);
                    cmd.Parameters.AddWithValue("@idUbicacionEnt", inventarioRequest.IdUbicacionEnt);
                    cmd.Parameters.AddWithValue("@idPersonal", inventarioRequest.IdPersonal);
                    cmd.Parameters.AddWithValue("@fecRegistro", inventarioRequest.FecRegistro);
                    cmd.Parameters.AddWithValue("@idCausa", inventarioRequest.IdCausa);
                    cmd.Parameters.AddWithValue("@OtraCausa", inventarioRequest.OtraCausa);
                    cmd.Parameters.AddWithValue("@xmlProductos", xmlProductos);

                    cmd.ExecuteNonQuery(); // ejecuto el SP
                    idSolicitudS += Convert.ToInt32(cmd.Parameters["@idDevolucionSAHI"].Value);
                }

                if (idSolicitudS == 0)
                {
                    inventarioResponse.ResultadoTransaccion = "00";
                    inventarioResponse.Mensaje = "El consecutivo: " + inventarioRequest.IdDevolucionSAHICO + " ya fue enviado.";

                    return BadRequest(inventarioResponse);
                }
                else
                {
                    inventarioResponse.ResultadoTransaccion = "01";
                    inventarioResponse.ConsecutivoRegistro = idSolicitudS;
                    inventarioResponse.Mensaje = "Transacción exitosa";

                    return Ok(inventarioResponse);
                }
            }
            //excepcion
            catch (SqlException sqlEx2)
            {
                ErrorResponse errorResponse = new();
                logSahico.Error("Error en Base de Datos :: " + sqlEx2.Message);
                logSahico.Info("Se ha presentado una Excepcion:" + sqlEx2.InnerException);
                logSahico.Info("Se ha presentado una Excepcion:" + sqlEx2.StackTrace);
                errorResponse.Codigo = 500;
                errorResponse.Mensaje = "Error con la operacion en la base de datos, comuníquese con el administrador.";
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
            catch (Exception ex3)
            {
                ErrorResponse errorResponse = new();
                logSahico.Error("Error en Base de Datos :: " + ex3.Message);
                logSahico.Info("Se ha presentado una Excepcion:" + ex3.InnerException);
                logSahico.Info("Se ha presentado una Excepcion:" + ex3.StackTrace);
                errorResponse.Codigo = 500;
                errorResponse.Mensaje = "Se ha presentado una Excepcion General No controlada, comuníquese con el administrador.";
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        /// <summary>
        /// Funcion interna para validar los datos de entrada
        /// </summary>
        private InventarioResponse ValidarEntrada(InventarioRequest inventarioRequest)
        {
            var response = new InventarioResponse
            {
                ResultadoTransaccion = "00",
                ConsecutivoRegistro = 0
            };
            var msg = "Se ha presentado una Excepcion: El siguiente campo(s)";
            if (inventarioRequest.IdPersonal == 0)
            {
                response.ResultadoTransaccion = "02";
                msg += ", idPersonal es vacio";
            }

#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
            foreach (var p in inventarioRequest.Productos)
            {
                if (p.IdProducto == 0)
                {
                    response.ResultadoTransaccion = "02";
                    msg += $", idProducto : {p.IdProducto} no puede ser 0";
                    continue;
                }

                if (!ExisteProducto(p.IdProducto))
                {
                    response.ResultadoTransaccion = "02";
                    msg += $", idProducto : {p.IdProducto} no existe en SAHI";
                }
            }
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
            response.Mensaje = msg;
            return response;
        }

        /// <summary>
        /// Funcion interna para comprobar que un producto si existe
        /// </summary>
        private bool ExisteProducto(int idproducto)
        {
            var response = false;
            DBConnection conn = new DBConnection();
            using (SqlConnection conexion = new SqlConnection(conn.getCs()))
            {
                conexion.Open(); // abro la conection                
                string strConsultar = @"select * from proproducto where  idproducto=@idproducto";
                SqlCommand cmdConsultar = new SqlCommand(strConsultar, conexion);
                cmdConsultar.Parameters.Add("@idproducto", SqlDbType.Int).Value = idproducto;
                SqlDataReader rdConsultar = cmdConsultar.ExecuteReader();
                response = rdConsultar.HasRows;
            }
            return response;
        }
    }
}
