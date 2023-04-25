using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HUSI_SIISA.Models.Request
{
    /// <summary>
	/// Modelo General de datos para procedimientos.
	/// </summary>
    public class OrdenarProcedimientosRequest
    {
        /// <summary>
        /// ID Consulta. Numero de la consulta
        /// </summary>
        /// <tipo_campo>Integer</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("idConsulta")]
        public Int32 IdConsulta { get; set; }

        /// <summary>
        /// ID Paciente. identificacion del paciente. Corresponde al ID en SAHI
        /// </summary>
        /// <tipo_campo>Integer</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("idPaciente")]
        public Int32 IdPaciente { get; set; }

        /// <summary>
        /// ID o Numero de atencion. corresponde al idAtencion en SAHI
        /// </summary>
        /// <tipo_campo>Integer</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("atencion")]
        public Int32 Atencion { get; set; }

        /// <summary>
        /// Fecha del procedimiento.
        /// </summary>
        /// <tipo_campo>Datetime</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("fecha")]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// ID del Medico. corresponde al Idpersonal en SAHI
        /// </summary>
        /// <tipo_campo>Integer</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("idMedico")]
        public Int32 IdMedico { get; set; }

        /// <summary>
        /// Listado de procedimientos.
        /// </summary>
        /// <tipo_campo>Complejo (Itemproce)</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("items")]
        public List<Itemproce>? Items { get; set; }
    }

    /// <summary>
    /// Modelo de Datos para Procedimiento
    /// </summary>
    public class Itemproce
    {
        /// <summary>
        /// ID del Procedimiento. Identificador equivalente en SAHI
        /// </summary>
        /// <tipo_campo>Integer</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("idProcedimiento")]
        public Int32 IdProcedimiento { get; set; }

        /// <summary>
        /// Codigo CUP del procedimiento.
        /// </summary>
        /// <tipo_campo>String</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("cups")]
        public string? Cups { get; set; }

        /// <summary>
        /// Cantidad del procedimiento requerida.
        /// </summary>
        /// <tipo_campo>smallint</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("cantidad")]
        public Int16 Cantidad { get; set; }

        /// <summary>
        /// Observaciones acerca del procedimiento
        /// </summary>
        /// <tipo_campo>String</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("observaciones")]
        public string? Observaciones { get; set; }

        /// <summary>
        /// Numero MiPres.
        /// </summary>
        /// <tipo_campo>String</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("num_MiPres")]
        public string? Num_MiPres { get; set; }

        /// <summary>
        /// Informacion correspondiente a la Resolucion 3047
        /// </summary>
        /// <tipo_campo>Complejo Res3047</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("res3047Item")]
        public Res3047? Res3047Item { get; set; }

        /// <summary>
        /// Tesxto corresponduiente a la Interpretacion
        /// </summary>
        /// <tipo_campo>String</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("interpretacion")]
        public string? Interpretacion { get; set; }
    }

    /// <summary>
    /// Modelo de Datos para la gestion de la Informacion de la Resolucion 3047
    /// </summary>
    public class Res3047
    {
        /// <summary>
        /// Numero de identificacion.
        /// </summary>
        /// <tipo_campo>String</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("numero")]
        public string? Numero { get; set; }

        /// <summary>
        /// Origen de la Atencion del paciente
        /// </summary>
        /// <tipo_campo>String</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("origen_Atencion")]
        public string? Origen_Atencion { get; set; }

        /// <summary>
        /// Tipo de Servicios Solicitados.
        /// </summary>
        /// <tipo_campo>String</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("tipo_Serv_Solicitados")]
        public string? Tipo_Serv_Solicitados { get; set; }

        /// <summary>
        /// Ubicacion del Paciente.
        /// </summary>
        /// <tipo_campo>String</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("ubiacion_Paciente")]
        public string? Ubiacion_Paciente { get; set; }

        /// <summary>
        /// Prioridad establecida.
        /// </summary>
        /// <tipo_campo>String</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("prioridad")]
        public string? Prioridad { get; set; }

        /// <summary>
        /// Guia de manejo establecida para el paciente.
        /// </summary>
        /// <tipo_campo>String</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("guia_Manejo")]
        public string? Guia_Manejo { get; set; }

        /// <summary>
        /// Usuario Responsable
        /// </summary>
        /// <tipo_campo>String</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("usr_Responsable")]
        public string? Usr_Responsable { get; set; }

        /// <summary>
        /// Observaciones generales que se requieran
        /// </summary>
        /// <tipo_campo>String</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("observaciones")]
        public string? Observaciones { get; set; }

        /// <summary>
        /// Informacion de la  Interpretacion
        /// </summary>
        /// <tipo_campo>String</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("interpretacion")]
        public string? Interpretacion { get; set; }
    }
}
