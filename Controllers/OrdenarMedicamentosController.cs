using HUSI_SIISA.DBContext;
using HUSI_SIISA.Models.Request;
using HUSI_SIISA.Models.Response;
using HUSI_SIISA.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NLog;
using System.Data;
using System.Text;
using System.Xml.Serialization;
using static HUSI_SIISA.Utilities.Utilidades;

namespace HUSI_SIISA.Controllers
{
    /// <summary>
	/// Servicio ordenar Medicamentos
	/// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrdenarMedicamentosController : ControllerBase
    {
        private static Logger logSahico = LogManager.GetCurrentClassLogger();

        // POST: api/OrdenarMedicamentos/actualizarmedicamentos
        /// <summary>
        /// Servicio para Actualizar medicamentos de una consulta
        /// </summary>
        /// <param name="ordenarMedicamentosRequest">Estructura con los parametros para consumo del servicio</param>
        /// <returns>Estructura de datos para ordenarMedicamentosResponse con el valor Logico del resultado de la transaccion</returns>
        /// <remarks>
        /// Sample request:
        ///     POST api/Ordenarmedicamentos/actualizarmedicamentos
        ///     {
        ///         "numDoc": "",
        ///         "tipoDoc": 0,
        ///         "servicio": 0
        ///     }
        /// </remarks>
        [HttpPost]
        [Route("actualizarMedicamentos")]
        public ActionResult ActualizarMedicamentos([FromBody] OrdenarMedicamentosRequest ordenarMedicamentosRequest)
        {
            OrdenarMedicamentosResponse ordenarMedicamentosResponse = new();

            try
            {
                DBConnection conn = new();
                using (SqlConnection conexion = new(conn.getCs()))
                {
                    //conexion.Open();

                    string strConsultar = string.Empty;
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                    ordenarMedicamentosResponse.Resultado = false;
                    ordenarMedicamentosResponse.Mensaje = "No esta implementada la Actualizacion de Ordenes de medicamentos";
                    ordenarMedicamentosResponse.DetalleMensaje = "No esta implementada la Actualizacion de Ordenes de medicamentos";
                    return Ok(ordenarMedicamentosResponse);
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

        // POST: api/OrdenarMedicamentos/ordenarMedicamentos
        /// <summary>
        /// Servicio para Actualizar medicamentos de una consulta
        /// </summary>
        /// <param name="ordenarMedicamentosRequest">Estructura con los parametros para consumo del servicio</param>
        /// <returns>Estructura de datos para ordenarMedicamentosResponse con el valor Logico del resultado de la transaccion</returns>
        /// <remarks>
        /// Sample request:
        ///     POST api/Ordenarmedicamentos/ordenarMedicamentos
        ///     {
        ///         "numDoc": "",
        ///         "tipoDoc": 0,
        ///         "servicio": 0
        ///     }
        /// </remarks>
        [HttpPost]
        [Route("ordenarMedicamentos")]
        public ActionResult OrdenarMedicamentos([FromBody] OrdenarMedicamentosRequest ordenarMedicamentosRequest)
        {
            OrdenarMedicamentosResponse ordenarMedicamentosResponse = new();

            try
            {
                logSahico.Info("Mensaje Recibido de Orden de Medicamentos:" + ordenarMedicamentosRequest.ToString());
                //get adm atencion
                if (ordenarMedicamentosRequest.Atencion == 0)
                {
                    string auxAten = Utilidades.GetAdmAtencion(ordenarMedicamentosRequest.IdPaciente + "", ordenarMedicamentosRequest.Fecha);
                    if (!string.IsNullOrEmpty(auxAten)) ordenarMedicamentosRequest.Atencion = int.Parse(auxAten);
                }
                //entry validation 
                ordenarMedicamentosResponse = ValidarEntrada(ordenarMedicamentosRequest);
                if (!ordenarMedicamentosResponse.Resultado)
                {
                    logSahico.Info($" No se puede procesar el mensaje porque falta informacion::	{ordenarMedicamentosResponse.Mensaje}");
                    return BadRequest(ordenarMedicamentosResponse);
                }
                //end validation
                Utilidades utilLocal = new Utilidades();
                string dataCargar = string.Empty;
                var saltoLinea = Environment.NewLine;
                Int32 NumeroNota = 0;
                //var pacienteW = new clientePacientesHusi.husiCliente();
                //var clienteMedicos = new clienteInfMed.ImedicosWSClient();

                //var rptaMedicos = clienteMedicos.idUsuarioPersonal(ordenarMedicamentosRequest.IdMedico.ToString());
                Int16 medicoOrdena = 0;
                //if (rptaMedicos.CodigoRpta.Equals("00"))
                //{
                //    medicoOrdena = Int16.Parse(rptaMedicos.resultado);
                //}

                DBConnection conn = new();
                using (SqlConnection conexion = new(conn.getCs()))
                {
                    conexion.Open();

                    string strConsultar = string.Empty;
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                    if (ordenarMedicamentosRequest.IdConsulta > 0 && ordenarMedicamentosRequest.IdPaciente > 0 && (ordenarMedicamentosRequest.Items_Medicamentos.Count > 0 || ordenarMedicamentosRequest.Items_med_NPos.Count > 0))
                    {
                        //clientePacientesHusi.IhusiClienteWSClient paciente = new clientePacientesHusi.IhusiClienteWSClient();
                        //pacienteW = paciente.Consulta_V3(ordenarMedicamentosRequest.IdPaciente.ToString());
                        dataCargar = "_________________ORDEN MEDICAMENTOS_______________________" + saltoLinea;

                        dataCargar = dataCargar + "Fecha:" + ordenarMedicamentosRequest.Fecha;
                        dataCargar = dataCargar + " Numero de Atencion:" + ordenarMedicamentosRequest.Atencion + saltoLinea + "Numero Consulta:" + ordenarMedicamentosRequest.IdConsulta;
                        //dataCargar = dataCargar + "  No Documento:" + pacienteW.NumDocumento + saltoLinea + "Fecha de Nacimiento " + pacienteW.FecNacimiento + saltoLinea;
                        //dataCargar = dataCargar + "Paciente:" + pacienteW.NomCliente + " " + pacienteW.ApeCliente + "         Tel:" + pacienteW.TelCasa + saltoLinea;
                        //dataCargar = dataCargar + " " + saltoLinea;

                        dataCargar = ordenarMedicamentosRequest.Items_Medicamentos.Count > 0 ? dataCargar + "Medicamentos POS" + saltoLinea + saltoLinea : dataCargar;
                        foreach (ItemMedicamentoPOS medicamento in ordenarMedicamentosRequest.Items_Medicamentos)
                        {
                            dataCargar = dataCargar + "ID Producto:" + medicamento.Id_Producto + "  Codigo:" + medicamento.CodigoPresentacion + " " + medicamento.NombrePresentacion + "  Cant:" + medicamento.Cantidad + "  Duracion:" + medicamento.Duracion + saltoLinea;
                            dataCargar = dataCargar + "Nombre Medicamento:" + medicamento.Nombre_Medicamento + "   Formulacion:" + medicamento.Formulacion + saltoLinea;
                            dataCargar = dataCargar + "Respuesta Clinica Esperada:" + medicamento.Resp_Clinica_Esp + saltoLinea;
                            dataCargar = dataCargar + "Observaciones:" + medicamento.Observaciones;
                            dataCargar = dataCargar + " " + saltoLinea + saltoLinea;
                        }
                        dataCargar = dataCargar + "" + saltoLinea;
                        dataCargar = ordenarMedicamentosRequest.Items_Medicamentos.Count > 0 ? dataCargar + "Medicamentos No POS" + saltoLinea + saltoLinea : dataCargar;
                        foreach (ItemMedicamentoNoPOS medNpos in ordenarMedicamentosRequest.Items_med_NPos)
                        {
                            dataCargar = dataCargar + "ID Producto:" + medNpos.Id_Producto + " CUM:" + medNpos.Codigo_Cum +
                                        saltoLinea + "No. MiPres: " + medNpos.Num_MiPres +
                                         " " + saltoLinea + medNpos.Nombre_Medicamento;
                            dataCargar += saltoLinea + "Cant Ciclo:" + medNpos.Cantidad_Ciclo + " Cant Tratamiento:" + medNpos.Cantidad_Tratamiento + saltoLinea;
                            dataCargar = dataCargar + " Duracion Tratamiento:" + medNpos.Duracion_Tratamiento + saltoLinea + "Formulacion: " + medNpos.Formulacion.Campo3 + saltoLinea + "Registro Sanitario: " + medNpos.Registro_Sanitario + " Tipo Medicamento: " + medNpos.Tipo_Medicamento + saltoLinea;
                            dataCargar = dataCargar + "Aplicacion:" + medNpos.Aplicacion + "  Vencimiento Justificacion: " + medNpos.Vencimiento_Justificacion + saltoLinea + saltoLinea;
                        }
                        dataCargar = dataCargar + "_________________FIN ORDEN MEDICAMENTOS__________________" + saltoLinea;

                        SqlTransaction txTransaccion01 = conexion.BeginTransaction("TX1");
                        ValidacionNotas objNotas = new();
                        objNotas = utilLocal.ValidaConsulta(ordenarMedicamentosRequest.IdConsulta, "2", ordenarMedicamentosRequest.Atencion);
                        NumeroNota = objNotas.IdNota;
                        if (NumeroNota > 0 && objNotas.IdNotaMed == 0)
                        {

                            NumeroNota = utilLocal.ConsecutivoSistabla("hceNotasAte");
                            string actHistoria1 = "INSERT INTO hceNotasAte (IdNota, IdAtencion, FecNota, IdUbicacion, DesNota, IdUsuarioR, IdTipoNota)	VALUES (@nota,@atencion,@fechaNota,@ubicacion,@desNota,@usuario,@tipoNota)";
                            SqlCommand cmdNotasAte = new(actHistoria1, conexion, txTransaccion01);
                            cmdNotasAte.Parameters.Add("@nota", SqlDbType.Int).Value = NumeroNota;
                            cmdNotasAte.Parameters.Add("@atencion", SqlDbType.Int).Value = ordenarMedicamentosRequest.Atencion;
                            cmdNotasAte.Parameters.Add("@fechaNota", SqlDbType.DateTime).Value = ordenarMedicamentosRequest.Fecha;
                            cmdNotasAte.Parameters.Add("@ubicacion", SqlDbType.Int).Value = 30;
                            cmdNotasAte.Parameters.Add("@desNota", SqlDbType.Text).Value = dataCargar;
                            cmdNotasAte.Parameters.Add("@usuario", SqlDbType.SmallInt).Value = medicoOrdena;
                            cmdNotasAte.Parameters.Add("@tipoNota", SqlDbType.SmallInt).Value = 811;
                            logSahico.Info("********************* Valor de tipoNota:" + 811 + "  Nota:" + NumeroNota + "   Atencion:" + ordenarMedicamentosRequest.Atencion + "****************************");
                            if (cmdNotasAte.ExecuteNonQuery() > 0)
                            {
                                logSahico.Info("Se inserta informacion en hceNotasAte O.K");
                                string actHistoria2 = "INSERT INTO hceEsquemasdeAte (IdAtencion, IdEsquema, IdEsquemadeAtencion, IdUbicacion, IdMedico, IdTraslado, FecEsquema, IndHabilitado, IndActivado, FecCerrado, EstadoApDx, idOrden,IndResCritico ) ";
                                actHistoria2 = actHistoria2 + "VALUES (@atencion,@esquema,@esquemaAte,@ubicacion, @medico,@traslado,@fechaEsquema,@indicadorHabilitado, @indicadorActivado,@fechaCerrado, @EstadoApDx, @orden,@rCritico)";
                                SqlCommand cmdEsquemasAte = new SqlCommand(actHistoria2, conexion, txTransaccion01);
                                cmdEsquemasAte.Parameters.Add("@atencion", SqlDbType.Int).Value = ordenarMedicamentosRequest.Atencion;
                                cmdEsquemasAte.Parameters.Add("@esquema", SqlDbType.Int).Value = 811;
                                cmdEsquemasAte.Parameters.Add("@esquemaAte", SqlDbType.Int).Value = NumeroNota;
                                cmdEsquemasAte.Parameters.Add("@ubicacion", SqlDbType.SmallInt).Value = 30;
                                cmdEsquemasAte.Parameters.Add("@medico", SqlDbType.SmallInt).Value = medicoOrdena;
                                cmdEsquemasAte.Parameters.Add("@traslado", SqlDbType.Int).Value = 1;
                                cmdEsquemasAte.Parameters.Add("@fechaEsquema", SqlDbType.DateTime).Value = ordenarMedicamentosRequest.Fecha;
                                cmdEsquemasAte.Parameters.Add("@indicadorHabilitado", SqlDbType.Bit).Value = 1;
                                cmdEsquemasAte.Parameters.Add("@indicadorActivado", SqlDbType.Bit).Value = 0;
                                cmdEsquemasAte.Parameters.Add("@fechaCerrado", SqlDbType.DateTime).Value = DateTime.Now;
                                cmdEsquemasAte.Parameters.Add("@EstadoApDx", SqlDbType.Int).Value = 4;
                                cmdEsquemasAte.Parameters.Add("@orden", SqlDbType.Int).Value = ordenarMedicamentosRequest.IdConsulta;
                                cmdEsquemasAte.Parameters.Add("@rCritico", SqlDbType.VarChar).Value = 0;
                                if (cmdEsquemasAte.ExecuteNonQuery() > 0)
                                {
                                    if (utilLocal.ActualizarSahicoRel(ordenarMedicamentosRequest.IdConsulta, objNotas.IdNota, NumeroNota, ordenarMedicamentosRequest.Atencion, ordenarMedicamentosRequest.Fecha, 2))
                                    {
                                        logSahico.Info("!!! Transaccion realizada Exitosamente !!!");
                                        txTransaccion01.Commit();
                                        ordenarMedicamentosResponse.IdNotaSAHI = NumeroNota;
                                        ordenarMedicamentosResponse.Resultado = true;
                                        ordenarMedicamentosResponse.Mensaje = "!!! Transaccion realizada Exitosamente !!!";
                                        ordenarMedicamentosResponse.DetalleMensaje = "";
                                        return Ok(ordenarMedicamentosResponse);
                                    }
                                    else
                                    {
                                        logSahico.Info("!!! No fue posible realizar la transaccion sobre la tabla:hceIntegraSahicoRel !!!");
                                        txTransaccion01.Rollback();
                                        ordenarMedicamentosResponse.Resultado = false;
                                        ordenarMedicamentosResponse.Mensaje = "!!! No fue posible realizar la transaccion sobre la tabla:hceIntegraSahicoRel !!!";
                                        ordenarMedicamentosResponse.DetalleMensaje = "";
                                        return StatusCode(StatusCodes.Status500InternalServerError, ordenarMedicamentosResponse);
                                    }
                                }
                                else
                                {
                                    logSahico.Info("!!! No fue posible realizar la transaccion sobre la tabla:hceEsquemasdeAte !!!");
                                    txTransaccion01.Rollback();
                                    ordenarMedicamentosResponse.Resultado = false;
                                    ordenarMedicamentosResponse.Mensaje = "!!! No fue posible realizar la transaccion sobre la tabla:hceEsquemasdeAte !!!";
                                    ordenarMedicamentosResponse.DetalleMensaje = "";
                                    return StatusCode(StatusCodes.Status500InternalServerError, ordenarMedicamentosResponse);
                                }
                            }
                            else
                            {
                                logSahico.Info("No fue posible realizar la transaccion sobre la tabla:hceNotasAte ");
                                txTransaccion01.Rollback();
                                ordenarMedicamentosResponse.Resultado = false;
                                ordenarMedicamentosResponse.Mensaje = "!!! No fue posible realizar la transaccion sobre la tabla:hceNotasAte  !!!";
                                ordenarMedicamentosResponse.DetalleMensaje = "";
                                return StatusCode(StatusCodes.Status500InternalServerError, ordenarMedicamentosResponse);
                            }
                        }
                        else if (NumeroNota > 0 && objNotas.IdNotaMed > 0) // Actualizar Nota en SAHI
                        {
                            NumeroNota = objNotas.IdNotaMed;
                            //SqlTransaction txTransaccion01 = Conex00.BeginTransaction("TX1");
                            string actHistoria1 = "UPDATE hceNotasAte SET  DesNota=CONVERT(VARCHAR(MAX),DesNota)+@desNota WHERE IdNota=@nota AND IdAtencion=@atencion";
                            SqlCommand cmdNotasAte = new SqlCommand(actHistoria1, conexion, txTransaccion01);
                            cmdNotasAte.Parameters.Add("@nota", SqlDbType.Int).Value = NumeroNota;
                            cmdNotasAte.Parameters.Add("@atencion", SqlDbType.Int).Value = ordenarMedicamentosRequest.Atencion;
                            cmdNotasAte.Parameters.Add("@fechaNota", SqlDbType.DateTime).Value = ordenarMedicamentosRequest.Fecha;
                            cmdNotasAte.Parameters.Add("@ubicacion", SqlDbType.Int).Value = 30;
                            cmdNotasAte.Parameters.Add("@desNota", SqlDbType.VarChar).Value = dataCargar;
                            cmdNotasAte.Parameters.Add("@usuario", SqlDbType.SmallInt).Value = 0;  // Toca implementar el medico o profesional de SAHICO
                            cmdNotasAte.Parameters.Add("@tipoNota", SqlDbType.SmallInt).Value = 811;
                            logSahico.Info("********************* Valor de tipoNota:" + 811 + "  Nota:" + NumeroNota + "   Atencion:" + ordenarMedicamentosRequest.Atencion + "****************************");
                            if (cmdNotasAte.ExecuteNonQuery() > 0)
                            {
                                if (utilLocal.ActualizarSahicoRel(ordenarMedicamentosRequest.IdConsulta, objNotas.IdNota, NumeroNota, ordenarMedicamentosRequest.Atencion, ordenarMedicamentosRequest.Fecha, 2))
                                {

                                    logSahico.Info("Se Actualiza informacion en hceNotasAte O.K");
                                    txTransaccion01.Commit();
                                    ordenarMedicamentosResponse.IdNotaSAHI = NumeroNota;
                                    ordenarMedicamentosResponse.Resultado = true;
                                    ordenarMedicamentosResponse.Mensaje = "!!! Transaccion realizada Exitosamente !!!";
                                    ordenarMedicamentosResponse.DetalleMensaje = "";
                                    return StatusCode(StatusCodes.Status500InternalServerError, ordenarMedicamentosResponse);
                                }
                                else
                                {
                                    logSahico.Info("No se actualizo la tabla de Relacion con SAHICO - Si Se Actualiza informacion en hceNotasAte O.K");
                                    txTransaccion01.Commit();
                                    ordenarMedicamentosResponse.IdNotaSAHI = NumeroNota;
                                    ordenarMedicamentosResponse.Resultado = true;
                                    ordenarMedicamentosResponse.Mensaje = "!!! Transaccion realizada Exitosamente !!!";
                                    ordenarMedicamentosResponse.DetalleMensaje = "";
                                    return Ok(ordenarMedicamentosResponse);
                                }
                            }
                            else
                            {
                                logSahico.Info("No fue posible realizar la transaccion de Actualizacion sobre la tabla:hceNotasAte ");
                                txTransaccion01.Rollback();
                                ordenarMedicamentosResponse.Resultado = false;
                                ordenarMedicamentosResponse.Mensaje = "!!! No fue posible realizar la transaccion sobre la tabla:hceNotasAte  !!!";
                                ordenarMedicamentosResponse.DetalleMensaje = "";
                                return StatusCode(StatusCodes.Status500InternalServerError, ordenarMedicamentosResponse);
                            }
                        }
                        else
                        {
                            logSahico.Info("No fue posible realizar la transaccion de Actualizacion sobre la tabla:hceNotasAte ");
                            logSahico.Warn("No existe la notra de la Consulta(Hisotria). Primero debe haberse creado la Nota de Historia desde SAHICO");
                            ordenarMedicamentosResponse.Resultado = false;
                            ordenarMedicamentosResponse.Mensaje = "!!! No Existe una Nota de Consulta la tabla:hceNotasAte  !!!";
                            ordenarMedicamentosResponse.DetalleMensaje = "";
                            return StatusCode(StatusCodes.Status500InternalServerError, ordenarMedicamentosResponse);
                        }
                    }
                    else
                    {
                        return BadRequest(ordenarMedicamentosResponse);
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

        // POST: api/OrdenarMedicamentos/OrdenarMedicamentosHosp
        /// <summary>
        /// Servicio para Actualizar medicamentos de una consulta
        /// </summary>
        /// <param name="ordenarMedicamentosRequest">Estructura con los parametros para consumo del servicio</param>
        /// <returns>Estructura de datos para ordenarMedicamentosResponse con el valor Logico del resultado de la transaccion</returns>
        /// <remarks>
        /// Sample request:
        ///     POST api/Ordenarmedicamentos/ordenarMedicamentosHosp
        ///     {
        ///         "numDoc": "",
        ///         "tipoDoc": 0,
        ///         "servicio": 0
        ///     }
        /// </remarks>
        [HttpPost]
        [Route("ordenarMedicamentosHosp")]
        public ActionResult OrdenarMedicamentosHosp([FromBody] OrdenarMedicamentosRequest ordenarMedicamentosRequest)
        {
            OrdenarMedicamentosResponse ordenarMedicamentosResponse = new();

            try
            {
                DBConnection conn = new();
                using (SqlConnection conexion = new(conn.getCs()))
                {
                    //conexion.Open();

                    string strConsultar = string.Empty;
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                    ordenarMedicamentosResponse.Resultado = false;
                    ordenarMedicamentosResponse.Mensaje = "En Construción";
                    ordenarMedicamentosResponse.DetalleMensaje = "En Construcción";
                    return Ok(ordenarMedicamentosResponse);
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

        /// <summary>
        /// Valida Entrada
        /// </summary>
        private static OrdenarMedicamentosResponse ValidarEntrada(OrdenarMedicamentosRequest ordenarMedicamentosRequest)
        {
            var response = new OrdenarMedicamentosResponse
            {
                Resultado = true,
                IdNotaSAHI = 0
            };
            var msg = "Se ha presentado una Excepcion: El siguiente campo es vacio";
            if (ordenarMedicamentosRequest.IdPaciente == 0)
            {
                response.Resultado = false;
                msg += ", idPaciente";
            }
            else response.IdPaciente = ordenarMedicamentosRequest.IdPaciente;
            if (ordenarMedicamentosRequest.IdConsulta == 0)
            {
                response.Resultado = false;
                msg += ", idconsulta";
            }
            else response.ConsultaSahico = ordenarMedicamentosRequest.IdConsulta;
            if (ordenarMedicamentosRequest.Atencion == 0)
            {
                response.Resultado = false;
                msg += ", idAtencion";
            }
            else response.Atencion = ordenarMedicamentosRequest.Atencion;
            response.Mensaje = msg;
            return response;
        }
    }
}
