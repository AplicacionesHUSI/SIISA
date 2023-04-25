using HUSI_SIISA.DBContext;
using HUSI_SIISA.Models.Request;
using HUSI_SIISA.Models.Response;
using HUSI_SIISA.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NLog;
using static HUSI_SIISA.Utilities.Utilidades;
using System.Data;

namespace HUSI_SIISA.Controllers
{
    /// <summary>
    /// Implementacion del Servicio OrdenProcedimientos
    /// Operaciones:    actualizarProced
    /// ordenarProcedimientos
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrdenarProcedimientosController : ControllerBase
    {
        private static Logger logSahico = LogManager.GetCurrentClassLogger();

        // POST: api/OrdenarProcedimientos/ActualizarProcedimientos
        /// <summary>
        /// Operacion del Servicio OrdenProcedimientos. Para realizar la Actualizacion de Procedimientos
        /// </summary>
        /// <param name="ordenarProcedimientosRequest">Modelo de contrato (Procedimientos) Poblado con la informacion de los Procedimientos</param>
        /// <returns>Valor Logico con el resultado de exito de la transaccion</returns>
        /// <remarks>
        /// Sample request:
        ///     POST api/OrdenarProcedimientos/actualizarProcedimientos
        ///     {
        ///         
        ///     }
        /// </remarks>
        [HttpPost]
        [Route("actualizarProcedimientos")]
        public ActionResult ActualizarProcedimientos([FromBody] OrdenarProcedimientosRequest ordenarProcedimientosRequest)
        {
            OrdenarProcedimientosResponse ordenarProcedimientosResponse = new();

            try
            {
                DBConnection conn = new();
                using (SqlConnection conexion = new(conn.getCs()))
                {
                    //conexion.Open();

                    string strConsultar = string.Empty;
                    ordenarProcedimientosResponse.Resultado = false;
                    ordenarProcedimientosResponse.Mensaje = "En Construción";
                    ordenarProcedimientosResponse.DetalleMensaje = "En Construccion";
                    return Ok(ordenarProcedimientosResponse);
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

        // POST: api/OrdenarProcedimientos/OrdenarProcedimientos
        /// <summary>
        /// Operacion del Servicio OrdenProcedimientos, para Insertar procedimientos
        /// </summary>
        /// <param name="ordenarProcedimientosRequest">Modelo de Procedimientos poblado con la informacion de los Procedimientos</param>
        /// <returns>Valor Logico con el resultado de exito de la transaccion</returns>
        /// <remarks>
        /// Sample request:
        ///     POST api/OrdenarProcedimientos/ordenarProcedimientos
        ///     {
        ///         
        ///     }
        /// </remarks>
        [HttpPost]
        [Route("ordenarProcedimientos")]
        public ActionResult OrdenarProcedimientos([FromBody] OrdenarProcedimientosRequest ordenarProcedimientosRequest)
        {
            OrdenarProcedimientosResponse ordenarProcedimientosResponse = new();
            logSahico.Info("Mensaje Recibido de Orden de Procedimientos:" + ordenarProcedimientosRequest.ToString());

            try
            {
                //get adm atencion
                if (ordenarProcedimientosRequest.Atencion == 0)
                {
                    string auxAten = Utilidades.GetAdmAtencion(ordenarProcedimientosRequest.IdPaciente + "", ordenarProcedimientosRequest.Fecha);
                    if (!string.IsNullOrEmpty(auxAten)) ordenarProcedimientosRequest.Atencion = int.Parse(auxAten);
                }
                //entry validation 
                ordenarProcedimientosResponse = ValidarEntrada(ordenarProcedimientosRequest);
                if (!ordenarProcedimientosResponse.Resultado)
                {
                    logSahico.Info($" No se puede procesar el mensaje porque falta informacion::	{ordenarProcedimientosResponse.Mensaje}");
                    return BadRequest(ordenarProcedimientosResponse);
                }
                //end validation

                if (ordenarProcedimientosRequest.IdConsulta > 0)
                {
                    Utilidades utilLocal = new Utilidades();
                    string dataCargar = string.Empty;
                    string salto = Environment.NewLine;
                    Int32 NumeroNota = 0;
                    Int16 medicoOrdena = 0;
                    //clientePacientesHusi.husiCliente pacienteW = new clientePacientesHusi.husiCliente();
                    //clienteInfMed.ImedicosWSClient clienteMedicos = new clienteInfMed.ImedicosWSClient();
                    //clienteInfMed.RespuestasWS rptaMedicos = clienteMedicos.idUsuarioPersonal(procedimientos.IdMedico.ToString());
                    //if (rptaMedicos.CodigoRpta.Equals("00"))
                    //{
                    //    medicoOrdena = Int16.Parse(rptaMedicos.resultado);
                    //}
                    DBConnection conn = new();
                    using (SqlConnection conexion = new(conn.getCs()))
                    {
                        conexion.Open();
                        string strConsultar = string.Empty;
                        //clientePacientesHusi.IhusiClienteWSClient paciente = new clientePacientesHusi.IhusiClienteWSClient();
                        //pacienteW = paciente.Consulta_V3(procedimientos.IdPaciente.ToString());
                        dataCargar = "_____________________PROCEDIMIENTOS_______________________" + salto;
                        dataCargar = dataCargar + "Fecha:" + ordenarProcedimientosRequest.Fecha;
                        dataCargar = dataCargar + " Numero de Atencion:" + ordenarProcedimientosRequest.Atencion + salto + "Numero de Consulta:" + ordenarProcedimientosRequest.IdConsulta;
                        //dataCargar = dataCargar + "  No Documento:" + pacienteW.NumDocumento + salto + "Fecha de Nacimiento:" + pacienteW.FecNacimiento.ToString("dd/MM/yyyy") + salto;
                        //dataCargar = dataCargar + "Nombre Paciente:" + pacienteW.NomCliente + " " + pacienteW.ApeCliente + "                          Telefono:" + pacienteW.TelCelular + salto;
                        dataCargar = dataCargar + " " + salto;
                        dataCargar = dataCargar + "Procedimientos" + salto + salto;

#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                        foreach (Itemproce procedimiento in ordenarProcedimientosRequest.Items)
                        {

                            string strConsulta = "SELECT NomProducto FROM proProducto WHERE IdProducto=@idProductoSAHICO AND CodLegal=@cups";
                            SqlCommand cmdConsulta = new(strConsulta, conexion);
                            cmdConsulta.Parameters.Add("@idProductoSAHICO", SqlDbType.Int).Value = procedimiento.IdProcedimiento;
                            cmdConsulta.Parameters.Add("@cups", SqlDbType.VarChar).Value = procedimiento.Cups;
                            SqlDataReader rdConsulta = cmdConsulta.ExecuteReader();
                            if (rdConsulta.HasRows)
                            {
                                rdConsulta.Read();
                                dataCargar = dataCargar + "CUPS: " + procedimiento.Cups + " Procedimiento: " + rdConsulta.GetString(0) + salto + "  Cantidad: " + procedimiento.Cantidad + " No. MiPres " + procedimiento.Num_MiPres + salto;
                                dataCargar = dataCargar + "Interpretacion: " + procedimiento.Interpretacion + salto;
                                dataCargar = dataCargar + "Observaciones: " + procedimiento.Observaciones + salto;
                                dataCargar = dataCargar + "Resolucion 3047" + salto;
                                dataCargar = dataCargar + "Origen: " + procedimiento.Res3047Item.Origen_Atencion + "  Prioridad: " + procedimiento.Res3047Item.Prioridad +
                                             salto + "Guia Manejo: " + procedimiento.Res3047Item.Guia_Manejo + salto +
                                             "Ubicacion Paciente: " + procedimiento.Res3047Item.Ubiacion_Paciente + salto;
                                dataCargar = dataCargar + "Servicios Solicitados: " + procedimiento.Res3047Item.Tipo_Serv_Solicitados + salto;
                                dataCargar = dataCargar + "Usuario Responsable:" + procedimiento.Res3047Item.Usr_Responsable + salto;
                                dataCargar = dataCargar + "Observaciones: " + procedimiento.Res3047Item.Observaciones + salto + salto;
                                rdConsulta.Close();
                            }
                            else
                            {
                                ordenarProcedimientosResponse.Resultado = false;
                                ordenarProcedimientosResponse.Mensaje = "El procedimiento No Existe en SAHI";
                                ordenarProcedimientosResponse.DetalleMensaje = "El idProductoSAHICO:" + procedimiento.IdProcedimiento + "   CUPS:" + procedimiento.Cups;
                                return BadRequest(ordenarProcedimientosResponse);
                            }

                        }
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.

                        dataCargar = dataCargar + "___________________FIN PROCEDIMIENTOS_____________________" + salto;

                        ValidacionNotas objNotas = new();
                        objNotas = utilLocal.ValidaConsulta(ordenarProcedimientosRequest.IdConsulta, "3", ordenarProcedimientosRequest.Atencion);
                        NumeroNota = objNotas.IdNota;
                        if (NumeroNota > 0 && objNotas.IdNotaProc == 0)
                        {
                            //nueva nota para el procedimiento
                            SqlTransaction txTransaccion01 = conexion.BeginTransaction("TX1");
                            NumeroNota = utilLocal.ConsecutivoSistabla("hceNotasAte");
                            string actHistoria1 = "INSERT INTO hceNotasAte (IdNota, IdAtencion, FecNota, IdUbicacion, DesNota, IdUsuarioR, IdTipoNota)	VALUES (@nota,@atencion,@fechaNota,@ubicacion,@desNota,@usuario,@tipoNota)";
                            SqlCommand cmdNotasAte = new SqlCommand(actHistoria1, conexion, txTransaccion01);
                            cmdNotasAte.Parameters.Add("@nota", SqlDbType.Int).Value = NumeroNota;
                            cmdNotasAte.Parameters.Add("@atencion", SqlDbType.Int).Value = ordenarProcedimientosRequest.Atencion;
                            cmdNotasAte.Parameters.Add("@fechaNota", SqlDbType.DateTime).Value = ordenarProcedimientosRequest.Fecha;
                            cmdNotasAte.Parameters.Add("@ubicacion", SqlDbType.Int).Value = 30;
                            cmdNotasAte.Parameters.Add("@desNota", SqlDbType.Text).Value = dataCargar;
                            cmdNotasAte.Parameters.Add("@usuario", SqlDbType.SmallInt).Value = medicoOrdena;
                            cmdNotasAte.Parameters.Add("@tipoNota", SqlDbType.SmallInt).Value = 812;
                            logSahico.Info("********************* Valor de tipoNota:" + 812 + "  Nota:" + NumeroNota + "   Atencion:" + ordenarProcedimientosRequest.Atencion + "****************************");
                            if (cmdNotasAte.ExecuteNonQuery() > 0)
                            {
                                logSahico.Info("Se inserta informacion en hceNotasAte O.K Medico que Ordena:" + medicoOrdena);
                                string actHistoria2 = "INSERT INTO hceEsquemasdeAte (IdAtencion, IdEsquema, IdEsquemadeAtencion, IdUbicacion, IdMedico, IdTraslado, FecEsquema, IndHabilitado, IndActivado, FecCerrado, EstadoApDx, idOrden,IndResCritico ) ";
                                actHistoria2 += "VALUES (@atencion,@esquema,@esquemaAte,@ubicacion, @medico,@traslado,@fechaEsquema,@indicadorHabilitado, @indicadorActivado,@fechaCerrado, @EstadoApDx, @orden,@rCritico)";
                                SqlCommand cmdEsquemasAte = new SqlCommand(actHistoria2, conexion, txTransaccion01);
                                cmdEsquemasAte.Parameters.Add("@atencion", SqlDbType.Int).Value = ordenarProcedimientosRequest.Atencion;
                                cmdEsquemasAte.Parameters.Add("@esquema", SqlDbType.Int).Value = 812;
                                cmdEsquemasAte.Parameters.Add("@esquemaAte", SqlDbType.Int).Value = NumeroNota;
                                cmdEsquemasAte.Parameters.Add("@ubicacion", SqlDbType.SmallInt).Value = 30;
                                cmdEsquemasAte.Parameters.Add("@medico", SqlDbType.SmallInt).Value = medicoOrdena;
                                cmdEsquemasAte.Parameters.Add("@traslado", SqlDbType.Int).Value = 1;
                                cmdEsquemasAte.Parameters.Add("@fechaEsquema", SqlDbType.DateTime).Value = ordenarProcedimientosRequest.Fecha;
                                cmdEsquemasAte.Parameters.Add("@indicadorHabilitado", SqlDbType.Bit).Value = 1;
                                cmdEsquemasAte.Parameters.Add("@indicadorActivado", SqlDbType.Bit).Value = 0;
                                cmdEsquemasAte.Parameters.Add("@fechaCerrado", SqlDbType.DateTime).Value = DateTime.Now;
                                cmdEsquemasAte.Parameters.Add("@EstadoApDx", SqlDbType.Int).Value = 4;
                                cmdEsquemasAte.Parameters.Add("@orden", SqlDbType.Int).Value = ordenarProcedimientosRequest.IdConsulta;
                                cmdEsquemasAte.Parameters.Add("@rCritico", SqlDbType.VarChar).Value = 0;
                                if (cmdEsquemasAte.ExecuteNonQuery() > 0)
                                {
                                    if (utilLocal.ActualizarSahicoRel(ordenarProcedimientosRequest.IdConsulta, objNotas.IdNota, NumeroNota, ordenarProcedimientosRequest.Atencion, ordenarProcedimientosRequest.Fecha, 3))
                                    {
                                        logSahico.Info("!!! Transaccion realizada Exitosamente !!!");
                                        txTransaccion01.Commit();
                                        ordenarProcedimientosResponse.IdNotaSAHI = NumeroNota;
                                        ordenarProcedimientosResponse.Resultado = true;
                                        ordenarProcedimientosResponse.Mensaje = "!!! Transaccion realizada Exitosamente !!!";
                                        ordenarProcedimientosResponse.DetalleMensaje = "";
                                        return Ok(ordenarProcedimientosResponse);
                                    }
                                    else
                                    {
                                        logSahico.Info("!!! No fue posible realizar la transaccion sobre la tabla:hceIntegraSahicoRel !!!");
                                        txTransaccion01.Rollback();
                                        ordenarProcedimientosResponse.Resultado = false;
                                        ordenarProcedimientosResponse.Mensaje = "!!! No fue posible realizar la transaccion sobre la tabla:hceIntegraSahicoRel !!!";
                                        ordenarProcedimientosResponse.DetalleMensaje = "";
                                        return StatusCode(StatusCodes.Status500InternalServerError, ordenarProcedimientosResponse);
                                    }
                                }
                                else
                                {
                                    logSahico.Info("!!! No fue posible realizar la transaccion sobre la tabla:hceEsquemasdeAte !!!");
                                    txTransaccion01.Rollback();
                                    ordenarProcedimientosResponse.Resultado = false;
                                    ordenarProcedimientosResponse.Mensaje = "!!! No fue posible realizar la transaccion sobre la tabla:hceEsquemasdeAte !!!";
                                    ordenarProcedimientosResponse.DetalleMensaje = "";
                                    return StatusCode(StatusCodes.Status500InternalServerError, ordenarProcedimientosResponse);
                                }
                            }
                            else
                            {
                                logSahico.Info("No fue posible realizar la transaccion sobre la tabla:hceNotasAte ");
                                txTransaccion01.Rollback();
                                ordenarProcedimientosResponse.Resultado = false;
                                ordenarProcedimientosResponse.Mensaje = "!!! No fue posible realizar la transaccion sobre la tabla:hceNotasAte  !!!";
                                ordenarProcedimientosResponse.DetalleMensaje = "";
                                return StatusCode(StatusCodes.Status500InternalServerError, ordenarProcedimientosResponse);
                            }
                        }
                        else if (NumeroNota > 0 && objNotas.IdNotaProc > 0) // Actualizar Nota en SAHI
                        {
                            NumeroNota = objNotas.IdNotaProc;
                            SqlTransaction txTransaccion01 = conexion.BeginTransaction("TX1");
                            string actHistoria1 = "UPDATE hceNotasAte SET  DesNota=CONVERT(VARCHAR(MAX),DesNota)+@desNota WHERE IdNota=@nota AND IdAtencion=@atencion";
                            SqlCommand cmdNotasAte = new SqlCommand(actHistoria1, conexion, txTransaccion01);
                            cmdNotasAte.Parameters.Add("@nota", SqlDbType.Int).Value = NumeroNota;
                            cmdNotasAte.Parameters.Add("@atencion", SqlDbType.Int).Value = ordenarProcedimientosRequest.Atencion;
                            cmdNotasAte.Parameters.Add("@fechaNota", SqlDbType.DateTime).Value = ordenarProcedimientosRequest.Fecha;
                            cmdNotasAte.Parameters.Add("@ubicacion", SqlDbType.Int).Value = 30;
                            cmdNotasAte.Parameters.Add("@desNota", SqlDbType.VarChar).Value = dataCargar;
                            cmdNotasAte.Parameters.Add("@usuario", SqlDbType.SmallInt).Value = medicoOrdena;
                            cmdNotasAte.Parameters.Add("@tipoNota", SqlDbType.SmallInt).Value = 812;
                            logSahico.Info("********************* Valor de tipoNota:" + 812 + "  Nota:" + NumeroNota + "   Atencion:" + ordenarProcedimientosRequest.Atencion + "****************************");
                            if (cmdNotasAte.ExecuteNonQuery() > 0)
                            {
                                logSahico.Info("Se Actualiza informacion en hceNotasAte O.K");
                                ordenarProcedimientosResponse.Resultado = true;
                                ordenarProcedimientosResponse.IdNotaSAHI = NumeroNota;
                                ordenarProcedimientosResponse.Mensaje = "!!! Transaccion realizada Exitosamente !!!";
                                ordenarProcedimientosResponse.DetalleMensaje = "";
                                txTransaccion01.Commit();
                                return Ok(ordenarProcedimientosResponse);
                            }
                            else
                            {
                                logSahico.Info("No fue posible realizar la transaccion de Actualizacion sobre la tabla:hceNotasAte ");
                                txTransaccion01.Rollback();
                                ordenarProcedimientosResponse.Resultado = false;
                                ordenarProcedimientosResponse.Mensaje = "!!! No fue posible realizar la transaccion sobre la tabla:hceNotasAte  !!!";
                                ordenarProcedimientosResponse.DetalleMensaje = "";
                                return StatusCode(StatusCodes.Status500InternalServerError, ordenarProcedimientosResponse);
                            }
                        }
                        else
                        {
                            logSahico.Info("No fue posible realizar la transaccion de Actualizacion sobre la tabla:hceNotasAte ");
                            ordenarProcedimientosResponse.Resultado = false;
                            ordenarProcedimientosResponse.Mensaje = "!!! No Existe una Nota de consulta en la tabla:hceNotasAte  !!!";
                            ordenarProcedimientosResponse.DetalleMensaje = "";
                            return StatusCode(StatusCodes.Status500InternalServerError, ordenarProcedimientosResponse);
                        }
                    }
                }
                else
                {
                    ordenarProcedimientosResponse.Resultado = false;
                    ordenarProcedimientosResponse.Mensaje = "No se proporciono el Numero de la consulta";
                    ordenarProcedimientosResponse.DetalleMensaje = "No se procesa el mensaje";
                    logSahico.Info("No se procesa el Procedimiento, porque no se dispone de numero de consulta.");
                    return BadRequest(ordenarProcedimientosResponse);
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

        /// <summary>
        /// Valida Entrada
        /// </summary>
        private static OrdenarProcedimientosResponse ValidarEntrada(OrdenarProcedimientosRequest procedimientos)
        {
            var response = new OrdenarProcedimientosResponse
            {
                Resultado = true,
                IdNotaSAHI = 0
            };
            var msg = "Se ha presentado una Excepcion: El siguiente campo es vacio";
            if (procedimientos.IdPaciente == 0)
            {
                response.Resultado = false;
                msg += ", idPaciente";
            }
            else response.IdPaciente = procedimientos.IdPaciente;
            if (procedimientos.IdConsulta == 0)
            {
                response.Resultado = false;
                msg += ", idconsulta";
            }
            else response.ConsultaSahico = procedimientos.IdConsulta;
            if (procedimientos.Atencion == 0)
            {
                response.Resultado = false;
                msg += ", idAtencion";
            }
            else response.Atencion = procedimientos.Atencion;
            response.Mensaje = msg;
            return response;
        }
    }
}
