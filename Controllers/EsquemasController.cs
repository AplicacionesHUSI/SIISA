using HUSI_SIISA.DBContext;
using HUSI_SIISA.Models.Request;
using HUSI_SIISA.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NLog;
using System.Data;

namespace HUSI_SIISA.Controllers
{
    /// <summary>
    /// Implememntacion del Servicio Atenciones. permite realizar varias operaciones con la entidad Atencion de SAHI.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class EsquemasController : ControllerBase
    {
        private static Logger logSahico = LogManager.GetCurrentClassLogger();

        // POST: api/Esquemas/ConsultaEsquema
        /// <summary>
        /// Operacion Consutla de Esquema
        /// </summary>
        /// <param name="esquemaRequest">Estructura con los parametros para consumo del servicio</param>
        /// <returns>Estructura de datos para EsquemaResponse</returns>
        /// <remarks>
        /// Sample request:
        ///     POST api/Esquemas/ConsultaEsquema
        ///     {
        ///         "atencion": "",
        ///         "orden": "",
        ///         "idEsquema": ""
        ///     }
        /// </remarks>
        [HttpPost]
        [Route("ConsultaEsquema")]
        public ActionResult ConsultaEsquema([FromBody] EsquemaRequest esquemaRequest)
        {
            EsquemaResponse esquemaResponse = new();

            try
            {
                DBConnection conn = new DBConnection();
                using (SqlConnection conexion = new SqlConnection(conn.getCs()))
                {
                    conexion.Open();

                    string strConsultar = string.Empty;
                    string qryConsulta = @"SELECT IdAtencion,IdEsquema,IdEsquemaDeAtencion,IdUbicacion,IdMedico,IdTraslado,FecEsquema,
IndHabilitado,IndActivado,FecCerrado,Interpretacion,idConsulta,idUsuarioInter,EstadoApDx,FecInterpretacion,idMovimiento,idOrden,idEstudiante,idDocente,indEvaluado,indEstudianteEnEval,
fecEval,indMigrado,idInterno,IndResCritico,FechaConducta,Conducta FROM hceEsquemasdeAte
WHERE idAtencion=@atencion AND idOrden=@idConsulta AND idUbicacion=30 AND idEsquema=@idEsquema AND idMedico>0";
                    SqlCommand cmdConsulta = new SqlCommand(qryConsulta, conexion);
                    cmdConsulta.Parameters.Add("@atencion", SqlDbType.Int).Value = esquemaRequest.Atencion;
                    cmdConsulta.Parameters.Add("@idConsulta", SqlDbType.Int).Value = esquemaRequest.Orden;
                    cmdConsulta.Parameters.Add("@idEsquema", SqlDbType.Int).Value = esquemaRequest.IdEsquema;
                    SqlDataReader rdConsulta = cmdConsulta.ExecuteReader();
                    if (rdConsulta.HasRows)
                    {
                        rdConsulta.Read();
                        if (rdConsulta.IsDBNull(0)) { esquemaResponse.IdAtencion = 0; } else { esquemaResponse.IdAtencion = rdConsulta.GetInt32(0); }
                        if (rdConsulta.IsDBNull(1)) { esquemaResponse.IdEsquema = 0; } else { esquemaResponse.IdEsquema = rdConsulta.GetInt32(1); }
                        if (rdConsulta.IsDBNull(2)) { esquemaResponse.IdEsquemaDeAtencion = 0; } else { esquemaResponse.IdEsquemaDeAtencion = rdConsulta.GetInt32(2); }
                        if (rdConsulta.IsDBNull(3)) { esquemaResponse.IdUbicacion = 0; } else { esquemaResponse.IdUbicacion = rdConsulta.GetInt16(3); }
                        if (rdConsulta.IsDBNull(4)) { esquemaResponse.IdMedico = 0; } else { esquemaResponse.IdMedico = rdConsulta.GetInt16(4); }
                        if (rdConsulta.IsDBNull(5)) { esquemaResponse.IdTraslado = 0; } else { esquemaResponse.IdTraslado = rdConsulta.GetInt32(5); }
                        if (rdConsulta.IsDBNull(6)) { esquemaResponse.FecEsquema = DateTime.Now; } else { esquemaResponse.FecEsquema = rdConsulta.GetDateTime(6); }
                        if (rdConsulta.IsDBNull(7)) { esquemaResponse.IndHabilitado = false; } else { esquemaResponse.IndHabilitado = rdConsulta.GetBoolean(7); }
                        if (rdConsulta.IsDBNull(8)) { esquemaResponse.IndActivado = false; } else { esquemaResponse.IndActivado = rdConsulta.GetBoolean(8); }
                        if (rdConsulta.IsDBNull(9)) { esquemaResponse.FecCerrado = DateTime.Now; } else { esquemaResponse.FecCerrado = rdConsulta.GetDateTime(9); }
                        if (rdConsulta.IsDBNull(10)) { esquemaResponse.Interpretacion = ""; } else { esquemaResponse.Interpretacion = rdConsulta.GetString(10); }
                        if (rdConsulta.IsDBNull(11)) { esquemaResponse.IdConsulta = 0; } else { esquemaResponse.IdConsulta = rdConsulta.GetInt32(11); }
                        if (rdConsulta.IsDBNull(12)) { esquemaResponse.IdUsuarioInter = 0; } else { esquemaResponse.IdUsuarioInter = rdConsulta.GetInt32(12); }
                        if (rdConsulta.IsDBNull(13)) { esquemaResponse.EstadoApDx = 0; } else { esquemaResponse.EstadoApDx = rdConsulta.GetInt32(13); }
                        if (rdConsulta.IsDBNull(14)) { esquemaResponse.FecInterpretacion = DateTime.Now; } else { esquemaResponse.FecInterpretacion = rdConsulta.GetDateTime(14); }
                        if (rdConsulta.IsDBNull(15)) { esquemaResponse.IdMovimiento = 0; } else { esquemaResponse.IdMovimiento = rdConsulta.GetInt32(15); }
                        //if (rdConsulta.IsDBNull(16)) { esquemaResponse.IdProducto = 0; } else { esquemaResponse.IdProducto = rdConsulta.GetInt32(16); }
                        if (rdConsulta.IsDBNull(16)) { esquemaResponse.IdOrden = 0; } else { esquemaResponse.IdOrden = rdConsulta.GetInt32(16); }
                        if (rdConsulta.IsDBNull(17)) { esquemaResponse.IdEstudiante = 0; } else { esquemaResponse.IdEstudiante = rdConsulta.GetInt16(17); }
                        if (rdConsulta.IsDBNull(18)) { esquemaResponse.IdDocente = 0; } else { esquemaResponse.IdDocente = rdConsulta.GetInt16(18); }
                        if (rdConsulta.IsDBNull(19)) { esquemaResponse.IndEvaluado = ""; } else { esquemaResponse.IndEvaluado = rdConsulta.GetString(19); }
                        if (rdConsulta.IsDBNull(20)) { esquemaResponse.IndEstudianteEnEval = false; } else { esquemaResponse.IndEstudianteEnEval = rdConsulta.GetBoolean(20); }
                        if (rdConsulta.IsDBNull(21)) { esquemaResponse.FecEval = DateTime.Now; } else { esquemaResponse.FecEval = rdConsulta.GetDateTime(21); }
                        if (rdConsulta.IsDBNull(22)) { esquemaResponse.IndMigrado = false; } else { esquemaResponse.IndMigrado = rdConsulta.GetBoolean(22); }
                        if (rdConsulta.IsDBNull(23)) { esquemaResponse.IdInterno = 0; } else { esquemaResponse.IdInterno = rdConsulta.GetInt16(23); }
                        if (rdConsulta.IsDBNull(24)) { esquemaResponse.IndResCritico = ""; } else { esquemaResponse.IndResCritico = rdConsulta.GetString(24); }
                        if (rdConsulta.IsDBNull(25)) { esquemaResponse.FechaConducta = DateTime.Now; } else { esquemaResponse.FechaConducta = rdConsulta.GetDateTime(25); }
                        if (rdConsulta.IsDBNull(26)) { esquemaResponse.Conducta = ""; } else { esquemaResponse.Conducta = rdConsulta.GetString(26); }

                        return Ok(esquemaResponse);
                    }
                    else
                    {
                        logSahico.Info("Esquema no encontrado.");
                        return NotFound(esquemaResponse);
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorResponse errorResponse = new();
                logSahico.Error("Error en Base de Datos :: " + ex.Message);
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
    }
}
