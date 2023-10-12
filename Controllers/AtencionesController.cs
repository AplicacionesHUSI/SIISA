using HUSI_SIISA.DBContext;
using HUSI_SIISA.Models.Request;
using HUSI_SIISA.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NLog;
using System.Data;
using System.Net;

namespace HUSI_SIISA.Controllers
{
    /// <summary>
    /// Implememntacion del Servicio Atenciones. permite realizar varias operaciones con la entidad Atencion de SAHI.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AtencionesController : ControllerBase
    {
        private static Logger logSahico = LogManager.GetCurrentClassLogger();

        // POST: api/Atenciones/GetConsAtenXDoc
        /// <summary>
        /// Operacion ConsAtenXDo
        /// </summary>
        /// <param name="atencionRequest">Estructura con los parametros para consumo del servicio</param>
        /// <returns>Estructura de datos para AtencionResponse</returns>
        /// <remarks>
        /// Sample request:
        ///     POST api/Atenciones/GetConsAtenXDoc
        ///     {
        ///         "numDoc": "",
        ///         "tipoDoc": 0,
        ///         "servicio": 0
        ///     }
        /// </remarks>
        [HttpPost]
        [Route("GetConsAtenXDoc")]
        public ActionResult GetConsAtenXDoc([FromBody] AtencionRequest atencionRequest)
        {
            AtencionResponse atencionResponse = new();

            try
            {
                DBConnection conn = new DBConnection();
                using (SqlConnection conexion = new SqlConnection(conn.getCs()))
                {
                    conexion.Open();

                    string strConsultar = string.Empty;
                    if (atencionRequest.Servicio == 28)
                    {
                        // 28 or 59
                        strConsultar = @"SELECT A.idCliente,A.idAtencion,A.IdAtencionTipo,B.NomAtencionTipo,D.IdAtenTipoBase,D.NomAtenTipoBase,FecIngreso,Cli.NomCliente,Cli.ApeCliente ,GT.IdTercero,GT.CodTercero,GT.NomTercero
FROM admAtencion A
INNER JOIN admCliente Cli ON A.IdCliente=Cli.IdCliente
INNER JOIN admAtencionTipo B ON A.IdAtencionTipo=B.IdAtencionTipo
INNER JOIN admAtenTipoBase D ON B.IdAtenTipoBase=d.IdAtenTipoBase
INNER JOIN admAtencionContrato AC ON AC.IdAtencion=A.IdAtencion and AC.OrdPrioridad=1
INNER JOIN conContrato CC ON CC.IdContrato=AC.IdContrato 
INNER JOIN genTercero GT ON GT.IdTercero=CC.IdTercero 
WHERE (cli.NumDocumento=@NumDocumento and cli.IdTipoDoc=@tipoDoc) AND A.IndActivado=1 AND (A.IdAtencionTipo=@idTipoAten or A.IdAtencionTipo=59  ) ORDER BY FecIngreso DESC";
                    }
                    else
                    {
                        strConsultar = @"SELECT A.idCliente,A.idAtencion,A.IdAtencionTipo,B.NomAtencionTipo,D.IdAtenTipoBase,D.NomAtenTipoBase,FecIngreso,Cli.NomCliente,Cli.ApeCliente ,GT.IdTercero,GT.CodTercero,GT.NomTercero
FROM admAtencion A
INNER JOIN admCliente Cli ON A.IdCliente=Cli.IdCliente
INNER JOIN admAtencionTipo B ON A.IdAtencionTipo=B.IdAtencionTipo
INNER JOIN admAtenTipoBase D ON B.IdAtenTipoBase=d.IdAtenTipoBase
INNER JOIN admAtencionContrato AC ON AC.IdAtencion=A.IdAtencion and AC.OrdPrioridad=1
INNER JOIN conContrato CC ON CC.IdContrato=AC.IdContrato 
INNER JOIN genTercero GT ON GT.IdTercero=CC.IdTercero 
WHERE (cli.NumDocumento=@NumDocumento and cli.IdTipoDoc=@tipoDoc) AND A.IndActivado=1 AND A.IdAtencionTipo=@idTipoAten ORDER BY FecIngreso DESC";
                    }

                    SqlCommand cmdConsultar = new SqlCommand(strConsultar, conexion);
                    cmdConsultar.Parameters.Add("@NumDocumento", SqlDbType.VarChar).Value = atencionRequest.NumDoc;
                    cmdConsultar.Parameters.Add("@tipoDoc", SqlDbType.SmallInt).Value = atencionRequest.TipoDoc;
                    cmdConsultar.Parameters.Add("@idTipoAten", SqlDbType.SmallInt).Value = atencionRequest.Servicio;
                    SqlDataReader rdConsultar = cmdConsultar.ExecuteReader();
                    if (rdConsultar.HasRows)
                    {
                        rdConsultar.Read();
                        atencionResponse.IdCliente =  rdConsultar.GetInt32(0);
                        atencionResponse.NroAtencion = rdConsultar.GetInt32(1);
                        atencionResponse.TipoAtencion = rdConsultar.GetInt16(2);
                        atencionResponse.NombreTipoAtn = rdConsultar.IsDBNull(3) ? "" : rdConsultar.GetString(3);
                        atencionResponse.TipoBaseAtencion = rdConsultar.GetInt16(4);
                        atencionResponse.NomAtnBase = rdConsultar.IsDBNull(5) ? "" : rdConsultar.GetString(5);
                        atencionResponse.FechaAtencion = rdConsultar.GetDateTime(6);
                        atencionResponse.NombrePaciente = rdConsultar.IsDBNull(7) ? "" : rdConsultar.GetString(7);
                        atencionResponse.ApellidosPaciente = rdConsultar.IsDBNull(8) ? "" : rdConsultar.GetString(8);
                        atencionResponse.IdTercero = rdConsultar.GetInt32(9);
                        atencionResponse.CodTercero =rdConsultar.IsDBNull(10)?"":rdConsultar.GetString(10);
                        atencionResponse.NomTercero = rdConsultar.IsDBNull(11) ? "" : rdConsultar.GetString(11);
                        logSahico.Info("Paciente encontrado. Doc :: " + atencionRequest.NumDoc + " , atencion :: " + atencionResponse.NroAtencion);

                        return Ok(atencionResponse);
                    }
                    else
                    {
                        logSahico.Info("Paciente no encontrado en SAHI con numDoc :: " + atencionRequest.NumDoc);
                        atencionResponse.IdCliente = 0;
                        atencionResponse.NroAtencion = 0;
                        atencionResponse.TipoAtencion = 0;
                        atencionResponse.NombreTipoAtn = "";
                        atencionResponse.TipoBaseAtencion = 0;
                        atencionResponse.NomAtnBase = "";
                        atencionResponse.FechaAtencion = DateTime.Now;
                        atencionResponse.NombrePaciente = "Paciente No Existe";
                        atencionResponse.ApellidosPaciente = "Paciente No Existe";

                        return NotFound(atencionResponse);
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
