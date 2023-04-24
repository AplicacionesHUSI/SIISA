using HUSI_SIISA.DBContext;
using Microsoft.Data.SqlClient;
using NLog;
using System.Data;
using System.Xml;
using System.Xml.Serialization;

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
    }
}
