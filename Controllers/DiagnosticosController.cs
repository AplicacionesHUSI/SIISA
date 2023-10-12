using HUSI_SIISA.Models.Request;
using HUSI_SIISA.Utilities;
using System.Data;
using System.Text;
using HUSI_SIISA.DBContext;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NLog;
using static HUSI_SIISA.Utilities.Utilidades;
using HUSI_SIISA.Models.Response;

namespace HUSI_SIISA.Controllers
{
    /// <summary>
    /// Modelo de contrato para Datos Diagnosticos
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class DiagnosticosController : ControllerBase
    {
        private static Logger logSahico = LogManager.GetCurrentClassLogger();

        // POST: api/Diagnosticos/ActualizarDx
        /// <summary>
        /// Servicio para Actualizar Diagnosticos
        /// </summary>
        /// <param name="dxActualizar">Diagnostico, en estructura XML</param>
        /// <returns>Complejo - Respuesta</returns>
        /// <remarks>
        /// Sample request:
        ///     POST api/Diagnosticos/ActualizarDx
        ///     {
        ///         "atencion": "",
        ///         "orden": "",
        ///         "idUbicacion": "",
        ///         "idEsquema": ""
        ///     }
        /// </remarks>

        [HttpPost]
        [Route("ActualizarDx")]
        public ActionResult ActualizarDx([FromBody] DiagnosticosRequest dxActualizar)
        {
            DiagnosticosResponse diagnosticosResponse = new();
            try
            {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                if (dxActualizar.Items_Dx.Count > 0 && dxActualizar.DxDestino > 0)
                {
                    diagnosticosResponse.Resultado = true;
                    diagnosticosResponse.Mensaje = "Operacion realizada Exitosamente";
                    diagnosticosResponse.DetalleMensaje = "Sin Detalle";
                    return Ok(diagnosticosResponse);
                }
                else
                {
                    diagnosticosResponse.Resultado = false;
                    diagnosticosResponse.DetalleMensaje = "No es posible realizar la operacion";
                    diagnosticosResponse.DetalleMensaje = "Por favor Revisar los parametros solicitados por el servicio.";
                    return BadRequest(diagnosticosResponse);
                }
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
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
                logSahico.Error("Error en Base de Datos :: " + ex1.Message);
                logSahico.Info("Se ha presentado una Excepcion:" + ex1.InnerException);
                logSahico.Info("Se ha presentado una Excepcion:" + ex1.StackTrace);
                errorResponse.Codigo = 500;
                errorResponse.Mensaje = "Se ha presentado una Excepcion de tipo General.";
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);

            }
        }

        // POST: api/Diagnosticos/ActualizarDx
        /// <summary>
        /// Servicio para Insertar o Ingresar Diagnosticos en SAHI.
        /// </summary>
        /// <param name="dxOrigen">Diagnostico estructurado en XML</param>
        /// <returns>Complejo Respuesta</returns>
        [HttpPost]
        [Route("InsertarDx")]
        public ActionResult InsertarDx([FromBody] DiagnosticosRequest dxOrigen)
        {
            StringBuilder serializado = new StringBuilder();
            XmlSerializer SerializadorDiagnosticos = new XmlSerializer(typeof(DiagnosticosRequest));
            StringWriter swWriter = new StringWriter(serializado);
            SerializadorDiagnosticos.Serialize(swWriter, dxOrigen);
            logSahico.Info("Mensaje Recibido de diagnosticos:" + serializado.ToString());

            //get adm atencion
            if (dxOrigen.IdAtencion == 0)
            {
                string auxAten = Utilidades.GetAdmAtencion(dxOrigen.IdPaciente + "", dxOrigen.Fecha);
                if (!string.IsNullOrEmpty(auxAten)) dxOrigen.IdAtencion = int.Parse(auxAten);
            }
            //entry validation 
            DiagnosticosResponse diagnosticosResponse = ValidarEntrada(dxOrigen);
            if (!diagnosticosResponse.Resultado)
            {
                logSahico.Info($" No se puede procesar el mensaje porque falta informacion::	{diagnosticosResponse.Mensaje}");
                return BadRequest(diagnosticosResponse);
            }
            //end validation
            Utilidades utilLocal = new Utilidades();
            string dataCargar = string.Empty;
            var saltoLinea = Environment.NewLine;
            Int32 NumeroNota = 0;
            //clientePacientesHusi.husiCliente pacienteW = new clientePacientesHusi.husiCliente();
            //clienteInfMed.ImedicosWSClient medico = new clienteInfMed.ImedicosWSClient();
            ValidacionNotas objNotas = utilLocal.ValidaConsulta(dxOrigen.IdConsulta, "1", dxOrigen.IdAtencion);

            NumeroNota = objNotas.IdNota;
            if (NumeroNota > 0)
            {
                try
                {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                    if (dxOrigen.Items_Dx.Count > 0)
                    {
                        //clientePacientesHusi.IhusiClienteWSClient paciente = new clientePacientesHusi.IhusiClienteWSClient();
                        //pacienteW = paciente.Consulta_V3(dxOrigen.IdPaciente.ToString());
                        dataCargar = dataCargar + "_____________________DIAGNOSTICOS_________________________" + saltoLinea;
                        dataCargar = dataCargar + "Fecha:" + dxOrigen.Fecha + saltoLinea;
                        foreach (ItemDiagnostico itemDx in dxOrigen.Items_Dx)
                        {
                            dataCargar = dataCargar + "Codigo Dx:" + itemDx.CodigoDx + "     Nombre:" + itemDx.NombreDx + saltoLinea;
                            dataCargar = dataCargar + "Tipo:" + itemDx.Tipo + "      Confirmado:" + itemDx.Confirmado + saltoLinea;
                            dataCargar = dataCargar + "T.N.M" + saltoLinea;
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                            if (itemDx.TnmDx.Tumor.Length > 0)
                                dataCargar = dataCargar + "Tumor:" + itemDx.TnmDx.Tumor.Split(',')[0] + " " + itemDx.TnmDx.Tumor.Split(',')[1];
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                            if (itemDx.TnmDx.Estado.Length > 0)
                                dataCargar = dataCargar + "  Estado:" + itemDx.TnmDx.Estado.Split(',')[0] + " " + itemDx.TnmDx.Estado.Split(',')[1];
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                            if (itemDx.TnmDx.Nodulo.Length > 0)
                                dataCargar = dataCargar + " Nodulo:" + itemDx.TnmDx.Nodulo.Split(',')[0] + " " + itemDx.TnmDx.Nodulo.Split(',')[1] + saltoLinea;
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
                            dataCargar = dataCargar + "Metastasis:" + itemDx.TnmDx.Metastasis + saltoLinea;
                            dataCargar = dataCargar + "Informacion resolucion 0247" + saltoLinea;
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                            dataCargar = dataCargar + "Fecha Informe Histopatologico Valido:" + itemDx.InfResol_0247.Fec_Inf_Histo_Val + saltoLinea;
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
                            dataCargar = dataCargar + "Fecha Recoleccion  de Muestra:" + itemDx.InfResol_0247.Fec_Rec_Muestra + saltoLinea;
                            dataCargar = dataCargar + "Grado de Diferenciacion:" + itemDx.InfResol_0247.Grado_Dif + saltoLinea;
                            dataCargar = dataCargar + "Histologia:" + itemDx.InfResol_0247.Histologia + saltoLinea;
                            dataCargar = dataCargar + "Objetivo Tratamiento Inicial:" + itemDx.InfResol_0247.Obj_Trata_Ini + saltoLinea;
                            dataCargar = dataCargar + "Objetivo Intervencion Medica:" + itemDx.InfResol_0247.Obj_Interv_Medica + saltoLinea;
                        }
                        dataCargar = dataCargar + "___________________FINAL DIAGNOSTICOS____________________" + saltoLinea;

                        DBConnection conn = new();
                        using (SqlConnection conexion = new(conn.getCs()))
                        {
                            conexion.Open();
                            SqlTransaction txTransaccion01 = conexion.BeginTransaction("TX1");
                            string actHistoria1 = "UPDATE hceNotasAte SET  DesNota=CONVERT(VARCHAR(MAX),DesNota)+@desNota WHERE IdNota=@nota AND IdAtencion=@atencion";
                            SqlCommand cmdNotasAte = new(actHistoria1, conexion, txTransaccion01);
                            cmdNotasAte.Parameters.Add("@nota", SqlDbType.Int).Value = NumeroNota;
                            cmdNotasAte.Parameters.Add("@atencion", SqlDbType.Int).Value = dxOrigen.IdAtencion;
                            cmdNotasAte.Parameters.Add("@fechaNota", SqlDbType.DateTime).Value = dxOrigen.Fecha;
                            cmdNotasAte.Parameters.Add("@ubicacion", SqlDbType.Int).Value = 30;
                            cmdNotasAte.Parameters.Add("@desNota", SqlDbType.VarChar).Value = dataCargar;
                            cmdNotasAte.Parameters.Add("@usuario", SqlDbType.SmallInt).Value = 0;  // Toca implementar el medico o profesional de SAHICO
                            cmdNotasAte.Parameters.Add("@tipoNota", SqlDbType.SmallInt).Value = 807;
                            logSahico.Info("********************* Valor de tipoNota:" + 807 + "  Nota:" + NumeroNota + "   Atencion:" + dxOrigen.IdAtencion + "****************************");
                            if (cmdNotasAte.ExecuteNonQuery() > 0)
                            {
                                logSahico.Info("Se Actualiza informacion en hceNotasAte O.K");
                                txTransaccion01.Commit();
                                diagnosticosResponse.Resultado = true;
                                diagnosticosResponse.Mensaje = "Transaccion Exitosa";
                                diagnosticosResponse.DetalleMensaje = "";
                                diagnosticosResponse.Atencion = dxOrigen.IdAtencion;
                                diagnosticosResponse.ConsultaSahico = dxOrigen.IdConsulta;
                                diagnosticosResponse.IdPaciente = dxOrigen.IdPaciente;
                                diagnosticosResponse.IdNotaSAHI = NumeroNota;
                                return Ok(diagnosticosResponse);

                            }
                            else
                            {
                                logSahico.Info("No fue posible realizar la transaccion de Actualizacion sobre la tabla:hceNotasAte ");
                                txTransaccion01.Rollback();
                                diagnosticosResponse.Resultado = false;
                                diagnosticosResponse.Mensaje = "!!! No fue posible realizar la transaccion sobre la tabla:hceNotasAte  !!!";
                                diagnosticosResponse.DetalleMensaje = "";
                                diagnosticosResponse.Atencion = dxOrigen.IdAtencion;
                                diagnosticosResponse.ConsultaSahico = dxOrigen.IdConsulta;
                                diagnosticosResponse.IdPaciente = dxOrigen.IdPaciente;
                                diagnosticosResponse.IdNotaSAHI = 0;
                                return Ok(diagnosticosResponse);
                            }
                            #region previo
                            //if (NumeroNota == 0)
                            //{
                            //////////////////NumeroNota = utilLocal.consecutivoSistabla("hceNotasAte");
                            //////////////////SqlTransaction txTransaccion01 = Conex00.BeginTransaction("TX1");
                            //////////////////string actHistoria1 = "INSERT INTO hceNotasAte (IdNota, IdAtencion, FecNota, IdUbicacion, DesNota, IdUsuarioR, IdTipoNota)	VALUES (@nota,@atencion,@fechaNota,@ubicacion,@desNota,@usuario,@tipoNota)";
                            //////////////////SqlCommand cmdNotasAte = new SqlCommand(actHistoria1, Conex00, txTransaccion01);
                            //////////////////cmdNotasAte.Parameters.Add("@nota", SqlDbType.Int).Value = NumeroNota;
                            //////////////////cmdNotasAte.Parameters.Add("@atencion", SqlDbType.Int).Value = dxOrigen.IdAtencion;
                            //////////////////cmdNotasAte.Parameters.Add("@fechaNota", SqlDbType.DateTime).Value = dxOrigen.Fecha;
                            //////////////////cmdNotasAte.Parameters.Add("@ubicacion", SqlDbType.Int).Value = 30;
                            //////////////////cmdNotasAte.Parameters.Add("@desNota", SqlDbType.VarChar).Value = dataCargar;
                            //////////////////cmdNotasAte.Parameters.Add("@usuario", SqlDbType.SmallInt).Value = 0;
                            //////////////////cmdNotasAte.Parameters.Add("@tipoNota", SqlDbType.SmallInt).Value = 807;
                            //////////////////logSahico.Info("********************* Valor de tipoNota:" + 807 + "  Nota:" + NumeroNota + "   Atencion:" + dxOrigen.IdAtencion + "****************************");
                            //////////////////if (cmdNotasAte.ExecuteNonQuery() > 0)
                            //////////////////{
                            //////////////////    logSahico.Info("Se inserta informacion en hceNotasAte O.K");
                            //////////////////    string actHistoria2 = "INSERT INTO hceEsquemasdeAte (IdAtencion, IdEsquema, IdEsquemadeAtencion, IdUbicacion, IdMedico, IdTraslado, FecEsquema, IndHabilitado, IndActivado, FecCerrado, EstadoApDx, idOrden,IndResCritico ) ";
                            //////////////////    actHistoria2 = actHistoria2 + "VALUES (@atencion,@esquema,@esquemaAte,@ubicacion, @medico,@traslado,@fechaEsquema,@indicadorHabilitado, @indicadorActivado,@fechaCerrado, @EstadoApDx, @orden,@rCritico)";
                            //////////////////    SqlCommand cmdEsquemasAte = new SqlCommand(actHistoria2, Conex00, txTransaccion01);
                            //////////////////    cmdEsquemasAte.Parameters.Add("@atencion", SqlDbType.Int).Value = dxOrigen.IdAtencion;
                            //////////////////    cmdEsquemasAte.Parameters.Add("@esquema", SqlDbType.Int).Value = 807;
                            //////////////////    cmdEsquemasAte.Parameters.Add("@esquemaAte", SqlDbType.Int).Value = NumeroNota;
                            //////////////////    cmdEsquemasAte.Parameters.Add("@ubicacion", SqlDbType.SmallInt).Value = 30;
                            //////////////////    cmdEsquemasAte.Parameters.Add("@medico", SqlDbType.SmallInt).Value = 0;
                            //////////////////    cmdEsquemasAte.Parameters.Add("@traslado", SqlDbType.Int).Value = 1;
                            //////////////////    cmdEsquemasAte.Parameters.Add("@fechaEsquema", SqlDbType.DateTime).Value = dxOrigen.Fecha;
                            //////////////////    cmdEsquemasAte.Parameters.Add("@indicadorHabilitado", SqlDbType.Bit).Value = 1;
                            //////////////////    cmdEsquemasAte.Parameters.Add("@indicadorActivado", SqlDbType.Bit).Value = 0;
                            //////////////////    cmdEsquemasAte.Parameters.Add("@fechaCerrado", SqlDbType.DateTime).Value = DateTime.Now;
                            //////////////////    cmdEsquemasAte.Parameters.Add("@EstadoApDx", SqlDbType.Int).Value = 4;
                            //////////////////    cmdEsquemasAte.Parameters.Add("@orden", SqlDbType.Int).Value = dxOrigen.IdConsulta;
                            //////////////////    cmdEsquemasAte.Parameters.Add("@rCritico", SqlDbType.VarChar).Value = 0;
                            //////////////////    if (cmdEsquemasAte.ExecuteNonQuery() > 0)
                            //////////////////    {
                            //////////////////        if (utilLocal.insertaSahicoRel(dxOrigen.IdConsulta, NumeroNota, dxOrigen.IdAtencion, 187, dxOrigen.Fecha))
                            //////////////////        {
                            //////////////////            logSahico.Info("!!! Transaccion realizada Exitosamente !!!");
                            //////////////////            txTransaccion01.Commit();
                            //////////////////            diagnosticosResponse.Resultado = true;
                            //////////////////            diagnosticosResponse.Mensaje = "!!! Transaccion realizada Exitosamente !!!";
                            //////////////////            diagnosticosResponse.DetalleMensaje = "";
                            //////////////////            return diagnosticosResponse;
                            //////////////////        }
                            //////////////////        else
                            //////////////////        {
                            //////////////////            logSahico.Info("!!! No fue posible realizar la transaccion sobre la tabla:hceIntegraSahicoRel !!!");
                            //////////////////            txTransaccion01.Rollback();
                            //////////////////            diagnosticosResponse.Resultado = false;
                            //////////////////            diagnosticosResponse.Mensaje = "!!! No fue posible realizar la transaccion sobre la tabla:hceIntegraSahicoRel !!!";
                            //////////////////            diagnosticosResponse.DetalleMensaje = "";
                            //////////////////            return diagnosticosResponse;
                            //////////////////        }
                            //////////////////    }
                            //////////////////    else
                            //////////////////    {
                            //////////////////        logSahico.Info("!!! No fue posible realizar la transaccion sobre la tabla:hceEsquemasdeAte !!!");
                            //////////////////        txTransaccion01.Rollback();
                            //////////////////        diagnosticosResponse.Resultado = false;
                            //////////////////        diagnosticosResponse.Mensaje = "!!! No fue posible realizar la transaccion sobre la tabla:hceEsquemasdeAte !!!";
                            //////////////////        diagnosticosResponse.DetalleMensaje = "";
                            //////////////////        return diagnosticosResponse;
                            //////////////////    }
                            //////////////////}
                            //////////////////else
                            //////////////////{
                            //////////////////    logSahico.Info("No fue posible realizar la transaccion sobre la tabla:hceNotasAte ");
                            //////////////////    txTransaccion01.Rollback();
                            //////////////////    diagnosticosResponse.Resultado = false;
                            //////////////////    diagnosticosResponse.Mensaje = "!!! No fue posible realizar la transaccion sobre la tabla:hceNotasAte  !!!";
                            //////////////////    diagnosticosResponse.DetalleMensaje = "";
                            //////////////////    return diagnosticosResponse;
                            //////////////////}
                            //}
                            //else // Actualizar Nota en SAHI
                            //{
                            //    SqlTransaction txTransaccion01 = Conex00.BeginTransaction("TX1");
                            //    string actHistoria1 = "UPDATE hceNotasAte SET  DesNota=DesNota+@desNota WHERE IdNota=@nota AND IdAtencion=@atencion";
                            //    SqlCommand cmdNotasAte = new SqlCommand(actHistoria1, Conex00, txTransaccion01);
                            //    cmdNotasAte.Parameters.Add("@nota", SqlDbType.Int).Value = NumeroNota;
                            //    cmdNotasAte.Parameters.Add("@atencion", SqlDbType.Int).Value = dxOrigen.IdAtencion;
                            //    cmdNotasAte.Parameters.Add("@fechaNota", SqlDbType.DateTime).Value = dxOrigen.Fecha;
                            //    cmdNotasAte.Parameters.Add("@ubicacion", SqlDbType.Int).Value = 30;
                            //    cmdNotasAte.Parameters.Add("@desNota", SqlDbType.VarChar).Value = dataCargar;
                            //    cmdNotasAte.Parameters.Add("@usuario", SqlDbType.SmallInt).Value = 0;  // Toca implementar el medico o profesional de SAHICO
                            //    cmdNotasAte.Parameters.Add("@tipoNota", SqlDbType.SmallInt).Value = 807;
                            //    logSahico.Info("********************* Valor de tipoNota:" + 807 + "  Nota:" + NumeroNota + "   Atencion:" + dxOrigen.IdAtencion + "****************************");
                            //    if (cmdNotasAte.ExecuteNonQuery() > 0)
                            //    {
                            //        logSahico.Info("Se Actualiza informacion en hceNotasAte O.K");
                            //        txTransaccion01.Commit();
                            //        diagnosticosResponse.Resultado = true;
                            //        diagnosticosResponse.Mensaje = "!!! Transaccion realizada Exitosamente !!!";
                            //        diagnosticosResponse.DetalleMensaje = "";
                            //        return diagnosticosResponse;

                            //    }
                            //    else
                            //    {
                            //        logSahico.Info("No fue posible realizar la transaccion de Actualizacion sobre la tabla:hceNotasAte ");
                            //        txTransaccion01.Rollback();
                            //        diagnosticosResponse.Resultado = false;
                            //        diagnosticosResponse.Mensaje = "!!! No fue posible realizar la transaccion sobre la tabla:hceNotasAte  !!!";
                            //        diagnosticosResponse.DetalleMensaje = "";
                            //        return diagnosticosResponse;
                            //    }
                            //}
                            #endregion
                        }
                    }
                    else
                    {
                        diagnosticosResponse.Resultado = false;
                        diagnosticosResponse.Mensaje = "Operacion No puede ser ejecutada";
                        diagnosticosResponse.DetalleMensaje = "Por Favor Revisar los Parametros de Entrada.El mensaje de Entrada no Contiene Diagnosticos ";
                        diagnosticosResponse.Atencion = dxOrigen.IdAtencion;
                        diagnosticosResponse.ConsultaSahico = dxOrigen.IdConsulta;
                        diagnosticosResponse.IdPaciente = dxOrigen.IdPaciente;
                        diagnosticosResponse.IdNotaSAHI = 0;
                        return BadRequest(diagnosticosResponse);
                    }
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
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
                    errorResponse.Mensaje = "Se ha presentado una Excepcion de tipo General.";
                    return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
                }
            }
            else
            {
                diagnosticosResponse.Resultado = false;
                diagnosticosResponse.Mensaje = "No se ha dado inicio a la consulta.";
                diagnosticosResponse.DetalleMensaje = "No se ha consumido el servicio HistoriaClinica. Para asociar la Nota Medica en SAHI";
                diagnosticosResponse.Atencion = dxOrigen.IdAtencion;
                diagnosticosResponse.ConsultaSahico = dxOrigen.IdConsulta;
                diagnosticosResponse.IdPaciente = dxOrigen.IdPaciente;
                diagnosticosResponse.IdNotaSAHI = 0;
                return BadRequest(diagnosticosResponse);

            }
        }

        private DiagnosticosResponse ValidarEntrada(DiagnosticosRequest dxOrigen)
        {
            var response = new DiagnosticosResponse
            {
                Resultado = true,
                IdNotaSAHI = 0
            };
            var msg = "Se ha presentado una Excepcion: El siguiente campo es vacio";
            if (dxOrigen.IdPaciente == 0)
            {
                response.Resultado = false;
                msg += ", idPaciente";
            }
            else response.IdPaciente = dxOrigen.IdPaciente;
            if (dxOrigen.IdConsulta == 0)
            {
                response.Resultado = false;
                msg += ", idconsulta";
            }
            else response.ConsultaSahico = dxOrigen.IdConsulta;
            if (dxOrigen.IdAtencion == 0)
            {
                response.Resultado = false;
                msg += ", idAtencion";
            }
            else response.Atencion = dxOrigen.IdAtencion;
            response.Mensaje = msg;

            return response;
        }
    }
}
