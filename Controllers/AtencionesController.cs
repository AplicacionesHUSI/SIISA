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
                using SqlConnection conexion = new SqlConnection(new DBConnection().getCs());
                conexion.Open();

                string query = GenerarConsultaSQL(atencionRequest);
                using SqlCommand cmd = new(query, conexion);
                cmd.Parameters.Add("@NumDocumento", SqlDbType.VarChar).Value = atencionRequest.NumDoc;
                cmd.Parameters.Add("@tipoDoc", SqlDbType.SmallInt).Value = atencionRequest.TipoDoc;
                cmd.Parameters.Add("@idTipoAten", SqlDbType.SmallInt).Value = atencionRequest.Servicio;

                using SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    logSahico.Info("Paciente no encontrado en SAHI con numDoc :: " + atencionRequest.NumDoc);
                    atencionResponse = new AtencionResponse
                    {
                        IdCliente = 0,
                        NroAtencion = 0,
                        TipoAtencion = 0,
                        NombreTipoAtn = "",
                        TipoBaseAtencion = 0,
                        NomAtnBase = "",
                        FechaAtencion = DateTime.Now,
                        NombrePaciente = "Paciente No Existe",
                        ApellidosPaciente = "Paciente No Existe"
                    };
                    return NotFound(atencionResponse);
                }

                List<AtencionResponse> respuestas = new();
                while (reader.Read())
                {
                    var resp = new AtencionResponse
                    {
                        IdCliente = reader.GetInt32(0),
                        NroAtencion = reader.GetInt32(1),
                        TipoAtencion = reader.GetInt16(2),
                        NombreTipoAtn = reader.IsDBNull(3) ? "" : reader.GetString(3),
                        TipoBaseAtencion = reader.GetInt16(4),
                        NomAtnBase = reader.IsDBNull(5) ? "" : reader.GetString(5),
                        FechaAtencion = reader.GetDateTime(6),
                        NombrePaciente = reader.IsDBNull(7) ? "" : reader.GetString(7),
                        ApellidosPaciente = reader.IsDBNull(8) ? "" : reader.GetString(8),
                        IdTercero = reader.GetInt32(9),
                        CodTercero = reader.IsDBNull(10) ? "" : reader.GetString(10),
                        NomTercero = reader.IsDBNull(11) ? "" : reader.GetString(11),
                    };

                    logSahico.Info($"Paciente encontrado. Doc :: {atencionRequest.NumDoc}, atencion :: {resp.NroAtencion}");
                    respuestas.Add(resp);
                }

                return Ok(respuestas);
            }
            catch (SqlException ex)
            {
                logSahico.Error("Error en Base de Datos :: " + ex.Message);
                return StatusCode(500, new ErrorResponse
                {
                    Codigo = 500,
                    Mensaje = "Error con la operación en la base de datos, comuníquese con el administrador."
                });
            }
            catch (Exception ex)
            {
                logSahico.Error("Excepción general :: " + ex.Message);
                logSahico.Info("Detalle excepción: " + ex.StackTrace);
                return StatusCode(500, new ErrorResponse
                {
                    Codigo = 500,
                    Mensaje = "Se ha presentado una excepción general no controlada, comuníquese con el administrador."
                });
            }
        }

        private string GenerarConsultaSQL(AtencionRequest req)
        {
            string baseQuery = @"
                SELECT A.idCliente,A.idAtencion,A.IdAtencionTipo,B.NomAtencionTipo,
                       D.IdAtenTipoBase,D.NomAtenTipoBase,FecIngreso,
                       Cli.NomCliente,Cli.ApeCliente,GT.IdTercero,
                       GT.CodTercero,GT.NomTercero
                FROM admAtencion A
                INNER JOIN admCliente Cli ON A.IdCliente = Cli.IdCliente
                INNER JOIN admAtencionTipo B ON A.IdAtencionTipo = B.IdAtencionTipo
                INNER JOIN admAtenTipoBase D ON B.IdAtenTipoBase = D.IdAtenTipoBase
                INNER JOIN admAtencionContrato AC ON AC.IdAtencion = A.IdAtencion AND AC.OrdPrioridad = 1
                INNER JOIN conContrato CC ON CC.IdContrato = AC.IdContrato
                INNER JOIN genTercero GT ON GT.IdTercero = CC.IdTercero
                WHERE cli.NumDocumento = @NumDocumento AND cli.IdTipoDoc = @tipoDoc AND A.IndActivado = 1 AND A.IndHabilitado = 1";

            string filtro = "";

            if (req.IdSede == 1 && req.Servicio == 28)
                filtro = " AND (A.IdAtencionTipo = @idTipoAten OR A.IdAtencionTipo = 59)";
            else if (req.IdSede == 68 && req.Servicio == 80)
                filtro = " AND A.IdAtencionTipo = 80";
            else
                filtro = " AND A.IdAtencionTipo = @idTipoAten";

            return baseQuery + filtro + " ORDER BY FecIngreso DESC";
        }

    }
}
