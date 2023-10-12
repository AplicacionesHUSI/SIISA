using HUSI_SIISA.Models.Request;
using HUSI_SIISA.Models.Response;
using HUSI_SIISA.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NLog;
using System.Data;
using HUSI_SIISA.DBContext;
using System.Text;
using System.Xml.Serialization;

namespace HUSI_SIISA.Controllers
{
    /// <summary>
    /// Servicio de Citas de quimioterapia
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CitaQuimioterapiaController : ControllerBase
    {
        private static Logger logSahico = LogManager.GetCurrentClassLogger();

        // POST: api/CitaQuimioterapia/NotificarCita
        /// <summary>
        /// Operacion para Notificar las nuevas Citas de Quimioterapia.
        /// </summary>
        /// <param name="citaQuimioterapiaRequest">elemento complejo del tipo citasQuimioterapia</param>
        /// <returns>Elemento complejo del tipo Respuesta</returns>
        /// <remarks>
        /// Sample request:
        ///     POST api/CitaQuimioterapia/NotificarCita
        ///     {
        ///         "atencion": "",
        ///         "orden": "",
        ///         "idUbicacion": "",
        ///         "idEsquema": ""
        ///     }
        /// </remarks>
        [HttpPost]
        [Route("NotificarCita")]
        public ActionResult NotificarCita([FromBody] CitaQuimioterapiaRequest citaQuimioterapiaRequest)
        {

            StringBuilder serializado = new StringBuilder();
            XmlSerializer SerializadorJustifica = new XmlSerializer(typeof(CitaQuimioterapiaRequest));
            StringWriter swWriter = new StringWriter(serializado);
            SerializadorJustifica.Serialize(swWriter, citaQuimioterapiaRequest);
            logSahico.Info("Mensaje de CITA  Recibido:" + serializado.ToString());

            //clientePacientesHusi.husiCliente pacienteDatos = new clientePacientesHusi.husiCliente();
            //clientePacientesHusi.IhusiClienteWSClient clientePaciente = new clientePacientesHusi.IhusiClienteWSClient();
            logSahico.Info("Se recibe cita de Quimioterapia para procesar Cita:" + citaQuimioterapiaRequest.ID_Cita + " ID Paciente:" + citaQuimioterapiaRequest.ID_Paciente);
            Utilidades utilLocal = new Utilidades();
            string dataCargar = string.Empty;
            string salto = @" \r\n";
            Int32 NumeroNota = 0;
            //citaQuimioterapiaRequest.

            //clientePacientesHusi.husiCliente pacienteW = new clientePacientesHusi.husiCliente();
            CitaQuimioterapiaResponse citaQuimioterapiaResponse = new();
            try
            {
                if (citaQuimioterapiaRequest.ID_Paciente > 0 && citaQuimioterapiaRequest.ID_Cita > 0)
                {
                    //pacienteDatos = clientePaciente.Consulta_V3(citaQuimioterapiaRequest.ID_Paciente.ToString());
                    dataCargar = "Fecha Cita:" + citaQuimioterapiaRequest.Fecha_Hora_Solicitud.Date + "      Numero de Cita:" + citaQuimioterapiaRequest.ID_Cita + "        Tipo de Cita:" + citaQuimioterapiaRequest.ID_Tipo_Cita + salto;
                    dataCargar = dataCargar + "Hora Cita:" + citaQuimioterapiaRequest.Fecha_Hora_Inicio.TimeOfDay.ToString() + " Consulta:" + citaQuimioterapiaRequest.Servicio_Solicitado_Consuta + "   Consultorio:" + citaQuimioterapiaRequest.Consultorio + salto;
                    dataCargar = dataCargar + "Documento Paciente:" + "111111"/*pacienteDatos.NumDocumento*/ + "   EPS:" + "ESPprueba"/*pacienteDatos.EpsAseg*/ + salto;
                    dataCargar = dataCargar + "Nombre Paciente:" + "Prueba nombre"/*pacienteDatos.NomCliente*/ + "         Apellidos Paciente:" + "Apellido prueba"/*pacienteDatos.ApeCliente*/ + salto;
                    dataCargar = dataCargar + " Nombre de Quien Solicito Cita:" + citaQuimioterapiaRequest.Nombre_Solicitante + salto;
                    dataCargar = dataCargar + "Observaciones:" + citaQuimioterapiaRequest.Observaciones + salto;
                    NumeroNota = utilLocal.ConsecutivoSistabla("hceNotasAte"); 
                    DBConnection conn = new();
                    using (SqlConnection conexion = new(conn.getCs()))
                    {
                        conexion.Open();
                        SqlTransaction txTransaccion01 = conexion.BeginTransaction("TX1");
                        string actHistoria1 = "INSERT INTO hceNotasAte (IdNota, IdAtencion, FecNota, IdUbicacion, DesNota, IdUsuarioR, IdTipoNota)	VALUES (@nota,@atencion,@fechaNota,@ubicacion,@desNota,@usuario,@tipoNota)";
                        SqlCommand cmdNotasAte = new(actHistoria1, conexion, txTransaccion01);
                        cmdNotasAte.Parameters.Add("@nota", SqlDbType.Int).Value = NumeroNota;
                        cmdNotasAte.Parameters.Add("@atencion", SqlDbType.Int).Value = citaQuimioterapiaRequest.ID_Atencion;
                        cmdNotasAte.Parameters.Add("@fechaNota", SqlDbType.DateTime).Value = citaQuimioterapiaRequest.Fecha_Hora_Inicio;
                        cmdNotasAte.Parameters.Add("@ubicacion", SqlDbType.Int).Value = 30;
                        cmdNotasAte.Parameters.Add("@desNota", SqlDbType.Text).Value = dataCargar;
                        cmdNotasAte.Parameters.Add("@usuario", SqlDbType.SmallInt).Value = citaQuimioterapiaRequest.IdMedico;
                        cmdNotasAte.Parameters.Add("@tipoNota", SqlDbType.SmallInt).Value = 187;
                        logSahico.Info("********************* Valor de tipoNota:" + 187 + "  Nota:" + NumeroNota + "   Atencion:" + citaQuimioterapiaRequest.ID_Atencion + "****************************");
                        if (cmdNotasAte.ExecuteNonQuery() > 0)
                        {
                            logSahico.Info("Se inserta informacion en hceNotasAte O.K");
                            string actHistoria2 = "INSERT INTO hceEsquemasdeAte (IdAtencion, IdEsquema, IdEsquemadeAtencion, IdUbicacion, IdMedico, IdTraslado, FecEsquema, IndHabilitado, IndActivado, FecCerrado, EstadoApDx, idOrden,IndResCritico ) ";
                            actHistoria2 = actHistoria2 + "VALUES (@atencion,@esquema,@esquemaAte,@ubicacion, @medico,@traslado,@fechaEsquema,@indicadorHabilitado, @indicadorActivado,@fechaCerrado, @EstadoApDx, @orden,@rCritico)";
                            SqlCommand cmdEsquemasAte = new(actHistoria2, conexion, txTransaccion01);
                            cmdEsquemasAte.Parameters.Add("@atencion", SqlDbType.Int).Value = citaQuimioterapiaRequest.ID_Atencion;
                            cmdEsquemasAte.Parameters.Add("@esquema", SqlDbType.Int).Value = 808;
                            cmdEsquemasAte.Parameters.Add("@esquemaAte", SqlDbType.Int).Value = NumeroNota;
                            cmdEsquemasAte.Parameters.Add("@ubicacion", SqlDbType.SmallInt).Value = 30;
                            cmdEsquemasAte.Parameters.Add("@medico", SqlDbType.SmallInt).Value = citaQuimioterapiaRequest.IdMedico;
                            cmdEsquemasAte.Parameters.Add("@traslado", SqlDbType.Int).Value = 1;
                            cmdEsquemasAte.Parameters.Add("@fechaEsquema", SqlDbType.DateTime).Value = citaQuimioterapiaRequest.Fecha_Hora_Inicio;
                            cmdEsquemasAte.Parameters.Add("@indicadorHabilitado", SqlDbType.Bit).Value = 1;
                            cmdEsquemasAte.Parameters.Add("@indicadorActivado", SqlDbType.Bit).Value = 0;
                            cmdEsquemasAte.Parameters.Add("@fechaCerrado", SqlDbType.DateTime).Value = DateTime.Now;
                            cmdEsquemasAte.Parameters.Add("@EstadoApDx", SqlDbType.Int).Value = 4;
                            cmdEsquemasAte.Parameters.Add("@orden", SqlDbType.Int).Value = citaQuimioterapiaRequest.ID_Cita;
                            cmdEsquemasAte.Parameters.Add("@rCritico", SqlDbType.VarChar).Value = 0;
                            if (cmdEsquemasAte.ExecuteNonQuery() > 0)
                            {
                                logSahico.Info("!!! Transaccion realizada Exitosamente !!!");
                                txTransaccion01.Commit();
                                citaQuimioterapiaResponse.Resultado = true;
                                citaQuimioterapiaResponse.Mensaje = "!!! Transaccion realizada Exitosamente !!!";
                                citaQuimioterapiaResponse.DetalleMensaje = "Numero de Nota:" + NumeroNota;
                                return Ok(citaQuimioterapiaResponse);
                            }
                            else
                            {
                                logSahico.Info("!!! No fue posible realizar la transaccion sobre la tabla:hceEsquemasdeAte !!!");
                                txTransaccion01.Rollback();
                                citaQuimioterapiaResponse.Resultado = false;
                                citaQuimioterapiaResponse.Mensaje = "!!! No fue posible realizar la transaccion sobre la tabla:hceEsquemasdeAte !!!";
                                citaQuimioterapiaResponse.DetalleMensaje = "";
                                return BadRequest(citaQuimioterapiaResponse);
                            }
                        }
                        else
                        {
                            logSahico.Info("No fue posible realizar la transaccion sobre la tabla:hceNotasAte ");
                            txTransaccion01.Rollback();
                            citaQuimioterapiaResponse.Resultado = false;
                            citaQuimioterapiaResponse.Mensaje = "!!! No fue posible realizar la transaccion sobre la tabla:hceNotasAte  !!!";
                            citaQuimioterapiaResponse.DetalleMensaje = "";
                            return BadRequest(citaQuimioterapiaResponse);
                        }
                        //***********************************************************************************                
                    }

                }
                else
                {
                    logSahico.Info("!!!   Transaccion Fallida   !!!");
                    logSahico.Info("La informacion Suministrada no es Correcta. Revisar los datos de la cita y del Paciente");
                    citaQuimioterapiaResponse.Resultado = true;
                    citaQuimioterapiaResponse.Mensaje = "!!!   Transaccion Fallida   !!!";
                    citaQuimioterapiaResponse.DetalleMensaje = "La informacion Suministrada no es Correcta. Revisar los datos de la cita y del Paciente";
                    return BadRequest(citaQuimioterapiaResponse);
                }
            }
            catch (SqlException sqlEx1)
            {
                ErrorResponse errorResponse = new();
                logSahico.Error("Error en Base de Datos :: " + sqlEx1.Message);
                logSahico.Info("Se ha presentado una Excepcion:" + sqlEx1.InnerException);
                logSahico.Info("Se ha presentado una Excepcion:" + sqlEx1.StackTrace);
                errorResponse.Codigo = 500;
                errorResponse.Mensaje = "Se ha presentado una Excepcion de SQL.";
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
            catch (Exception ex1)
            {
                ErrorResponse errorResponse = new();
                logSahico.Error("Error:: " + ex1.Message);
                logSahico.Info("Se ha presentado una Excepcion:" + ex1.InnerException);
                logSahico.Info("Se ha presentado una Excepcion:" + ex1.StackTrace);
                errorResponse.Codigo = 500;
                errorResponse.Mensaje = "Se ha presentado una Excepcion General";
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
    }
}
