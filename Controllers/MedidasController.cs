using System.Data;
using HUSI_SIISA.DBContext;
using System.Text;
using System.Xml.Serialization;
using HUSI_SIISA.Models.Request;
using HUSI_SIISA.Models.Response;
using HUSI_SIISA.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NLog;
using static HUSI_SIISA.Utilities.Utilidades;

namespace HUSI_SIISA.Controllers
{
    /// <summary>
    /// Modelo de control para Medidas
    /// </summary>
    
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class MedidasController : ControllerBase
    {
        private static Logger logSahico = LogManager.GetCurrentClassLogger();

        // POST: api/Medidas/ActualizarMedidasPac
        /// <summary>
        /// Operacion No implementada
        /// </summary>
        /// <param name="medidasPcte">No Im plementada</param>
        /// <returns>Objeto Respue
        /// staII</returns>
        /// <remarks>
        /// Sample request:
        ///     POST api/Medidas/ActualizarMedidasPac
        ///     {
        ///         "atencion": "",
        ///         "orden": "",
        ///         "idUbicacion": "",
        ///         "idEsquema": ""
        ///     }
        /// </remarks>
        [HttpPost]
        [Route("ActualizarMedidasPac")]
        public MedidasResponse ActualizarMedidasPac([FromBody] MedidasRequest medidasPcte)
        {
            MedidasResponse medidasResponse = new();
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
            if (medidasPcte.IdPaciente.Length > 0 && medidasPcte.IdConsulta > 0)
            {
                return medidasResponse;
            }
            else
            {
                return medidasResponse;
            }
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
        }

        //public MedidasResponse ConsultarMedidasPac(string idPaciente, string nroConsulta)
        //{
        //    throw new NotImplementedException();
        //}

        // POST: api/Medidas/InsertarMedidasPac
        /// <summary>
        /// Operaacion para insertar las medidas del paciente en una consulta. Esta operacion adiciona las medidas en la Nota de la Consulta
        /// </summary>
        /// <param name="medidasPac">Objeto Medias, poblado con los datos esteblecidos por el Medico en la consulta</param>
        /// <returns></returns>
        /// /// <remarks>
        /// Sample request:
        ///     POST api/Medidas/InsertarMedidasPac
        ///     {
        ///         "atencion": "",
        ///         "orden": "",
        ///         "idUbicacion": "",
        ///         "idEsquema": ""
        ///     }
        /// </remarks>

        [HttpPost]
        [Route("InsertarMedidasPac")]
        public ActionResult InsertarMedidasPac([FromBody] MedidasRequest medidasPac)
        {
            StringBuilder serializado = new StringBuilder();
            XmlSerializer SerializadorMedidas = new XmlSerializer(typeof(MedidasRequest));
            StringWriter swWriter = new StringWriter(serializado);
            SerializadorMedidas.Serialize(swWriter, medidasPac);
            logSahico.Info("Mensaje Recibido de Orden de medidas:" + serializado.ToString());
            //get adm atencion
            if (string.IsNullOrEmpty(medidasPac.NroAtencion))
            {
                string auxAten = Utilidades.GetAdmAtencion(medidasPac.IdPaciente + "", DateTime.Now);
                if (!string.IsNullOrEmpty(auxAten)) medidasPac.NroAtencion= auxAten;
            }
            //entry validation
            MedidasResponse medidasResponse = ValidarEntrada(medidasPac.NroAtencion, medidasPac.NroConsulta, medidasPac.IdPaciente, medidasPac);
            if (!medidasResponse.Resultado)
            {
                logSahico.Info($" No se puede procesar el mensaje porque falta informacion::	{medidasResponse.Mensaje}");
                return BadRequest(medidasResponse);
            }
            //end validation

            if (medidasPac.IdPaciente.Length > 0 && medidasPac.IdConsulta > 0)
            {
                Utilidades utilLocal = new Utilidades();
                string dataCargar = string.Empty;
                string salto = Environment.NewLine;
                Int32 NumeroNota = 0;

                //clientePacientesHusi.husiCliente pacienteW = new clientePacientesHusi.husiCliente();
                try
                {
                    if (medidasPac.IdConsulta > 0 && medidasPac.IdPaciente.Length > 0)
                    {

                        logSahico.Info("Consumo de Servicio de Medidas: Nro Consulta:" + medidasPac.NroConsulta + "  medidasPac.IdPaciente:" + medidasPac.IdPaciente);
                        //clientePacientesHusi.IhusiClienteWSClient paciente = new clientePacientesHusi.IhusiClienteWSClient();
                        //pacienteW = paciente.Consulta_V3(medidasPac.IdPaciente);
                        logSahico.Info("Datos Paciente: Nro Doc:" + "1111111"/*pacienteW.NumDocumento*/ + "   Tipo Doc:" + "CCPrueba"/*pacienteW.IdTipoDoc*/);
                        //clienteWSatenciones.IatencionesClient atencionesCli = new clienteWSatenciones.IatencionesClient();
                        //clienteWSatenciones.Atencion atencionPaciente = atencionesCli.ConsAtenXDoc(pacienteW.NumDocumento, pacienteW.IdTipoDoc);
                        logSahico.Info("Numero de Atencion del Paciente:" + medidasPac.NroAtencion);
                        //clienteWSatenciones.Atencion datosAtencion = atencionesCli.ConsAtenXDoc(pacienteW.NumDocumento, pacienteW.IdTipoDoc);
                        dataCargar = "________________________MEDIDAS___________________________" + salto;
                        dataCargar = dataCargar + "Fecha:" + DateTime.Now;
                        dataCargar = dataCargar + " Numero de Atencion:" + medidasPac.NroAtencion + salto + "                         Numero Consulta:" + medidasPac.IdConsulta + salto;
                        dataCargar = dataCargar + "No Documento:" + "111111"/*pacienteW.NumDocumento*/ + "  Fecha de Nacimiento " + "21/21/2121"/*pacienteW.FecNacimiento.ToString("dd/MM/yyyy")*/ + salto;
                        dataCargar = dataCargar + "Paciente:" + "Nombreprueba"/*pacienteW.NomCliente*/ + " " + "apellidoprueba"/*pacienteW.ApeCliente*/ + "         Tel:" + "31233"/*pacienteW.TelCasa*/ + salto;
                        dataCargar = dataCargar + " " + salto;
                        dataCargar = dataCargar + "Peso:" + medidasPac.Peso + salto;
                        dataCargar = dataCargar + "Talla:" + medidasPac.Talla + salto;
                        dataCargar = dataCargar + "Indice Superficie Corporal:" + medidasPac.Indice_Super_Corporal + salto;
                        dataCargar = dataCargar + "Indice Masa Corporal:" + medidasPac.Ind_Masa_Corporal + salto;
                        dataCargar = dataCargar + "Frecuencia Cardiaca:" + medidasPac.Frecuencia_Cardiaca + salto;
                        dataCargar = dataCargar + "Temperatura:" + medidasPac.Temperatura + salto;
                        dataCargar = dataCargar + "Presion Arterial Sistolica:" + medidasPac.Presion_Art_Sist + salto;
                        dataCargar = dataCargar + "Presion Arterial Diastolica:" + medidasPac.Presion_Art_Dias + salto;
                        dataCargar = dataCargar + "Frecuencia Respiratora:" + medidasPac.Frec_Respiratoria + salto;
                        dataCargar = dataCargar + "Estado" + medidasPac.Estado + salto;
                        dataCargar = dataCargar + "Glucometria:" + medidasPac.Glucometria + salto;
                        dataCargar = dataCargar + "Saturacion Oxigeno:" + medidasPac.Saturacion_Oxigeno + salto;
                        dataCargar = dataCargar + "Escalas" + salto;

#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                        foreach (indices itemScala in medidasPac.Escalas)
                        {
                            dataCargar = dataCargar + itemScala.Nombre_Escala + ":" + itemScala.Valor + salto;
                        }
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
                        dataCargar = dataCargar + "_____________________FINAL MEDIDAS________________________" + salto;


                        DBConnection conn = new();
                        using (SqlConnection conexion = new(conn.getCs()))
                        {
                            conexion.Open();
                            ValidacionNotas objNotas = utilLocal.ValidaConsulta(medidasPac.IdConsulta, "1", Int32.Parse(medidasPac.NroAtencion));
                            NumeroNota = objNotas.IdNota;

                            if (NumeroNota == 0) // Insertar Nota Nueva en SAHI
                            {
                                #region Opcional para insertar nota nueva
                                //////////Int32 NumeroNota = medidasPac.IdConsulta; /// ojo aqui debe enviarse el numero de nora en HC cuando es nueva o la existente
                                ////////NumeroNota = utilLocal.consecutivoSistabla("hceNotasAte");
                                ////////SqlTransaction txTransaccion01 = Conex00.BeginTransaction("TX1");
                                ////////string actHistoria1 = "INSERT INTO hceNotasAte (IdNota, IdAtencion, FecNota, IdUbicacion, DesNota, IdUsuarioR, IdTipoNota)	VALUES (@nota,@atencion,@fechaNota,@ubicacion,@desNota,@usuario,@tipoNota)";
                                ////////SqlCommand cmdNotasAte = new SqlCommand(actHistoria1, Conex00, txTransaccion01);
                                ////////cmdNotasAte.Parameters.Add("@nota", SqlDbType.Int).Value = NumeroNota;
                                ////////cmdNotasAte.Parameters.Add("@atencion", SqlDbType.Int).Value = datosAtencion.medidasPac.NroAtencion;
                                ////////cmdNotasAte.Parameters.Add("@fechaNota", SqlDbType.DateTime).Value = datosAtencion.Fechaatencion;
                                ////////cmdNotasAte.Parameters.Add("@ubicacion", SqlDbType.Int).Value = 30;
                                ////////cmdNotasAte.Parameters.Add("@desNota", SqlDbType.VarChar).Value = dataCargar;
                                ////////cmdNotasAte.Parameters.Add("@usuario", SqlDbType.SmallInt).Value = 0;//????????????????????Aqui voy Toca implementar el Medico o Profesional de SAHICO
                                ////////cmdNotasAte.Parameters.Add("@tipoNota", SqlDbType.SmallInt).Value = 807;
                                ////////logSahico.Info("********************* Valor de tipoNota:" + 807 + "  Nota:" + NumeroNota + "   Atencion:" + datosAtencion.medidasPac.NroAtencion + "****************************");
                                ////////if (cmdNotasAte.ExecuteNonQuery() > 0)
                                ////////{
                                ////////    logSahico.Info("Se inserta informacion en hceNotasAte O.K Medico que Ordena:" + 0);
                                ////////    string actHistoria2 = "INSERT INTO hceEsquemasdeAte (IdAtencion, IdEsquema, IdEsquemadeAtencion, IdUbicacion, IdMedico, IdTraslado, FecEsquema, IndHabilitado, IndActivado, FecCerrado, EstadoApDx, idOrden,IndResCritico ) ";
                                ////////    actHistoria2 = actHistoria2 + "VALUES (@atencion,@esquema,@esquemaAte,@ubicacion, @medico,@traslado,@fechaEsquema,@indicadorHabilitado, @indicadorActivado,@fechaCerrado, @EstadoApDx, @orden,@rCritico)";
                                ////////    SqlCommand cmdEsquemasAte = new SqlCommand(actHistoria2, Conex00, txTransaccion01);
                                ////////    cmdEsquemasAte.Parameters.Add("@atencion", SqlDbType.Int).Value = 0;// procedimientos.Atencion;
                                ////////    cmdEsquemasAte.Parameters.Add("@esquema", SqlDbType.Int).Value = 807;
                                ////////    cmdEsquemasAte.Parameters.Add("@esquemaAte", SqlDbType.Int).Value = NumeroNota;
                                ////////    cmdEsquemasAte.Parameters.Add("@ubicacion", SqlDbType.SmallInt).Value = 30;
                                ////////    cmdEsquemasAte.Parameters.Add("@medico", SqlDbType.SmallInt).Value = 0;// medicoOrdena;
                                ////////    cmdEsquemasAte.Parameters.Add("@traslado", SqlDbType.Int).Value = 1;
                                ////////    cmdEsquemasAte.Parameters.Add("@fechaEsquema", SqlDbType.DateTime).Value = DateTime.Now;// procedimientos.Fecha;
                                ////////    cmdEsquemasAte.Parameters.Add("@indicadorHabilitado", SqlDbType.Bit).Value = 1;
                                ////////    cmdEsquemasAte.Parameters.Add("@indicadorActivado", SqlDbType.Bit).Value = 0;
                                ////////    cmdEsquemasAte.Parameters.Add("@fechaCerrado", SqlDbType.DateTime).Value = DateTime.Now;
                                ////////    cmdEsquemasAte.Parameters.Add("@EstadoApDx", SqlDbType.Int).Value = 4;
                                ////////    cmdEsquemasAte.Parameters.Add("@orden", SqlDbType.Int).Value = 0;//procedimientos.IdConsulta;
                                ////////    cmdEsquemasAte.Parameters.Add("@rCritico", SqlDbType.VarChar).Value = 0;
                                ////////    if (cmdEsquemasAte.ExecuteNonQuery() > 0)
                                ////////    {
                                ////////        if (utilLocal.insertaSahicoRel(medidasPac.IdConsulta, NumeroNota, datosAtencion.medidasPac.NroAtencion, 187, DateTime.Now))
                                ////////        {
                                ////////            logSahico.Info("!!! Transaccion realizada Exitosamente !!!");
                                ////////            txTransaccion01.Commit();
                                ////////            medidasResponse.Resultado = true;
                                ////////            medidasResponse.Mensaje = "!!! Transaccion realizada Exitosamente !!!";
                                ////////            medidasResponse.DetalleMensaje = "";
                                ////////            return medidasResponse;
                                ////////        }
                                ////////        else
                                ////////        {
                                ////////            logSahico.Info("!!! No fue posible realizar la transaccion sobre la tabla:hceIntegraSahicoRel !!!");
                                ////////            txTransaccion01.Rollback();
                                ////////            medidasResponse.Resultado = false;
                                ////////            medidasResponse.Mensaje = "!!! No fue posible realizar la transaccion sobre la tabla:hceIntegraSahicoRel !!!";
                                ////////            medidasResponse.DetalleMensaje = "";
                                ////////            return medidasResponse;
                                ////////        }

                                ////////    }
                                ////////    else
                                ////////    {
                                ////////        logSahico.Info("!!! No fue posible realizar la transaccion sobre la tabla:hceEsquemasdeAte !!!");
                                ////////        txTransaccion01.Rollback();
                                ////////        medidasResponse.Resultado = false;
                                ////////        medidasResponse.Mensaje = "!!! No fue posible realizar la transaccion sobre la tabla:hceEsquemasdeAte !!!";
                                ////////        medidasResponse.DetalleMensaje = "";
                                ////////        return medidasResponse;
                                ////////    }
                                ////////}
                                ////////else
                                ////////{
                                ////////    logSahico.Info("No fue posible realizar la transaccion sobre la tabla:hceNotasAte ");
                                ////////    txTransaccion01.Rollback();
                                ////////    medidasResponse.Resultado = false;
                                ////////    medidasResponse.Mensaje = "!!! No fue posible realizar la transaccion sobre la tabla:hceNotasAte  !!!";
                                ////////    medidasResponse.DetalleMensaje = "";
                                ////////    return medidasResponse;
                                ////////}
                                //
                                #endregion
                                medidasResponse.Resultado = false;
                                medidasResponse.Mensaje = "!!! Transaccion NO realizada. No se cargado una Historia Previamente para esta consulta !!!";
                                medidasResponse.DetalleMensaje = "Es encesario, primero crear la consulta guardando Historia";
                                medidasResponse.Atencion = Int32.Parse(medidasPac.NroAtencion);
                                medidasResponse.ConsultaSahico = Int32.Parse(medidasPac.NroConsulta);
                                medidasResponse.IdPaciente = Int32.Parse(medidasPac.IdPaciente);
                                medidasResponse.IdNotaSAHI = NumeroNota;
                                return BadRequest(medidasResponse);
                            }
                            else // Actualizar Nota en SAHI
                            {
                                SqlTransaction txTransaccion01 = conexion.BeginTransaction("TX1");
                                string actHistoria1 = "UPDATE hceNotasAte SET  DesNota=CONVERT(VARCHAR(MAX),DesNota)+@desNota WHERE IdNota=@nota AND IdAtencion=@atencion";
                                SqlCommand cmdNotasAte = new(actHistoria1, conexion, txTransaccion01);
                                cmdNotasAte.Parameters.Add("@nota", SqlDbType.Int).Value = NumeroNota;
                                cmdNotasAte.Parameters.Add("@atencion", SqlDbType.Int).Value = Int32.Parse(medidasPac.NroAtencion);
                                cmdNotasAte.Parameters.Add("@fechaNota", SqlDbType.DateTime).Value = DateTime.Now;
                                cmdNotasAte.Parameters.Add("@ubicacion", SqlDbType.Int).Value = 30;
                                cmdNotasAte.Parameters.Add("@desNota", SqlDbType.VarChar).Value = dataCargar;
                                cmdNotasAte.Parameters.Add("@usuario", SqlDbType.SmallInt).Value = 0; // No se tiene el medico en Medidas toca implementarlo
                                cmdNotasAte.Parameters.Add("@tipoNota", SqlDbType.SmallInt).Value = 807;
                                logSahico.Info("********************* Valor de tipoNota:" + 807 + "  Nota:" + NumeroNota + "   Atencion:" + medidasPac.NroAtencion + "****************************");
                                if (cmdNotasAte.ExecuteNonQuery() > 0)
                                {

                                    logSahico.Info("Se Actualiza informacion en hceNotasAte O.K");
                                    txTransaccion01.Commit();
                                    medidasResponse.Resultado = true;
                                    medidasResponse.Mensaje = "!!! Transaccion realizada Exitosamente !!!";
                                    medidasResponse.Atencion = Int32.Parse(medidasPac.NroAtencion);
                                    medidasResponse.ConsultaSahico = Int32.Parse(medidasPac.NroConsulta);
                                    medidasResponse.IdPaciente = Int32.Parse(medidasPac.IdPaciente);
                                    medidasResponse.IdNotaSAHI = NumeroNota;
                                    medidasResponse.DetalleMensaje = "";
                                    return Ok(medidasResponse);
                                }
                                else
                                {
                                    logSahico.Info("No fue posible Actualizar Registros sobre la tabla:hceNotasAte ");
                                    txTransaccion01.Rollback();
                                    medidasResponse.Resultado = false;
                                    medidasResponse.Mensaje = "!!! No fue posible Actualizar Registros sobre la tabla:hceNotasAte  !!!";
                                    medidasResponse.DetalleMensaje = "";
                                    medidasResponse.Atencion = Int32.Parse(medidasPac.NroAtencion);
                                    medidasResponse.ConsultaSahico = Int32.Parse(medidasPac.NroConsulta);
                                    medidasResponse.IdPaciente = Int32.Parse(medidasPac.IdPaciente);
                                    medidasResponse.IdNotaSAHI = 0;
                                    return BadRequest(medidasResponse);
                                }
                            }
                        }
                    }
                    else
                    {
                        logSahico.Info("No se proceso el mensaje, para la Atencion:" + medidasPac.NroAtencion + "  Nro Consulta" + medidasPac.NroConsulta + "IdPaciente:" + medidasPac.IdPaciente);
                        medidasResponse.Resultado = false;
                        medidasResponse.Mensaje = "!!! Transaccion NO realizada. Los Datos de Consulta o del paciente No son VALIDOS !!!";
                        medidasResponse.DetalleMensaje = "Es encesario, primero crear la consulta guardando Historia";
                        medidasResponse.Atencion = Int32.Parse(medidasPac.NroAtencion);
                        medidasResponse.ConsultaSahico = Int32.Parse(medidasPac.NroConsulta);
                        medidasResponse.IdPaciente = Int32.Parse(medidasPac.IdPaciente);
                        medidasResponse.IdNotaSAHI = 0;
                        return BadRequest(medidasResponse);

                    }
                }
                catch (Exception Ex)
                {
                    ErrorResponse errorResponse = new();
                    logSahico.Error("Error:: " + Ex.Message);
                    logSahico.Info("Se ha presentado una Excepcion:" + Ex.InnerException);
                    logSahico.Info("Se ha presentado una Excepcion:" + Ex.StackTrace);
                    errorResponse.Codigo = 500;
                    errorResponse.Mensaje = "Se ha presentado una Excepcion de tipo General.";
                    return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
                }
            }
            else
            {
                logSahico.Info("No se proceso el mensaje, para la Atencion:" + medidasPac.NroAtencion + "   nroConsulta" + medidasPac.NroConsulta + "IdPaciente:" + medidasPac.IdPaciente);
                medidasResponse.Resultado = false;
                medidasResponse.Mensaje = "!!! Transaccion NO realizada. Los Datos de Consulta o del paciente No son VALIDOS !!!";
                medidasResponse.DetalleMensaje = "Por Favor Revisar la Informacion suministrada en el consumo del Servicio";
                medidasResponse.Atencion = Int32.Parse(medidasPac.NroAtencion);
                medidasResponse.ConsultaSahico = Int32.Parse(medidasPac.NroConsulta);
                medidasResponse.IdPaciente = Int32.Parse(medidasPac.IdPaciente);
                medidasResponse.IdNotaSAHI = 0;
                return BadRequest(medidasResponse);
            }
        }

        private MedidasResponse ValidarEntrada(string nroAtencion, string nroConsulta, string idPaciente, MedidasRequest medidasPac)
        {

            var response = new MedidasResponse
            {
                Resultado = true,
                IdNotaSAHI = 0
            };
            var msg = "Se ha presentado una Excepcion: El siguiente campo es vacio";
            if (String.IsNullOrEmpty(idPaciente))
            {
                response.Resultado = false;
                msg += ", idPaciente";
            }
            else response.IdPaciente = Int32.Parse(idPaciente);
            if (medidasPac.IdConsulta == 0)
            {
                response.Resultado = false;
                msg += ", idconsulta";
            }
            else response.ConsultaSahico = medidasPac.IdConsulta;
            if (String.IsNullOrEmpty(nroAtencion))
            {
                response.Resultado = false;
                msg += ", idAtencion";
            }
            else response.Atencion = Int32.Parse(nroAtencion);
            response.Mensaje = msg;

            return response;
        }
    }
}
