using HUSI_SIISA.DBContext;
using HUSI_SIISA.Models.Request;
using HUSI_SIISA.Models.Response;
using HUSI_SIISA.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NLog;
using System.Data;

namespace HUSI_SIISA.Controllers
{
    /// <summary>
    /// Implememtacion del servicio Notas de Enfermeria
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class NotasEnfermeriaController : ControllerBase
    {
        private static Logger logSahico = LogManager.GetCurrentClassLogger();

        // POST: api/NotasEnfermeria/ActualizarNotaEnf
        /// <summary>
        /// Operacion para Actualizar Notas de Enfermeria
        /// </summary>
        /// <param name="notaEnfermeriaRequest">Estructura con los parametros para consumo del servicio</param>
        /// <returns>Estructura de datos para NotasEnfermeriaResponse con el valor Logico del resultado de la transaccion</returns>
        /// <remarks>
        /// Sample request:
        ///     POST api/NotasEnfermeria/ActualizarNotaEnf
        ///     {
        ///         "numDoc": "",
        ///         "tipoDoc": 0,
        ///         "servicio": 0
        ///     }
        /// </remarks>
        [HttpPost]
        [Route("actualizarNotaEnf")]
        public ActionResult ActualizarNotaEnf([FromBody] NotasEnfermeriaRequest notaEnfermeriaRequest)
        {
            NotasEnfermeriaResponse notaEnfermeriaResponse = new();

            try
            {
                DBConnection conn = new();
                using (SqlConnection conexion = new(conn.getCs()))
                {
                    conexion.Open();

                    string strConsultar = string.Empty;
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                    if (notaEnfermeriaRequest.NotaQx.Length > 0 && notaEnfermeriaRequest.NroNotadestino > 0)
                    {
                        return Ok(notaEnfermeriaResponse);
                    }
                    else
                    {
                        return BadRequest(notaEnfermeriaResponse);
                    }
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
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

        // POST: api/NotasEnfermeria/insertarNotaEnf
        /// <summary>
        /// Operacion para Insertar Notas de Enfermeria
        /// </summary>
        /// <param name="notaEnfermeriaRequest">Modelo de Datos del Contrato (NotasEnfermeria), poblado con la informacion de la Nota de Enfermeria que se desea insertar o crear</param>
        /// <returns>Estructura de datos para NotasEnfermeriaResponse con el valor Logico del resultado de la transaccion</returns>
        /// <remarks>
        /// Sample request:
        ///     POST api/NotasEnfermeria/insertarNotaEnf
        ///     {
        ///         "numDoc": "",
        ///         "tipoDoc": 0,
        ///         "servicio": 0
        ///     }
        /// </remarks>
        [HttpPost]
        [Route("insertarNotaEnf")]
        public ActionResult InsertarNotaEnf([FromBody] NotasEnfermeriaRequest notaEnfermeriaRequest)
        {
            NotasEnfermeriaResponse notaEnfermeriaResponse = new();

            try
            {
                Utilidades utilLocal = new();
                string dataCargar = string.Empty;
                string salto = @" \r\n";
                Int32 NumeroNota = 0;
                Int16 Profesional = 0;
                /*clientePacientesHusi.husiCliente pacienteW = new clientePacientesHusi.husiCliente();
                clienteInfMed.ImedicosWSClient clienteProfesionales = new clienteInfMed.ImedicosWSClient();
                clienteInfMed.RespuestasWS rptaProfesionales = clienteProfesionales.idUsuarioPersonal(notaNueva.Profesionales[0].Id_Profesional.ToString());
                if (rptaProfesionales.CodigoRpta.Equals("00"))
                {
                    Profesional = Int16.Parse(rptaProfesionales.resultado);
                }*/
                DBConnection conn = new();
                using (SqlConnection conexion = new(conn.getCs()))
                {
                    conexion.Open();

                    string strConsultar = string.Empty;
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                    if (notaEnfermeriaRequest.NotaQx.Length > 0 && notaEnfermeriaRequest.IdConsulta > 0 && notaEnfermeriaRequest.IdNota > 0)
                    {
                        dataCargar = "Numero de Nota:" + notaEnfermeriaRequest.IdConsulta + "Feha Nota:" + notaEnfermeriaRequest.FechaNota + " Numero de Consulta:" + notaEnfermeriaRequest.IdConsulta + salto;
                        //dataCargar = dataCargar + "No Documento:" + pacienteW.NumDocumento + "           Tipo:" + pacienteW.IdTipoDoc + salto;
                        // = dataCargar + "Nombre Paciente:" + pacienteW.NomCliente + " " + pacienteW.ApeCliente + "Fecha Nacimiento:" + pacienteW.FecNacimiento + salto;
                        //dataCargar = dataCargar + "Direccion:" + pacienteW.DirCasa + "      Telefonos:" + pacienteW.TelCasa + " " + pacienteW.TelCelular + salto;
                        dataCargar = dataCargar + "Numero de Atencion:" + notaEnfermeriaRequest.IdAtencion + salto;
                        dataCargar = dataCargar + " " + salto;
                        dataCargar = dataCargar + "Consecutivo Quimioterapia:" + notaEnfermeriaRequest.Consecutivo_qtx + "    Dias Aplicacion:" + notaEnfermeriaRequest.Dias_aplicacion + salto;
                        dataCargar = dataCargar + "Estado:" + notaEnfermeriaRequest.Estado + salto;
                        foreach (Profesional profesional in notaEnfermeriaRequest.Profesionales)
                        {
                            //dataCargar = dataCargar + "ID Medico:" + profesional.Id_Profesional + "  Nombre:" + profesional.Nombres_Profesional + " " + profesional.Apellidos_Profesional + " Relacion" + profesional.Relacion + salto;
                        }
                        dataCargar = dataCargar + "Nota de Enfermeria" + salto;
                        dataCargar = dataCargar + notaEnfermeriaRequest.NotaQx + salto;
                        dataCargar = dataCargar + "Observaciones:" + notaEnfermeriaRequest.Observaciones + salto;
                        NumeroNota = utilLocal.consecutivoSistabla("hceNotasAte");
                        conexion.Open();
                        SqlTransaction txTransaccion01 = conexion.BeginTransaction("TX1");
                        string actHistoria1 = "INSERT INTO hceNotasAte (IdNota, IdAtencion, FecNota, IdUbicacion, DesNota, IdUsuarioR, IdTipoNota)	VALUES (@nota,@atencion,@fechaNota,@ubicacion,@desNota,@usuario,@tipoNota)";
                        SqlCommand cmdNotasAte = new SqlCommand(actHistoria1, conexion, txTransaccion01);
                        cmdNotasAte.Parameters.Add("@nota", SqlDbType.Int).Value = NumeroNota;
                        cmdNotasAte.Parameters.Add("@atencion", SqlDbType.Int).Value = notaEnfermeriaRequest.IdAtencion;
                        cmdNotasAte.Parameters.Add("@fechaNota", SqlDbType.DateTime).Value = notaEnfermeriaRequest.FechaNota;
                        cmdNotasAte.Parameters.Add("@ubicacion", SqlDbType.Int).Value = 30;
                        cmdNotasAte.Parameters.Add("@desNota", SqlDbType.Text).Value = dataCargar;
                        cmdNotasAte.Parameters.Add("@usuario", SqlDbType.SmallInt).Value = Profesional;
                        cmdNotasAte.Parameters.Add("@tipoNota", SqlDbType.SmallInt).Value = 187;
                        logSahico.Info("********************* Valor de tipoNota:" + 187 + "  Nota:" + NumeroNota + "   Atencion:" + notaEnfermeriaRequest.IdAtencion + "****************************");
                        if (cmdNotasAte.ExecuteNonQuery() > 0)
                        {
                            logSahico.Info("Se inserta informacion en hceNotasAte O.K");
                            string actHistoria2 = "INSERT INTO hceEsquemasdeAte (IdAtencion, IdEsquema, IdEsquemadeAtencion, IdUbicacion, IdMedico, IdTraslado, FecEsquema, IndHabilitado, IndActivado, FecCerrado, EstadoApDx, idOrden,IndResCritico ) ";
                            actHistoria2 = actHistoria2 + "VALUES (@atencion,@esquema,@esquemaAte,@ubicacion, @medico,@traslado,@fechaEsquema,@indicadorHabilitado, @indicadorActivado,@fechaCerrado, @EstadoApDx, @orden,@rCritico)";
                            SqlCommand cmdEsquemasAte = new SqlCommand(actHistoria2, conexion, txTransaccion01);
                            cmdEsquemasAte.Parameters.Add("@atencion", SqlDbType.Int).Value = notaEnfermeriaRequest.IdAtencion;
                            cmdEsquemasAte.Parameters.Add("@esquema", SqlDbType.Int).Value = 809;
                            cmdEsquemasAte.Parameters.Add("@esquemaAte", SqlDbType.Int).Value = NumeroNota;
                            cmdEsquemasAte.Parameters.Add("@ubicacion", SqlDbType.SmallInt).Value = 30;
                            cmdEsquemasAte.Parameters.Add("@medico", SqlDbType.SmallInt).Value = Profesional;
                            cmdEsquemasAte.Parameters.Add("@traslado", SqlDbType.Int).Value = 1;
                            cmdEsquemasAte.Parameters.Add("@fechaEsquema", SqlDbType.DateTime).Value = notaEnfermeriaRequest.FechaNota;
                            cmdEsquemasAte.Parameters.Add("@indicadorHabilitado", SqlDbType.Bit).Value = 1;
                            cmdEsquemasAte.Parameters.Add("@indicadorActivado", SqlDbType.Bit).Value = 0;
                            cmdEsquemasAte.Parameters.Add("@fechaCerrado", SqlDbType.DateTime).Value = DateTime.Now;
                            cmdEsquemasAte.Parameters.Add("@EstadoApDx", SqlDbType.Int).Value = 4;
                            cmdEsquemasAte.Parameters.Add("@orden", SqlDbType.Int).Value = notaEnfermeriaRequest.IdNota;
                            cmdEsquemasAte.Parameters.Add("@rCritico", SqlDbType.VarChar).Value = 0;
                            if (cmdEsquemasAte.ExecuteNonQuery() > 0)
                            {
                                logSahico.Info("!!! Transaccion realizada Exitosamente !!!");
                                txTransaccion01.Commit();
                                notaEnfermeriaResponse.Resultado = true;
                                notaEnfermeriaResponse.Mensaje = "!!! Transaccion realizada Exitosamente !!!";
                                notaEnfermeriaResponse.DetalleMensaje = "Numero de Nota:" + NumeroNota;
                                return Ok(notaEnfermeriaResponse);
                            }
                            else
                            {
                                logSahico.Info("!!! No fue posible realizar la transaccion sobre la tabla:hceEsquemasdeAte !!!");
                                txTransaccion01.Rollback();
                                notaEnfermeriaResponse.Resultado = false;
                                notaEnfermeriaResponse.Mensaje = "!!! No fue posible realizar la transaccion sobre la tabla:hceEsquemasdeAte !!!";
                                notaEnfermeriaResponse.DetalleMensaje = "";
                                return StatusCode(StatusCodes.Status500InternalServerError, notaEnfermeriaResponse);
                            }
                        }
                        else
                        {
                            logSahico.Info("No fue posible realizar la transaccion sobre la tabla:hceNotasAte ");
                            txTransaccion01.Rollback();
                            notaEnfermeriaResponse.Resultado = false;
                            notaEnfermeriaResponse.Mensaje = "!!! No fue posible realizar la transaccion sobre la tabla:hceNotasAte  !!!";
                            notaEnfermeriaResponse.DetalleMensaje = "";
                            return StatusCode(StatusCodes.Status500InternalServerError, notaEnfermeriaResponse);
                        }
                    }
                    else
                    {
                        notaEnfermeriaResponse.Resultado = false;
                        notaEnfermeriaResponse.Mensaje = "!!! Error en datos de entrada, verifique  !!!";
                        notaEnfermeriaResponse.DetalleMensaje = "";
                        return BadRequest(notaEnfermeriaResponse);
                    }
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
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
