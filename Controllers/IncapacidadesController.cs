using System.Data;
using System.ServiceModel;
using System.Text;
using HUSI_SIISA.DBContext;
using System.Xml.Serialization;
using HUSI_SIISA.Models.Request;
using HUSI_SIISA.Models.Response;
using HUSI_SIISA.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NLog;

namespace HUSI_SIISA.Controllers
{

    /// <summary>
    /// Implementacion del Servicio
    ///     Operaciones:    actualizarIncapacidad
    ///                             InsertarIncapacidad
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class IncapacidadesController : ControllerBase
    {
        private static Logger logSahico = LogManager.GetCurrentClassLogger();
        IncapacidadesResponse incapacidadesResponse = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actualizarIncapacidad"></param>
        /// <returns>Valor Logico con el Resultado de la operacion</returns>

        [HttpPost]
        [Route("ActualizarIncapacidad")]
        public IncapacidadesResponse ActualizarIncapacidad(IncapacidadesRequest actualizarIncapacidad)
        {
            if (actualizarIncapacidad.IdAtencion > 0 && actualizarIncapacidad.IdConsulta > 0 && actualizarIncapacidad.IdPaciente > 0)
            {
                return incapacidadesResponse;
            }
            else
            {
                return incapacidadesResponse;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nuevaIncapacidad"></param>
        /// <returns>Valor Logico con el Resultado de la operacion</returns>

        [HttpPost]
        [Route("InsertarIncapacidad")]
        public ActionResult InsertarIncapacidad([FromBody] IncapacidadesRequest nuevaIncapacidad)
        {
            StringBuilder serializado = new StringBuilder();
            XmlSerializer SerializadorIncapacidades = new XmlSerializer(typeof(IncapacidadesRequest));
            StringWriter swWriter = new StringWriter(serializado);
            SerializadorIncapacidades.Serialize(swWriter, nuevaIncapacidad);
            logSahico.Info("Mensaje Recibido de Incapacidades:" + serializado.ToString());

            Utilidades utilLocal = new Utilidades();
            string dataCargar = string.Empty;
            string salto = Environment.NewLine;
            Int32 NumeroNota = 0;

            //clientePacientesHusi.husiCliente pacienteW = new clientePacientesHusi.husiCliente();
            try
            {

                logSahico.Info("Nro Atencion Recibido;" + nuevaIncapacidad.IdAtencion);
                logSahico.Info("Numero de consulta:" + nuevaIncapacidad.IdConsulta);
                if (nuevaIncapacidad.IdAtencion > 0 && nuevaIncapacidad.IdConsulta > 0)
                {
                    //clientePacientesHusi.IhusiClienteWSClient paciente = new clientePacientesHusi.IhusiClienteWSClient();
                    //pacienteW = paciente.Consulta_V3(nuevaIncapacidad.IdPaciente.ToString());
                    dataCargar = "_______________ INCAPACIDADES _______________" + salto;
                    dataCargar = dataCargar + "Fecha Consulta:" + nuevaIncapacidad.Fecha + salto;
                    dataCargar = dataCargar + "Documento:" + "1111111"/*pacienteW.NumDocumento*/ + "           Numero de Atencion" + nuevaIncapacidad.IdAtencion + salto;
                    dataCargar = dataCargar + "Nombre Paciente:" + "nombrePrueba"/*pacienteW.NomCliente*/ + " " + "apellido prueba"/*pacienteW.ApeCliente*/ + "          Fecha Nacimiento:" + "21/21/2121"/*pacienteW.FecNacimiento*/ + salto;
                    dataCargar = dataCargar + "Numero Incapacidad:" + nuevaIncapacidad.IdIncapacidad + "         Numero de Consulta:" + nuevaIncapacidad.IdConsulta + salto;
                    dataCargar = dataCargar + "Fecha de Inicio Incapacidad:" + nuevaIncapacidad.Fecha_Inicio + "         Fecha Final Incapacidad:" + nuevaIncapacidad.Fecha_Final + salto;
                    dataCargar = dataCargar + "Numero total de Dias:" + nuevaIncapacidad.Nro_Dias + "                   Servicio:" + nuevaIncapacidad.Servicio + salto;
                    dataCargar = dataCargar + "Tipo Incapacidad:" + nuevaIncapacidad.TipoIncapacidad + "                    Entidad:" + nuevaIncapacidad.Entidad + salto;
                    dataCargar = dataCargar + "Diagnosticos" + salto;
                    dataCargar = dataCargar + "_______________________________________________________" + salto;
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                    foreach (Diagnostico diagnosticoX in nuevaIncapacidad.Diagnosticos)
                    {
                        foreach (ItemDiagnostico itemX in diagnosticoX.Items_Dx)
                        {
                            dataCargar = dataCargar + "Codigo Diagnostico:" + itemX.CodigoDx + "       Nombre Dx:" + itemX.NombreDx + "            Tipo:" + itemX.Tipo + "           Confirmado:" + itemX.Confirmado + salto;
                            dataCargar = dataCargar + "TNM";
                            dataCargar = dataCargar + "Tumor:" + itemX.TnmDx.Tumor + "     Nodulo:" + itemX.TnmDx.Nodulo + "      Metastasis:" + itemX.TnmDx.Metastasis + "       Estado:" + itemX.TnmDx.Estado + salto;
                            dataCargar = dataCargar + "Informacion Resolucion:0247" + salto;
                            dataCargar = dataCargar + "Fecha Informe Histopatologico Valido:" + itemX.InfResol_0247.Fec_Inf_Histo_Val + "            Fecha Recoleccion Muestra:" + itemX.InfResol_0247.Fec_Rec_Muestra + salto;
                            dataCargar = dataCargar + "Grado de Diferenciacion:" + itemX.InfResol_0247.Grado_Dif + "             Valor Histologia:" + itemX.InfResol_0247.Histologia + salto;
                            dataCargar = dataCargar + "Objetivo del Tratamiento Inicial:" + itemX.InfResol_0247.Obj_Trata_Ini + salto;
                            dataCargar = dataCargar + "Objetivo Intervencion Medica:" + itemX.InfResol_0247.Obj_Interv_Medica + salto;
                        }
                    }
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.

                    dataCargar = dataCargar + "_______________FIN INCAPACIDADES _______________" + salto;

                    //********************* INSERTAR TABLAS
                    NumeroNota = utilLocal.ConsecutivoSistabla("hceNotasAte"); 
                    DBConnection conn = new();
                    using (SqlConnection conexion = new(conn.getCs()))
                    {
                        conexion.Open();
                        SqlTransaction txTransaccion01 = conexion.BeginTransaction("TX1");
                        string actHistoria1 = "INSERT INTO hceNotasAte (IdNota, IdAtencion, FecNota, IdUbicacion, DesNota, IdUsuarioR, IdTipoNota)	VALUES (@nota,@atencion,@fechaNota,@ubicacion,@desNota,@usuario,@tipoNota)";
                        SqlCommand cmdNotasAte = new(actHistoria1, conexion, txTransaccion01);
                        cmdNotasAte.Parameters.Add("@nota", SqlDbType.Int).Value = NumeroNota;
                        cmdNotasAte.Parameters.Add("@atencion", SqlDbType.Int).Value = nuevaIncapacidad.IdAtencion;
                        cmdNotasAte.Parameters.Add("@fechaNota", SqlDbType.DateTime).Value = nuevaIncapacidad.Fecha;
                        cmdNotasAte.Parameters.Add("@ubicacion", SqlDbType.Int).Value = 30;
                        cmdNotasAte.Parameters.Add("@desNota", SqlDbType.Text).Value = dataCargar;
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                        cmdNotasAte.Parameters.Add("@usuario", SqlDbType.SmallInt).Value = nuevaIncapacidad.Profesionales[0].Id_profesional;
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
                        cmdNotasAte.Parameters.Add("@tipoNota", SqlDbType.SmallInt).Value = 187;
                        logSahico.Info("********************* Valor de tipoNota:" + 187 + "  Nota:" + NumeroNota + "   Atencion:" + nuevaIncapacidad.IdAtencion + "****************************");
                        if (cmdNotasAte.ExecuteNonQuery() > 0)
                        {
                            logSahico.Info("Se inserta informacion en hceNotasAte O.K");
                            string actHistoria2 = "INSERT INTO hceEsquemasdeAte (IdAtencion, IdEsquema, IdEsquemadeAtencion, IdUbicacion, IdMedico, IdTraslado, FecEsquema, IndHabilitado, IndActivado, FecCerrado, EstadoApDx, idOrden,IndResCritico ) ";
                            actHistoria2 = actHistoria2 + "VALUES (@atencion,@esquema,@esquemaAte,@ubicacion, @medico,@traslado,@fechaEsquema,@indicadorHabilitado, @indicadorActivado,@fechaCerrado, @EstadoApDx, @orden,@rCritico)";
                            SqlCommand cmdEsquemasAte = new(actHistoria2, conexion, txTransaccion01);
                            cmdEsquemasAte.Parameters.Add("@atencion", SqlDbType.Int).Value = nuevaIncapacidad.IdAtencion;
                            cmdEsquemasAte.Parameters.Add("@esquema", SqlDbType.Int).Value = 807;
                            cmdEsquemasAte.Parameters.Add("@esquemaAte", SqlDbType.Int).Value = NumeroNota;
                            cmdEsquemasAte.Parameters.Add("@ubicacion", SqlDbType.SmallInt).Value = 30;
                            cmdEsquemasAte.Parameters.Add("@medico", SqlDbType.SmallInt).Value = Int16.Parse(nuevaIncapacidad.Profesionales[0].Id_profesional.ToString());
                            cmdEsquemasAte.Parameters.Add("@traslado", SqlDbType.Int).Value = 1;
                            cmdEsquemasAte.Parameters.Add("@fechaEsquema", SqlDbType.DateTime).Value = nuevaIncapacidad.Fecha;
                            cmdEsquemasAte.Parameters.Add("@indicadorHabilitado", SqlDbType.Bit).Value = 1;
                            cmdEsquemasAte.Parameters.Add("@indicadorActivado", SqlDbType.Bit).Value = 0;
                            cmdEsquemasAte.Parameters.Add("@fechaCerrado", SqlDbType.DateTime).Value = DateTime.Now;
                            cmdEsquemasAte.Parameters.Add("@EstadoApDx", SqlDbType.Int).Value = 4;
                            cmdEsquemasAte.Parameters.Add("@orden", SqlDbType.Int).Value = nuevaIncapacidad.IdConsulta;
                            cmdEsquemasAte.Parameters.Add("@rCritico", SqlDbType.VarChar).Value = 0;
                            if (cmdEsquemasAte.ExecuteNonQuery() > 0)
                            {
                                logSahico.Info("!!! Transaccion realizada Exitosamente !!!");
                                txTransaccion01.Commit();
                                incapacidadesResponse.Resultado = true;
                                incapacidadesResponse.Mensaje = "!!! Transaccion realizada Exitosamente !!!";
                                incapacidadesResponse.DetalleMensaje = "Numero de Nota:" + NumeroNota;
                                return Ok(incapacidadesResponse);
                            }
                            else
                            {
                                logSahico.Info("!!! No fue posible realizar la transaccion sobre la tabla:hceEsquemasdeAte !!!");
                                txTransaccion01.Rollback();
                                incapacidadesResponse.Resultado = false;
                                incapacidadesResponse.Mensaje = "!!! No fue posible realizar la transaccion sobre la tabla:hceEsquemasdeAte !!!";
                                incapacidadesResponse.DetalleMensaje = "";
                                return BadRequest(incapacidadesResponse);
                            }
                        }
                        else
                        {
                            logSahico.Info("No fue posible realizar la transaccion sobre la tabla:hceNotasAte ");
                            txTransaccion01.Rollback();
                            incapacidadesResponse.Resultado = false;
                            incapacidadesResponse.Mensaje = "!!! No fue posible realizar la transaccion sobre la tabla:hceNotasAte  !!!";
                            incapacidadesResponse.DetalleMensaje = "";
                            return BadRequest(incapacidadesResponse);
                        }
                    }
                }
                else
                {
                    incapacidadesResponse.Resultado = false;
                    incapacidadesResponse.Mensaje = "No es posible realizar la Operacion. La informacion suministrada se Encuentra Incompleta";
                    incapacidadesResponse.DetalleMensaje = "El numero de Atencion o Numero de consulta no se han enviado.";
                    logSahico.Warn("El numero de Atencion o Numero de consulta no se han enviado.");
                    return BadRequest(incapacidadesResponse);
                }
            }
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
            catch (CommunicationException cmEx1)
            {
                ErrorResponse errorResponse = new();
                logSahico.Error("Error de Comunicacion :: " + cmEx1.Message);
                logSahico.Info("Se ha presentado una Excepcion:" + cmEx1.InnerException);
                logSahico.Info("Se ha presentado una Excepcion:" + cmEx1.StackTrace);
                errorResponse.Codigo = 500;
                errorResponse.Mensaje = "Se ha presentado una Excepcion de Comunicaciones.";
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
            catch (Exception ex3)
            {

                ErrorResponse errorResponse = new();
                logSahico.Error("Error:: " + ex3.Message);
                logSahico.Info("Se ha presentado una Excepcion:" + ex3.InnerException);
                logSahico.Info("Se ha presentado una Excepcion:" + ex3.StackTrace);
                errorResponse.Codigo = 500;
                errorResponse.Mensaje = "Se ha presentado una Excepcion General No controlada.";
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
    }
}
