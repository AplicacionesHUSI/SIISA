using HUSI_SIISA.DBContext;
using Microsoft.Data.SqlClient;
using NLog;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using static HUSI_SIISA.Utilities.Utilidades;

namespace HUSI_SIISA.Utilities
{
    /// <summary>
    /// Metodo con funciones Utilitarias
    /// </summary>
    public class Utilidades
    {
        private static Logger logLabcore = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Metodo para convertir un  objeto XML
        /// </summary>
        /// <param name="o">object a trasformar en xml</param>
        /// <returns>XML del objeto recibido</returns>
        public string GetXMLFromObject(object o)
        {
            XmlSerializer serializer = new(o.GetType());
            StringWriter sw = new();
            XmlTextWriter tw = new(sw);
            serializer.Serialize(tw, o);
            return sw.ToString();
        }

        /// <summary>
        /// Metodo para consultar el codigo de la Atencion
        /// </summary>
        /// <param name="idCliente">Codigo de Cliente</param>
        /// <param name="fecIngreso">Fecha de ingreso</param>
        /// <returns>Código de atencion</returns>
        public static string GetAdmAtencion(string idCliente, DateTime fecIngreso)
        {
            string atencion = string.Empty;
            DBConnection conn = new();
            using (SqlConnection conexion = new(conn.getCs()))
            {
                conexion.Open();
                string strConsultar = $@"select top 1 IdAtencion from admAtencion  where IdAtencionTipo in (27,48) AND IndHabilitado=1 AND IndActivado=1 
                                            AND  idcliente={idCliente} ORDER BY IdAtencionTipo ";

                SqlCommand cmdConsulta = new(strConsultar, conexion);
                cmdConsulta.Parameters.Add("@fecIngreso", SqlDbType.DateTime).Value = fecIngreso;
                SqlDataReader conCursor = cmdConsulta.ExecuteReader();
                if (conCursor.HasRows)
                {
                    conCursor.Read();
                    atencion = conCursor.GetInt32(0) + "";

                }
                else
                {
                    conCursor.Close();
                    strConsultar = $@"select top 1 IdAtencion from admAtencion  where IdAtencionTipo in (27,48) AND IndHabilitado=1 AND 
                                        AND CONVERT(VARCHAR, FecIngreso,112)= CONVERT(VARCHAR,  @fecIngreso, 112) and idcliente={idCliente}  ORDER BY IdAtencionTipo";
                    cmdConsulta = new SqlCommand(strConsultar, conexion);
                    cmdConsulta.Parameters.Add("@fecIngreso", SqlDbType.DateTime).Value = fecIngreso;

                    conCursor = cmdConsulta.ExecuteReader();
                    if (conCursor.HasRows)
                    {
                        conCursor.Read();
                        atencion = conCursor.GetInt32(0) + "";
                    }
                }
            }
            Utilidades.logLabcore.Info("Atencion encontrada: " + atencion + " para idcliente: " + idCliente + " fecingreso: " + fecIngreso.ToString());

            return atencion;
        }

        /// <summary>
        /// Funcion para validar si la Consulta en SAHICO, ya cuenta con una Nota en Historia.
        /// </summary>
        /// <param name="nroConsulta">Numero de consulta en SAHI. corresponde a idConsulta</param>
        /// <param name="tipo">Tipo de validacion (1:Historia,2:Medicamentos,3:Procedimientos)</param>
        /// <param name="idAtencion"> El ID de la atencion en la cual se esta atendiendo el paciente</param> 
        /// <returns>Numero de la Nota en SAHI</returns>
        public ValidacionNotas ValidaConsulta(int nroConsulta, string tipo, int idAtencion)
        {
            string qryValida = string.Empty;
            ValidacionNotas rpta = new();
            DBConnection conn = new();
            using (SqlConnection conexion = new(conn.getCs()))
            {
                conexion.Open();
                switch (tipo)
                {
                    case "1":
                        qryValida = "SELECT nroConsultaSahico,idNota,procedimientos,idNotaProc,medicamentos,idNotaMed FROM  hceIntegraSahicoRel  WHERE idAtencion=@idAtencion AND nroConsultaSahico=@nroConsulta";
                        break;
                    case "2":
                        qryValida = "SELECT nroConsultaSahico,idNota,procedimientos,idNotaProc,medicamentos,idNotaMed FROM  hceIntegraSahicoRel WHERE idAtencion=@idAtencion AND  nroConsultaSahico=@nroConsulta";
                        break;
                    case "3":
                        qryValida = "SELECT nroConsultaSahico,idNota,procedimientos,idNotaProc,medicamentos,idNotaMed FROM  hceIntegraSahicoRel WHERE idAtencion=@idAtencion AND nroConsultaSahico=@nroConsulta";
                        break;
                }
                SqlCommand cmdValida = new SqlCommand(qryValida, conexion);
                cmdValida.Parameters.Add("@nroConsulta", SqlDbType.Int).Value = nroConsulta;
                cmdValida.Parameters.Add("@idAtencion", SqlDbType.Int).Value = idAtencion;
                SqlDataReader rdvalida = cmdValida.ExecuteReader();
                if (rdvalida.HasRows)
                {
                    rdvalida.Read();
                    if (rdvalida.GetInt32(0) == nroConsulta)
                    {
                        rpta.NroConsultaSahico = rdvalida.GetInt32(0);
                        rpta.IdNota = rdvalida.GetInt32(1);
                        rpta.Procedimientos = rdvalida.GetBoolean(2);
                        rpta.IdNotaProc = rdvalida.GetInt32(3);
                        rpta.Medicamentos = rdvalida.GetBoolean(4);
                        rpta.IdNotaMed = rdvalida.GetInt32(5);
                        return rpta;
                    }
                    else
                    {
                        return rpta;
                    }
                }
                else
                {
                    return rpta;
                }
            }
        }

        /// <summary>
        /// utilidad para obtener consecutivo de la tabla: Sistabla
        /// </summary>
        /// <param name="tabla">Nombre de la tabla para cual se desea obtener el consecutivo</param>
        /// <returns>Numero consecutivo Obtenido</returns>
        public Int32 ConsecutivoSistabla(string tabla)
        {
            //hceNotasAte
            try
            {
                Int32 nroConsecutivo = 0;
                DBConnection conn = new();
                using (SqlConnection conexion = new(conn.getCs()))
                {
                    conexion.Open();
                    SqlTransaction txResultados = conexion.BeginTransaction("TxResultados");
                    string NroNota = "SELECT (Consecutivo + 1) FROM sisTabla WITH (NOLOCK) WHERE NomTabla = '" + tabla + "'";
                    SqlCommand cmdNroNota = new SqlCommand(NroNota, conexion, txResultados);
                    SqlDataReader readerNroNota = cmdNroNota.ExecuteReader();
                    if (readerNroNota.HasRows)
                    {
                        readerNroNota.Read();
                        nroConsecutivo = readerNroNota.GetInt32(0);
                        readerNroNota.Close();
                        readerNroNota.Dispose();
                        string qryActualizaConsec = "UPDATE sistabla SET consecutivo = " + nroConsecutivo + " WHERE nomTabla = '" + tabla + "'";
                        SqlCommand cmdUpdNota = new SqlCommand(qryActualizaConsec, conexion, txResultados);
                        if (cmdUpdNota.ExecuteNonQuery() < 0)
                        {
                            logLabcore.Warn("No se Actualizo el Consecutivo:" + nroConsecutivo);
                            txResultados.Rollback();
                        }
                        else
                        {
                            txResultados.Commit();
                        }
                    }
                }
                return nroConsecutivo;
            }
            catch (Exception exp)
            {
                logLabcore.Warn(exp.Message, "Excepcion Obteniendo consecutivo para:" + tabla);
                return 0;
            }
        }

        /// <summary>
        /// Inserta la consulta de SAHICO en la tabla de relacion con Notas de SAHI
        /// </summary>
        /// <param name="nroConsultaSahico">Numero de consulta en SAHICO</param>
        /// <param name="idNota">Id de Nota en SAHI</param>
        /// <param name="idAtencion">Numero de Atencion en SAHI</param>
        /// <param name="idTipoNota">Tipo de Nota en SAHI</param>
        /// <param name="fechaNota">Fecha de la realizacion de la Nota</param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public Boolean InsertaSahicoRel(Int32 nroConsultaSahico, Int32 idNota, Int32 idAtencion, Int16 idTipoNota, DateTime fechaNota, Int16 tipo)
        {
            bool respuesta = false;
            DBConnection conn = new();
            using (SqlConnection conexion = new(conn.getCs()))
            {
                conexion.Open();
                string InsertaRelacion = string.Empty;
                ////////switch (tipo)
                ////////{
                ////////    case 0:
                ////////        InsertaRelacion = "INSERT INTO hceIntegraSahicoRel (nroConsultaSahico,idNota,idAtencion,idTipoNota,fechaNota) VALUES(@nroConsulta,@idNota,@idAtencion,@idTipoNota,@fechaNota)";
                ////////        break;
                ////////    case 1:
                ////////        InsertaRelacion = "INSERT INTO hceIntegraSahicoRel (nroConsultaSahico,idNota,idAtencion,idTipoNota,fechaNota,insumos,idNotaMed) VALUES(@nroConsulta,@idNota,@idAtencion,@idTipoNota,@fechaNota,1,@idNotaMed)";
                ////////        break;
                ////////    case 2:
                ////////        InsertaRelacion = "INSERT INTO hceIntegraSahicoRel (nroConsultaSahico,idNota,idAtencion,idTipoNota,fechaNota,medicamentos,idNotaMed) VALUES(@nroConsulta,@idNota,@idAtencion,@idTipoNota,@fechaNota,1,@idNotaMed)";
                ////////        break;
                ////////    case 3:
                ////////        InsertaRelacion = "INSERT INTO hceIntegraSahicoRel (nroConsultaSahico,idNota,idAtencion,idTipoNota,fechaNota,procedimientos,idNotaMed) VALUES(@nroConsulta,@idNota,@idAtencion,@idTipoNota,@fechaNota,1,@idNotaMed)";
                ////////        break;

                ////////}
                InsertaRelacion = "INSERT INTO hceIntegraSahicoRel (nroConsultaSahico,idNota,idAtencion,idTipoNota,fechaNota) VALUES(@nroConsulta,@idNota,@idAtencion,@idTipoNota,@fechaNota)";
                SqlCommand cmdActualizaRelacion = new SqlCommand(InsertaRelacion, conexion);
                cmdActualizaRelacion.Parameters.Add("@nroConsulta", SqlDbType.Int).Value = nroConsultaSahico;
                cmdActualizaRelacion.Parameters.Add("@idNota", SqlDbType.Int).Value = idNota;
                cmdActualizaRelacion.Parameters.Add("@idAtencion", SqlDbType.Int).Value = idAtencion;
                cmdActualizaRelacion.Parameters.Add("@idTipoNota", SqlDbType.SmallInt).Value = idTipoNota;
                cmdActualizaRelacion.Parameters.Add("@fechaNota", SqlDbType.DateTime).Value = fechaNota;
                if (cmdActualizaRelacion.ExecuteNonQuery() > 0)
                {
                    respuesta = true;
                }
                return respuesta;
            }
        }

        /// <summary>
        /// Actualiza una consulta de SAHICO en la tabla de relacion con Notas de SAHI
        /// </summary>
        /// <param name="nroConsultaSahico"></param>
        /// <param name="idNota"></param>
        /// <param name="nvaNota"></param>
        /// <param name="idAtencion"></param>
        /// <param name="fechaNota"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public Boolean ActualizarSahicoRel(Int32 nroConsultaSahico, Int32 idNota, Int32 nvaNota, Int32 idAtencion, DateTime fechaNota, Int16 tipo)
        {
            bool respuesta = false;
            DBConnection conn = new();
            using (SqlConnection conexion = new(conn.getCs()))
            {
                string ActualizaRelacion = string.Empty;
                conexion.Open();
                switch (tipo)
                {
                    case 1:
                        ActualizaRelacion = "UPDATE hceIntegraSahicoRel SET insumos=1,idNotaMed=@idNota WHERE nroConsultaSahico=@nroConsulta AND idNota=@idNota AND idAtencion=@idAtencion";
                        break;

                    case 2:
                        ActualizaRelacion = "UPDATE hceIntegraSahicoRel SET medicamentos=1,idNotaMed=@idNuevaNota WHERE nroConsultaSahico=@nroConsulta AND idNota=@idNota AND idAtencion=@idAtencion";
                        break;

                    case 3:
                        ActualizaRelacion = "UPDATE hceIntegraSahicoRel SET procedimientos=1,idNotaProc=@idNuevaNota WHERE nroConsultaSahico=@nroConsulta AND idNota=@idNota AND idAtencion=@idAtencion";
                        break;
                }


                SqlCommand cmdActualizaRelacion = new SqlCommand(ActualizaRelacion, conexion);
                cmdActualizaRelacion.Parameters.Add("@nroConsulta", SqlDbType.Int).Value = nroConsultaSahico;
                cmdActualizaRelacion.Parameters.Add("@idNota", SqlDbType.Int).Value = idNota;
                cmdActualizaRelacion.Parameters.Add("@idNuevaNota", SqlDbType.Int).Value = nvaNota;
                cmdActualizaRelacion.Parameters.Add("@idAtencion", SqlDbType.Int).Value = idAtencion;

                //cmdActualizaRelacion.Parameters.Add("@idTipoNota", SqlDbType.SmallInt).Value = 187;
                //cmdActualizaRelacion.Parameters.Add("@fechaNota", SqlDbType.DateTime).Value = fechaNota;
                if (cmdActualizaRelacion.ExecuteNonQuery() > 0)
                {
                    respuesta = true;
                }
                return respuesta;
            }
        }

        /// <summary>
        /// Estructura de Validacion de Notas
        /// </summary>
        public struct ValidacionNotas
        {
            /// <summary>
            /// 
            /// </summary>
            public Int32 NroConsultaSahico { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public Int32 IdNota { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public bool Procedimientos { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public Int32 IdNotaProc { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public bool Medicamentos { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public Int32 IdNotaMed { get; set; }
        }
    }
}
