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
    public class AgendaController : ControllerBase
    {
        private static Logger logSahico = LogManager.GetCurrentClassLogger();

        // POST: api/Agenda/ConsultarAgenda
        /// <summary>
        /// Operacion ConsultarAgenda
        /// </summary>
        /// <param name="agendaRequest">Estructura con los parametros para consumo del servicio</param>
        /// <returns>Estructura de datos para AgendaResponse</returns>
        /// <remarks>
        /// Sample request:
        ///     POST api/Agenda/ConsultarAgenda
        ///     {
        ///         "idMedico": "",
        ///         "idPaciente": 0,
        ///         "fechaIni": 0
        ///         "fechaFin": 0
        ///         "Estado": 0
        ///     }
        /// </remarks>
        [HttpPost]
        [Route("ConsultarAgenda")]
        public ActionResult ConsultarAgenda([FromBody] AgendaRequest agendaRequest)
        {

            try
            {
                DBConnection conn = new DBConnection();
                using (SqlConnection conexion = new SqlConnection(conn.getCs2()))
                {
                    conexion.Open();
                    List<AgendaResponse> lar= new List<AgendaResponse>();
                    string strConsultar = string.Empty;
                  
                        strConsultar ="SpAgendaSiisa";

                    SqlCommand cmdConsultar = new SqlCommand(strConsultar, conexion);
                    cmdConsultar.CommandType = CommandType.StoredProcedure;
                    cmdConsultar.Parameters.Add("@idMedico", SqlDbType.Int).Value =agendaRequest.idMedico;
                    cmdConsultar.Parameters.Add("@idPaciente", SqlDbType.Int).Value = agendaRequest.idPaciente;
                    cmdConsultar.Parameters.Add("@fechaInicial", SqlDbType.Date).Value = agendaRequest.FechaIni;
                    cmdConsultar.Parameters.Add("@fechaFinal", SqlDbType.Date).Value = agendaRequest.FechaFin;
                    cmdConsultar.Parameters.Add("@Estado", SqlDbType.VarChar).Value = agendaRequest.EstadoCita;
                    SqlDataReader rdConsultar = cmdConsultar.ExecuteReader();
                    if (rdConsultar.HasRows)
                    {
                        while (rdConsultar.Read())
                        {
                            AgendaResponse agendaResponse = new();
                            agendaResponse.FechaCita = rdConsultar.GetDateTime(0);
                            agendaResponse.CodigoCita = rdConsultar.GetInt64(1);

                            agendaResponse.IdMedico = rdConsultar.GetInt64(2);
                            agendaResponse.NombreMedico = rdConsultar.IsDBNull(3) ? "" : rdConsultar.GetString(3);
                            agendaResponse.ApellidoMedico = rdConsultar.IsDBNull(4) ? "" : rdConsultar.GetString(4);
                            agendaResponse.IdPaciente = rdConsultar.GetInt64(5);
                            agendaResponse.NombrePaciente = rdConsultar.IsDBNull(6) ? "" : rdConsultar.GetString(6);
                            agendaResponse.ApellidoPaciente = rdConsultar.IsDBNull(7) ? "" : rdConsultar.GetString(7);
                            agendaResponse.TipoDocumento = rdConsultar.IsDBNull(8) ? "" : rdConsultar.GetString(8);
                            agendaResponse.NumDocumento = rdConsultar.IsDBNull(9) ? "" : rdConsultar.GetString(9);
                            agendaResponse.FecQuierePaciente = rdConsultar.GetDateTime(10);
                            agendaResponse.IdServicio = rdConsultar.GetInt64(11);
                            agendaResponse.NombreServicio = rdConsultar.IsDBNull(12) ? "" : rdConsultar.GetString(12);
                            agendaResponse.NombreCortoServicio = rdConsultar.IsDBNull(13) ? "" : rdConsultar.GetString(13);
                            agendaResponse.PrimeraVezOControl = rdConsultar.GetInt32(14);
                            agendaResponse.IdAtencionTipo = rdConsultar.IsDBNull(15) ? "" : rdConsultar.GetString(15);
                            agendaResponse.IdAsegurador = rdConsultar.IsDBNull(16) ? "" : rdConsultar.GetString(16);
                            agendaResponse.NombreAsegurador = rdConsultar.IsDBNull(17) ? "" : rdConsultar.GetString(17);
                            agendaResponse.IdConsultorio = rdConsultar.IsDBNull(18) ? "" : rdConsultar.GetString(18);
                            agendaResponse.NombreConsultorio = rdConsultar.IsDBNull(19) ? "" : rdConsultar.GetString(19);
                            agendaResponse.IdConvenio = rdConsultar.IsDBNull(20) ? "" : rdConsultar.GetString(20);
                            agendaResponse.NombreConvenio = rdConsultar.IsDBNull(21) ? "" : rdConsultar.GetString(21);
                            agendaResponse.EstadoCita = rdConsultar.IsDBNull(22) ? "" : rdConsultar.GetString(22);
                            agendaResponse.EsExtra = rdConsultar.IsDBNull(23) ? "" : rdConsultar.GetString(23);
                            agendaResponse.TieneServicioControl = rdConsultar.IsDBNull(24) ? "" : rdConsultar.GetString(24);
                            agendaResponse.TieneMasControles = rdConsultar.IsDBNull(25) ? "" : rdConsultar.GetString(25);
                            lar.Add(agendaResponse);
                        }
                        logSahico.Info("Agenda encontrada. Paciente :: " + agendaRequest.idPaciente + " , medico :: " + agendaRequest.idMedico);

                        return Ok(lar);
                    }
                    else
                    {
                        AgendaResponse agendaResponse = new();
                        logSahico.Info("Agenda no encontrado en SAHI con medico :: " + agendaRequest.idMedico);
                        agendaResponse.FechaCita = DateTime.Now;
                        agendaResponse.CodigoCita = 0;
                        agendaResponse.IdMedico = 0;
                        agendaResponse.NombreMedico ="Agenda no encontrada";
                        agendaResponse.ApellidoMedico = "Agenda no encontrada";
                        agendaResponse.IdPaciente =0;
                        agendaResponse.NombrePaciente = "Agenda no encontrada";
                        agendaResponse.ApellidoPaciente = "Agenda no encontrada";
                        agendaResponse.TipoDocumento = "";
                        agendaResponse.NumDocumento = "";
                        agendaResponse.FecQuierePaciente =DateTime.Now;
                        agendaResponse.IdServicio = 0;
                        agendaResponse.NombreServicio = "Agenda no encontrada";
                        agendaResponse.NombreCortoServicio ="";
                        agendaResponse.PrimeraVezOControl = 0;
                        agendaResponse.IdAtencionTipo ="";
                        agendaResponse.IdAsegurador ="";
                        agendaResponse.NombreAsegurador ="";
                        agendaResponse.IdConsultorio ="";
                        agendaResponse.NombreConsultorio = "Agenda no encontrada";
                        agendaResponse.IdConvenio = "";
                        agendaResponse.NombreConvenio ="";
                        agendaResponse.EstadoCita = "";
                        agendaResponse.EsExtra ="";
                        agendaResponse.TieneServicioControl = "";
                        agendaResponse.TieneMasControles ="";


                        return NotFound(agendaResponse);
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
