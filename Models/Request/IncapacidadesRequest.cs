using HUSI_SIISA.Utilities;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace HUSI_SIISA.Models.Request
{
    /// <summary>
    /// Contrato de Datos
    /// Modelo para Incapacidades
    /// </summary>
    public class IncapacidadesRequest
    {
        /// <summary>
        /// ID de la Incapacidad
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        [Required]
        [JsonProperty("idincapacidad")]
        public Int32 IdIncapacidad {get; set;}

        /// <summary>
        /// ID de la Atencion en SAHI
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        [Required]
        [JsonProperty("idatencion")]
        public Int32 IdAtencion {get; set;}

        /// <summary>
        /// ID de paciente en SAHI
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        [Required]
        [JsonProperty("idpaciente")]
        public Int32 IdPaciente {get; set;}

        /// <summary>
        /// ID o Numero de la consulta
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        [Required]
        [JsonProperty("idconsulta")]
        public Int32 IdConsulta {get; set;}


        /// <summary>
        /// Fecha de Expedicion de la consulta.
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Datetime</tipo_dato>
        [Required]
        [JsonProperty("fecha")]
        public DateTime Fecha {get; set;}

        /// <summary>
        /// Nombre de la Entidad
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("entidad")]
        public string? Entidad {get; set;}

        /// <summary>
        /// Tipo de Incapacidad.
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("tipoincapacidad")]
        public string? TipoIncapacidad {get; set;}

        /// <summary>
        /// Servicio 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("servicio")]
        public string? Servicio {get; set;}

        /// <summary>
        /// Numero de dias de la Incapacidad.
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>SmallInt</tipo_dato>
        [Required]
        [JsonProperty("nro_dias")]
        public Int16 Nro_Dias {get; set;}

        /// <summary>
        /// Fecha de Inicio de la Incapacidad
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Datetime</tipo_dato>
        [Required]
        [JsonProperty("fecha_inicio")]
        public DateTime Fecha_Inicio {get; set;}

        /// <summary>
        /// FEcha de Finalizacion de la Incapacidad
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Datetime</tipo_dato>
        [Required]
        [JsonProperty("fecha_final")]
        public DateTime Fecha_Final {get; set;}

        /// <summary>
        /// Diagnosticos asociados a la Incapacidad
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Complejo - Diagnostico</tipo_dato>
        [Required]
        [JsonProperty("diagnosticos")]
        public List<Diagnostico>? Diagnosticos {get; set;}

        /// <summary>
        /// Listado de profesionales realcionados con la Incapacidad
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Complejo - Profesionales</tipo_dato>
        [Required]
        [JsonProperty("profesionales")]
        public List<Profesional>? Profesionales {get; set;}
    }
}
