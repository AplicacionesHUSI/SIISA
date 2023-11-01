using HUSI_SIISA.DBContext;
using HUSI_SIISA.Models.Request;
using HUSI_SIISA.Models.Response;
using HUSI_SIISA.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NLog;
using System.Data;
using static HUSI_SIISA.Utilities.Utilidades;

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
        public async Task<ActionResult> InsertarHCAsync([FromBody] HistoriaClinicaRequest historiaRequest)
        {
            HistoriaClinicaResponse historiaResponse = new();

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
                    ErrorResponse errorResponse = new()
                    {
                        Codigo = 500,
                        Mensaje = "Error al convertir la fecha de la consulta, comuníquese con el administrador."
                    };
                    return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
                }
            }

            Utilidades utilLocal = new();
            string dataCargar = string.Empty;
            string salto = Environment.NewLine;
            Int32 NumeroNota = 0;
            Int16 Profesional = 0;
            try
            {
                //clienteInfMed.ImedicosWSClient clienteProfesionales = new clienteInfMed.ImedicosWSClient();
                //clienteInfMed.RespuestasWS rptaProfesionales = clienteProfesionales.idUsuarioPersonal(historiaInsertar.ID_Profesional);
                //if (rptaProfesionales.CodigoRpta.Equals("00"))
                //{
                //    Profesional = Int16.Parse(rptaProfesionales.resultado);
                //}

                DBConnection conn = new();
                using (SqlConnection conexion = new(conn.getCs()))
                {
                    conexion.Open();

                    string strConsultar = string.Empty;
                    string qryuConsultaAtn = "SELECT idAtencion FROM admAtencion WHERE idAtencion=@atencion AND IndActivado=1";
                    SqlCommand cmdConsultaAtn = new(qryuConsultaAtn, conexion);
#pragma warning disable CS8604 // Posible argumento de referencia nulo
                    cmdConsultaAtn.Parameters.Add("@atencion", SqlDbType.Int).Value = Int32.Parse(historiaRequest.IdAtencion);
#pragma warning restore CS8604 // Posible argumento de referencia nulo
                    SqlDataReader rdConsultaAtn = await cmdConsultaAtn.ExecuteReaderAsync();

                    if (historiaRequest.IdAtencion.Length > 0 && rdConsultaAtn.HasRows)
                    {
                        rdConsultaAtn.Close();
                        dataCargar = "*********** INFORMACION INGRESADA POR SAHICO ************" + salto;
                        //dataCargar = dataCargar + "Fecha Consulta:" + historiaInsertar.fechaConsulta + salto;
                        dataCargar = dataCargar + "Fecha Consulta:" + DateTime.Now.ToString() + salto;
                        dataCargar = dataCargar + "Numero Consulta:" + historiaRequest.IdConsulta + salto;
                        dataCargar = dataCargar + "Numero de Atencion:" + historiaRequest.IdAtencion;
                        dataCargar = dataCargar + "  ID del Paciente:" + historiaRequest.IdPaciente + salto;
                        dataCargar = dataCargar + "Informacion:" + salto;
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                        foreach (ConsultaConceptos concepto in historiaRequest.Conceptos)
                        {
                            dataCargar = dataCargar + concepto.Titulo +":"+ salto;
                            dataCargar = dataCargar + concepto.Cuerpo + salto;
                            string sqlinser = @"INSERT INTO HceSiisaDatos (FecReg,Idconsulta, IdAtencion, IdCliente, IdMedico,titulo, cuerpo)	
                                VALUES (@fecha,@consulta,@atencion,@cliente,@medico,@titulo,@cuerpo)";
                            SqlCommand cmdinser = new SqlCommand(sqlinser, conexion);
                            cmdinser.Parameters.Add("@fecha", SqlDbType.DateTime).Value = DateTime.Now;
                            cmdinser.Parameters.Add("@consulta", SqlDbType.Int).Value =Convert.ToInt32(historiaRequest.IdConsulta);
                            cmdinser.Parameters.Add("@atencion", SqlDbType.Int).Value = Convert.ToInt32(historiaRequest.IdAtencion);
                            //cmdNotasAte.Parameters.Add("@fechaNota", SqlDbType.DateTime).Value = DateTime.Parse(historiaInsertar.fechaConsulta);
                            cmdinser.Parameters.Add("@cliente", SqlDbType.Int).Value = Convert.ToInt32(historiaRequest.IdPaciente);
                            cmdinser.Parameters.Add("@medico", SqlDbType.Int).Value = (historiaRequest.IdProfesional);
                            cmdinser.Parameters.Add("@titulo", SqlDbType.Text).Value = concepto.Titulo;
                            cmdinser.Parameters.Add("@cuerpo", SqlDbType.Text).Value = concepto.Cuerpo;
                            logSahico.Info("********************* Valor de tipoNota:" + 807 + "  Nota:" + NumeroNota + "   Atencion:" + historiaRequest.IdAtencion + "****************************");
                            if (cmdinser.ExecuteNonQuery() > 0)
                            {

                                logSahico.Info("Se inserto correctamente idconsulta" + historiaRequest.IdConsulta + " concepto:" + concepto.Titulo);
                            }
                            else
                            {
                                logSahico.Info("No se pudo insertar datos de concepto  idconsulta" + historiaRequest.IdConsulta + " concepto:" + concepto.Titulo);
                            }

                        }
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
                        dataCargar = dataCargar + "__________________________________________________________" + salto;
                        //***********************************************************************************
                        logSahico.Info("Datos para cargar a Historia:" + dataCargar);
                        SqlTransaction txTransaccion01 = conexion.BeginTransaction("TX1");
#pragma warning disable CS8604 // Posible argumento de referencia nulo
                        ValidacionNotas objNotas = utilLocal.ValidaConsulta(Int32.Parse(historiaRequest.IdConsulta), "1", Int32.Parse(historiaRequest.IdAtencion));
#pragma warning restore CS8604 // Posible argumento de referencia nulo
                        NumeroNota = objNotas.IdNota;
                        //NumeroNota = utilLocal.validaConsulta(Int32.Parse(historiaInsertar.idconsulta), "1", Int32.Parse(historiaInsertar.idAtencion));
                        if (NumeroNota == 0)
                        {
                            NumeroNota = utilLocal.ConsecutivoSistabla("hceNotasAte");

                            string actHistoria1 = @"INSERT INTO hceNotasAte (IdNota, IdAtencion, FecNota, IdUbicacion, DesNota, IdUsuarioR, IdTipoNota)	VALUES (@nota,@atencion,@fechaNota,@ubicacion,@desNota,@usuario,@tipoNota)";
                            SqlCommand cmdNotasAte = new SqlCommand(actHistoria1, conexion, txTransaccion01);
                            cmdNotasAte.Parameters.Add("@nota", SqlDbType.Int).Value = NumeroNota;
                            cmdNotasAte.Parameters.Add("@atencion", SqlDbType.Int).Value = historiaRequest.IdAtencion;
                            //cmdNotasAte.Parameters.Add("@fechaNota", SqlDbType.DateTime).Value = DateTime.Parse(historiaInsertar.fechaConsulta);
                            cmdNotasAte.Parameters.Add("@fechaNota", SqlDbType.DateTime).Value = DateTime.Now;
                            cmdNotasAte.Parameters.Add("@ubicacion", SqlDbType.Int).Value = 30;
                            cmdNotasAte.Parameters.Add("@desNota", SqlDbType.Text).Value = dataCargar;
                            cmdNotasAte.Parameters.Add("@usuario", SqlDbType.SmallInt).Value = Profesional;
                            cmdNotasAte.Parameters.Add("@tipoNota", SqlDbType.SmallInt).Value = 807;
                            logSahico.Info("********************* Valor de tipoNota:" + 807 + "  Nota:" + NumeroNota + "   Atencion:" + historiaRequest.IdAtencion + "****************************");
                            if (cmdNotasAte.ExecuteNonQuery() > 0)
                            {
                                logSahico.Info("Se inserta informacion en hceNotasAte O.K");
                                string actHistoria2 = @"INSERT INTO hceEsquemasdeAte (IdAtencion, IdEsquema, IdEsquemadeAtencion, IdUbicacion, IdMedico, IdTraslado, FecEsquema, IndHabilitado, IndActivado, FecCerrado, EstadoApDx, idOrden,IndResCritico) 
                                    VALUES (@atencion,@esquema,@esquemaAte,@ubicacion, @medico,@traslado,@fechaEsquema,@indicadorHabilitado, @indicadorActivado,@fechaCerrado, @EstadoApDx, @orden,@rCritico)";
                                SqlCommand cmdEsquemasAte = new SqlCommand(actHistoria2, conexion, txTransaccion01);
                                cmdEsquemasAte.Parameters.Add("@atencion", SqlDbType.Int).Value = historiaRequest.IdAtencion;
                                cmdEsquemasAte.Parameters.Add("@esquema", SqlDbType.Int).Value = 807;
                                cmdEsquemasAte.Parameters.Add("@esquemaAte", SqlDbType.Int).Value = NumeroNota;
                                cmdEsquemasAte.Parameters.Add("@ubicacion", SqlDbType.SmallInt).Value = 30;
                                cmdEsquemasAte.Parameters.Add("@medico", SqlDbType.SmallInt).Value = Profesional;
                                cmdEsquemasAte.Parameters.Add("@traslado", SqlDbType.Int).Value = 1;
                                //cmdEsquemasAte.Parameters.Add("@fechaEsquema", SqlDbType.DateTime).Value = DateTime.Parse(historiaInsertar.fechaConsulta);
                                cmdEsquemasAte.Parameters.Add("@fechaEsquema", SqlDbType.DateTime).Value = DateTime.Now;
                                cmdEsquemasAte.Parameters.Add("@indicadorHabilitado", SqlDbType.Bit).Value = 1;
                                cmdEsquemasAte.Parameters.Add("@indicadorActivado", SqlDbType.Bit).Value = 0;
                                cmdEsquemasAte.Parameters.Add("@fechaCerrado", SqlDbType.DateTime).Value = DateTime.Now;
                                cmdEsquemasAte.Parameters.Add("@EstadoApDx", SqlDbType.Int).Value = 4;
                                cmdEsquemasAte.Parameters.Add("@orden", SqlDbType.Int).Value = historiaRequest.IdConsulta;
                                cmdEsquemasAte.Parameters.Add("@rCritico", SqlDbType.VarChar).Value = 0;
                                var result = await cmdEsquemasAte.ExecuteNonQueryAsync();
                                if (result > 0)
                                {
                                    if (utilLocal.InsertaSahicoRel(Int32.Parse(historiaRequest.IdConsulta), NumeroNota, Int32.Parse(historiaRequest.IdAtencion), 807, DateTime.Now, 0))
                                    {
                                        logSahico.Info("!!! Transaccion realizada Exitosamente !!!");
                                        txTransaccion01.Commit();
                                        historiaResponse.Resultado = true;
                                        historiaResponse.Mensaje = "Transaccion Exitosa";
                                        historiaResponse.DetalleMensaje = "";
                                        historiaResponse.Atencion = Int32.Parse(historiaRequest.IdAtencion);
                                        historiaResponse.ConsultaSahico = Int32.Parse(historiaRequest.IdConsulta);
#pragma warning disable CS8604 // Posible argumento de referencia nulo
                                        historiaResponse.IdPaciente = Int32.Parse(historiaRequest.IdPaciente);
#pragma warning restore CS8604 // Posible argumento de referencia nulo
                                        historiaResponse.IdNotaSAHI = NumeroNota;
                                        return Ok(historiaResponse);
                                    }
                                    else
                                    {
                                        logSahico.Info("!!! No fue posible realizar la transaccion sobre la tabla:hceIntegraSahicoRel !!!");
                                        txTransaccion01.Rollback();
                                        historiaResponse.Resultado = false;
                                        historiaResponse.Mensaje = "Se ha presentado una Excepcion: No fue posible insertar sobre la tabla hceIntegraSahicoRel";
                                        historiaResponse.DetalleMensaje = "";
                                        historiaResponse.Atencion = Int32.Parse(historiaRequest.IdAtencion);
                                        historiaResponse.ConsultaSahico = Int32.Parse(historiaRequest.IdConsulta);
#pragma warning disable CS8604 // Posible argumento de referencia nulo
                                        historiaResponse.IdPaciente = Int32.Parse(historiaRequest.IdPaciente);
#pragma warning restore CS8604 // Posible argumento de referencia nulo
                                        historiaResponse.IdNotaSAHI = 0;
                                        return StatusCode(StatusCodes.Status500InternalServerError, historiaResponse);
                                    }
                                }
                                else
                                {
                                    logSahico.Info("!!! No fue posible realizar la INSERCION sobre la tabla:hceEsquemasdeAte !!!");
                                    txTransaccion01.Rollback();
                                    historiaResponse.Resultado = false;
                                    historiaResponse.Mensaje = "Se ha presentado una Excepcion: No fue posible insertar sobre la tabla hceEsquemasdeAte";
                                    historiaResponse.DetalleMensaje = "";
                                    historiaResponse.Atencion = Int32.Parse(historiaRequest.IdAtencion);
                                    historiaResponse.ConsultaSahico = Int32.Parse(historiaRequest.IdConsulta);
#pragma warning disable CS8604 // Posible argumento de referencia nulo
                                    historiaResponse.IdPaciente = Int32.Parse(historiaRequest.IdPaciente);
#pragma warning restore CS8604 // Posible argumento de referencia nulo
                                    historiaResponse.IdNotaSAHI = 0;
                                    return StatusCode(StatusCodes.Status500InternalServerError, historiaResponse);
                                }
                            }
                            else
                            {
                                logSahico.Info("No fue posible realizar la INSERCION sobre la tabla:hceNotasAte ");
                                txTransaccion01.Rollback();
                                historiaResponse.Resultado = false;
                                historiaResponse.Mensaje = "Se ha presentado una Excepcion: No fue posible realizar INSERCION sobre la tabla hceNotasAte";
                                historiaResponse.DetalleMensaje = "";
                                historiaResponse.Atencion = Int32.Parse(historiaRequest.IdAtencion);
                                historiaResponse.ConsultaSahico = Int32.Parse(historiaRequest.IdConsulta);
#pragma warning disable CS8604 // Posible argumento de referencia nulo
                                historiaResponse.IdPaciente = Int32.Parse(historiaRequest.IdPaciente);
#pragma warning restore CS8604 // Posible argumento de referencia nulo
                                historiaResponse.IdNotaSAHI = 0;
                                return StatusCode(StatusCodes.Status500InternalServerError, historiaResponse);
                            }
                            //***********************************************************************************   
                        }
                        else // ******** Actualizar Nota en SAHI
                        {
                            //SqlTransaction txTransaccion01 = Conex00.BeginTransaction("TX1");
                            string actHistoria1 = "UPDATE hceNotasAte SET DesNota=CONVERT(VARCHAR(MAX),DesNota)+@desNota WHERE IdNota=@nota AND IdAtencion=@atencion";
                            SqlCommand cmdNotasAte = new(actHistoria1, conexion, txTransaccion01);
                            cmdNotasAte.Parameters.Add("@nota", SqlDbType.Int).Value = NumeroNota;
                            cmdNotasAte.Parameters.Add("@atencion", SqlDbType.Int).Value = historiaRequest.IdAtencion;
                            //cmdNotasAte.Parameters.Add("@fechaNota", SqlDbType.DateTime).Value = DateTime.Parse(historiaInsertar.fechaConsulta);
                            cmdNotasAte.Parameters.Add("@fechaNota", SqlDbType.DateTime).Value = DateTime.Now;
                            cmdNotasAte.Parameters.Add("@ubicacion", SqlDbType.Int).Value = 30;
                            cmdNotasAte.Parameters.Add("@desNota", SqlDbType.VarChar).Value = dataCargar;
                            cmdNotasAte.Parameters.Add("@usuario", SqlDbType.SmallInt).Value = Profesional;
                            cmdNotasAte.Parameters.Add("@tipoNota", SqlDbType.SmallInt).Value = 807;
                            logSahico.Info("********************* Valor de tipoNota:" + 807 + "  Nota:" + NumeroNota + "   Atencion:" + historiaRequest.IdAtencion + "****************************");
                            var result = await cmdNotasAte.ExecuteNonQueryAsync();

                            if (result > 0)
                            {
                                logSahico.Info("Se Actualiza informacion en hceNotasAte O.K");
                                txTransaccion01.Commit();
                                historiaResponse.Resultado = true;
                                historiaResponse.Mensaje = "Transaccion Exitosa";
                                historiaResponse.DetalleMensaje = "";
                                historiaResponse.Atencion = Int32.Parse(historiaRequest.IdAtencion);
                                historiaResponse.ConsultaSahico = Int32.Parse(historiaRequest.IdConsulta);
#pragma warning disable CS8604 // Posible argumento de referencia nulo
                                historiaResponse.IdPaciente = Int32.Parse(historiaRequest.IdPaciente);
#pragma warning restore CS8604 // Posible argumento de referencia nulo
                                historiaResponse.IdNotaSAHI = NumeroNota;
                                return Ok(historiaResponse);
                            }
                            else
                            {
                                logSahico.Info("No fue posible realizar la transaccion de Actualizacion sobre la tabla:hceNotasAte ");
                                txTransaccion01.Rollback();
                                historiaResponse.Resultado = false;
                                historiaResponse.Mensaje = "Se ha presentado una Excepcion: actualizacion sobre la tabla hceNotasAte";
                                historiaResponse.DetalleMensaje = "";
                                historiaResponse.Atencion = Int32.Parse(historiaRequest.IdAtencion);
                                historiaResponse.ConsultaSahico = Int32.Parse(historiaRequest.IdConsulta);
#pragma warning disable CS8604 // Posible argumento de referencia nulo
                                historiaResponse.IdPaciente = Int32.Parse(historiaRequest.IdPaciente);
#pragma warning restore CS8604 // Posible argumento de referencia nulo
                                historiaResponse.IdNotaSAHI = 0;
                                return StatusCode(StatusCodes.Status500InternalServerError, historiaResponse);
                            }
                        }
                    }
                    else
                    {
                        logSahico.Info("La informacion enviada no cuenta con un numero de Atencion Valido");
                        historiaResponse.Resultado = false;
                        historiaResponse.Mensaje = "Se ha presentado una Excepcion:  La informacion enviada no cuenta con un numero de Atencion Valido";
                        historiaResponse.Atencion = Int32.Parse(historiaRequest.IdAtencion);
#pragma warning disable CS8604 // Posible argumento de referencia nulo
                        historiaResponse.ConsultaSahico = Int32.Parse(historiaRequest.IdConsulta);
                        historiaResponse.IdPaciente = Int32.Parse(historiaRequest.IdPaciente);
#pragma warning restore CS8604 // Posible argumento de referencia nulo
                        historiaResponse.IdNotaSAHI = 0;
                        historiaResponse.DetalleMensaje = "";
                        return BadRequest(historiaResponse);
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
