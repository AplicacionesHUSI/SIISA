using HUSI_SIISA.Utilities;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace HUSI_SIISA.Models.Request
{
    /// <summary>
    /// Estructura de datos para la peticion del Servicio de Inventario
    /// </summary>
    public class CitaQuimioterapiaRequest
    {

        /// <summary>
        /// ID de identificacion del paciente en SAHI
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        [Required]
        [JsonProperty("_id_paciente")]
        public Int32 ID_Paciente { get; set; }



        /// <summary>
        /// id de la atencion.
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        /// 
        [Required]
        [JsonProperty("_idAtencion")]
        public Int32 ID_Atencion { get; set; }

        /// <summary>
        /// ID o Numero de la cita
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        [Required]
        [JsonProperty("_id_cita")]
        public Int32 ID_Cita { get;  set;}

        /// <summary>
        /// ID del tipo de la cita.
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("_id_tipo_cita")]
        public string? ID_Tipo_Cita { get;  set;}

        /// <summary>
        /// Servicio solicitado desde Agenda
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>string</tipo_dato>
        [Required]
        [JsonProperty("_servicio_solicitado_agenta")]
        public string? Servicio_Solicitado_Agenta {get; set;}

        /// <summary>
        /// Servicio Solicitado desde la Consulta
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>string</tipo_dato>
        [Required]
        [JsonProperty("_servicio_solicitado_consuta")]
        public string? Servicio_Solicitado_Consuta {get; set;}

        /// <summary>
        /// 2Nombre EPS del paciente
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>string</tipo_dato>
        [Required]
        [JsonProperty("_eps")]
        public string? Eps {get; set; }

        /// <summary>
        /// Consultorio donde tiene lugar la cita
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>string</tipo_dato>
        [Required]
        [JsonProperty("_consultorio")]
        public string? Consultorio {get; set;}

        /// <summary>
        /// Id del medico
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Int16</tipo_dato>
        [Required]
        [JsonProperty("_idMedico")]
        public Int16 IdMedico {get; set;}


        /// <summary>
        /// Fecha y Hora de la Solicitud
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>string</tipo_dato>
        [Required]
        [JsonProperty("_fecha_hora_solicitud")]
        public DateTime Fecha_Hora_Solicitud {get; set;}

        /// <summary>
        /// Fecha y Hora de Inicio de la Consulta
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>DateTime</tipo_dato>
        [Required]
        [JsonProperty("_fecha_hora_inicio")]
        public DateTime Fecha_Hora_Inicio { get; set; }

        /// <summary>
        /// Fecha y Hora de finalizacion de la consulta
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>DateTime</tipo_dato>
        [Required]
        [JsonProperty("_fecha_hora_fin")]
        public DateTime Fecha_Hora_Fin {get; set;}

        /// <summary>
        /// Observaciones Generales
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>string</tipo_dato>
        [Required]
        [JsonProperty("_observaciones")]
        public string? Observaciones {get; set;}

        /// <summary>
        /// Nombre de la persona que Solicita la Cita
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>string</tipo_dato>
        [Required]
        [JsonProperty("_nombre_solicitante")]
        public string? Nombre_Solicitante {get; set;}

        /// <summary>
        /// Fecha que solicita el usuario
        /// </summary>
        [Required]
        [JsonProperty("_fecha_solicitud_usuario")]
        public DateTime Fecha_Solicitud_Usuario {get; set;}
    }
}
