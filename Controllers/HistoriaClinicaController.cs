using HUSI_SIISA.DBContext;
using HUSI_SIISA.Models.Request;
using HUSI_SIISA.Models.Response;
using HUSI_SIISA.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NLog;

namespace HUSI_SIISA.Controllers
{
    /// <summary>
    /// Implememntacion del Servicio Atenciones. permite realizar varias operaciones con la entidad Atencion de SAHI.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class HistoriaClinicaController : ControllerBase
    {
        private static Logger logSahico = LogManager.GetCurrentClassLogger();
        HistoriaClinicaResponse historiaClinicaResponse = new();

        // POST: api/HistoriaClinica/InsertarHC
        /// <summary>
        /// Servicio para Incluir informacion de la consulta de pacientes en Historia Clinica de SAHI
        /// </summary>
        /// <param name="historiaRequest">Estructura con los parametros para consumo del servicio</param>
        /// <returns>Estructura de datos para HistoriaResponse</returns>
        /// <remarks>
        /// Sample request:
        ///     POST api/HistoriaClinica/InsertarHC
        ///     {
        ///         "atencion": "",
        ///         "orden": "",
        ///         "idUbicacion": "",
        ///         "idEsquema": ""
        ///     }
        /// </remarks>
        [HttpPost]
        [Route("InsertarHC")]
        public ActionResult InsertarHC([FromBody] HistoriaClinicaRequest historiaRequest)
        {
            logSahico.Info($"Mensaje Recibido de Consulta=Historiaclinica.");
            if (string.IsNullOrEmpty(historiaRequest.IdAtencion))
            {
                try
                {
#pragma warning disable CS8604 // Posible argumento de referencia nulo
                    DateTime oDate = DateTime.ParseExact(historiaRequest.FechaConsulta, "yyyy-MM-dd HH:mm:ss", null);
#pragma warning restore CS8604 // Posible argumento de referencia nulo
                    string auxAten = Utilidades.GetAdmAtencion(historiaRequest.IdPaciente + "", oDate);
                    if (!string.IsNullOrEmpty(auxAten)) historiaRequest.IdAtencion = auxAten;
                }
                catch (Exception ex)
                {
                    logSahico.Info($"Error al convertir la fecha de la consulta : {ex.Message}");
                }
            }

            Utilidades utilLocal = new();
            string dataCargar = string.Empty;
            string salto = Environment.NewLine;
            Int32 NumeroNota = 0;
            Int16 Profesional = 0;
            //clienteInfMed.ImedicosWSClient clienteProfesionales = new clienteInfMed.ImedicosWSClient();
            //clienteInfMed.RespuestasWS rptaProfesionales = clienteProfesionales.idUsuarioPersonal(historiaInsertar.ID_Profesional);
            //if (rptaProfesionales.CodigoRpta.Equals("00"))
            //{
            //    Profesional = Int16.Parse(rptaProfesionales.resultado);
            //}

            try
            {
                DBConnection conn = new DBConnection();
                using (SqlConnection conexion = new SqlConnection(conn.getCs()))
                {
                    conexion.Open();

                    string strConsultar = string.Empty;

                    return Ok(historiaClinicaResponse);
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
