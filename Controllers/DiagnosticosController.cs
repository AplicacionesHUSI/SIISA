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

        [HttpPost]
        [Route("InsertarDx")]
        public ActionResult InsertarDx([FromBody] DiagnosticosRequest dxOrigen)
        {
            try
            {
                // Serializar entrada
                using var sw = new StringWriter();
                var serializer = new XmlSerializer(typeof(DiagnosticosRequest));
                serializer.Serialize(sw, dxOrigen);
                logSahico.Info("Mensaje Recibido de diagnosticos: " + sw.ToString());

                // Obtener atención si no viene
                if (dxOrigen.IdAtencion == 0)
                {
                    string auxAten = Utilidades.GetAdmAtencion(dxOrigen.IdPaciente.ToString(), dxOrigen.Fecha);
                    if (!string.IsNullOrEmpty(auxAten))
                        dxOrigen.IdAtencion = int.Parse(auxAten);
                }

                // Validar entrada
                DiagnosticosResponse diagnosticosResponse = ValidarEntrada(dxOrigen);
                if (!diagnosticosResponse.Resultado)
                {
                    logSahico.Info($"No se puede procesar el mensaje porque falta informacion: {diagnosticosResponse.Mensaje}");
                    return BadRequest(diagnosticosResponse);
                }

                // Obtener tipo de consulta
                string tipoConsulta = dxOrigen.IdSede switch
                {
                    1 => "5", //cjo
                    68 => "4",//infecto
                    _ => ""
                };

                // Obtener el id Ubicación según la sede CJO(1) = 30 Infecto(68) = 42
                Int32 IdUbicación = dxOrigen.IdSede switch
                {
                    1 => 30,
                    68 => 42,
                    _ => (Int32)0
                };

                var utilLocal = new Utilidades();
                ValidacionNotas objNotas = utilLocal.ValidaConsulta(dxOrigen.IdConsulta, tipoConsulta, dxOrigen.IdAtencion);
                int sahico = objNotas.NroConsultaSahico;
                int numeroNota = objNotas.IdNota;

                if (numeroNota > 0)
                {
                    if (dxOrigen.Items_Dx?.Count > 0)
                    {
                        string dataCargar = GenerarTextoDiagnosticos(dxOrigen);

                        DBConnection conn = new();
                        using SqlConnection conexion = new(conn.getCs());
                        conexion.Open();

                        short tipoNota = dxOrigen.IdSede switch
                        {
                            1 => 807,
                            68 => 821,//consultas
                            _ => (short)0
                        };

                        using SqlTransaction tx = conexion.BeginTransaction("TX1");
                        SqlCommand cmd = new("UPDATE hceNotasAte SET DesNota=CONVERT(VARCHAR(MAX),DesNota)+@desNota WHERE IdNota=@nota AND IdAtencion=@atencion", conexion, tx);
                        cmd.Parameters.Add("@nota", SqlDbType.Int).Value = numeroNota;
                        cmd.Parameters.Add("@atencion", SqlDbType.Int).Value = dxOrigen.IdAtencion;
                        cmd.Parameters.Add("@fechaNota", SqlDbType.DateTime).Value = dxOrigen.Fecha;
                        cmd.Parameters.Add("@ubicacion", SqlDbType.Int).Value = IdUbicación;
                        cmd.Parameters.Add("@desNota", SqlDbType.VarChar).Value = dataCargar;
                        cmd.Parameters.Add("@usuario", SqlDbType.SmallInt).Value = 0;
                        cmd.Parameters.Add("@tipoNota", SqlDbType.SmallInt).Value = tipoNota;

                        logSahico.Info($"********************* Valor de tipoNota: {tipoNota}  Nota: {numeroNota}   Atencion: {dxOrigen.IdAtencion} ****************************");

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            tx.Commit();
                            diagnosticosResponse.Resultado = true;
                            diagnosticosResponse.Mensaje = "Transaccion Exitosa";
                            diagnosticosResponse.Atencion = dxOrigen.IdAtencion;
                            diagnosticosResponse.ConsultaSahico = sahico;
                            diagnosticosResponse.IdPaciente = dxOrigen.IdPaciente;
                            diagnosticosResponse.IdNotaSAHI = numeroNota;

                            logSahico.Info("Se Actualiza informacion en hceNotasAte O.K");
                            return Ok(diagnosticosResponse);
                        }
                        else
                        {
                            tx.Rollback();
                            diagnosticosResponse.Resultado = false;
                            diagnosticosResponse.Mensaje = "!!! No fue posible realizar la transaccion sobre la tabla:hceNotasAte  !!!";
                            diagnosticosResponse.Atencion = dxOrigen.IdAtencion;
                            diagnosticosResponse.ConsultaSahico = dxOrigen.IdConsulta;
                            diagnosticosResponse.IdPaciente = dxOrigen.IdPaciente;
                            diagnosticosResponse.IdNotaSAHI = 0;

                            logSahico.Info("No fue posible realizar la transaccion de Actualizacion sobre la tabla:hceNotasAte ");
                            return Ok(diagnosticosResponse);
                        }
                    }
                    else
                    {
                        diagnosticosResponse.Resultado = false;
                        diagnosticosResponse.Mensaje = "Operacion No puede ser ejecutada";
                        diagnosticosResponse.DetalleMensaje = "Por Favor Revisar los Parametros de Entrada. El mensaje no contiene Diagnosticos.";
                        diagnosticosResponse.Atencion = dxOrigen.IdAtencion;
                        diagnosticosResponse.ConsultaSahico = dxOrigen.IdConsulta;
                        diagnosticosResponse.IdPaciente = dxOrigen.IdPaciente;
                        diagnosticosResponse.IdNotaSAHI = 0;
                        return BadRequest(diagnosticosResponse);
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
            catch (SqlException sqlEx)
            {
                logSahico.Error("Error en Base de Datos :: " + sqlEx.Message);
                logSahico.Info("Excepcion SQL: " + sqlEx.InnerException);
                logSahico.Info("StackTrace: " + sqlEx.StackTrace);

                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
                {
                    Codigo = 500,
                    Mensaje = "Se ha presentado una Excepcion de SQL."
                });
            }
            catch (Exception ex)
            {
                logSahico.Error("Error:: " + ex.Message);
                logSahico.Info("Excepcion general: " + ex.InnerException);
                logSahico.Info("StackTrace: " + ex.StackTrace);

                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
                {
                    Codigo = 500,
                    Mensaje = "Se ha presentado una Excepcion de tipo General."
                });
            }
        }


        private string GenerarTextoDiagnosticos(DiagnosticosRequest dx)
        {
            // Obtener el id Ubicación según la sede CJO(1) = 30 Infecto(68) = 42
            Int32 IdUbicación = dx.IdSede switch
            {
                1 => 30,
                68 => 42,
                _ => (Int32)0
            };

            var sb = new StringBuilder();
            var nl = Environment.NewLine;

            sb.AppendLine("_____________________DIAGNOSTICOS_________________________");
            sb.AppendLine("Fecha:" + dx.Fecha);

            foreach (var item in dx.Items_Dx)
            {
                sb.AppendLine($"Codigo Dx: {item.CodigoDx}     Nombre: {item.NombreDx}");
                if (IdUbicación == 30)
                {
                    sb.AppendLine($"Tipo: {item.Tipo}      Confirmado: {item.Confirmado}");
                    sb.AppendLine("T.N.M");

                    if (!string.IsNullOrEmpty(item.TnmDx?.Tumor))
                    {
                        var partes = item.TnmDx.Tumor.Split(',');
                        if (partes.Length >= 2)
                            sb.AppendLine($"Tumor: {partes[0]} {partes[1]}");
                    }

                    if (!string.IsNullOrEmpty(item.TnmDx?.Estado))
                    {
                        var partes = item.TnmDx.Estado.Split(',');
                        if (partes.Length >= 2)
                            sb.AppendLine($"Estado: {partes[0]} {partes[1]}");
                    }

                    if (!string.IsNullOrEmpty(item.TnmDx?.Nodulo))
                    {
                        var partes = item.TnmDx.Nodulo.Split(',');
                        if (partes.Length >= 2)
                            sb.AppendLine($"Nodulo: {partes[0]} {partes[1]}");
                    }

                    sb.AppendLine("Metastasis: " + item.TnmDx?.Metastasis);
                    sb.AppendLine("Informacion resolucion 0247");

                    if (item.InfResol_0247 != null)
                    {
                        sb.AppendLine("Fecha Informe Histopatologico Valido: " + item.InfResol_0247.Fec_Inf_Histo_Val);
                        sb.AppendLine("Fecha Recoleccion  de Muestra: " + item.InfResol_0247.Fec_Rec_Muestra);
                        sb.AppendLine("Grado de Diferenciacion: " + item.InfResol_0247.Grado_Dif);
                        sb.AppendLine("Histologia: " + item.InfResol_0247.Histologia);
                        sb.AppendLine("Objetivo Tratamiento Inicial: " + item.InfResol_0247.Obj_Trata_Ini);
                        sb.AppendLine("Objetivo Intervencion Medica: " + item.InfResol_0247.Obj_Interv_Medica);
                    }
                }
            }

            sb.AppendLine("___________________FINAL DIAGNOSTICOS____________________");
            return sb.ToString();
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
