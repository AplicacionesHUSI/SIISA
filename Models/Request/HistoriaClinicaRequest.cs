using HUSI_SIISA.Utilities;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HUSI_SIISA.Models.Request
{
    /// <summary>
    /// Estructura de datos para la peticion del Servicio de Historia Clinica
    /// </summary>
    public class HistoriaClinicaRequest
    {

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Int32</tipo_dato>
        [Required]
        [JsonProperty("idConsulta")]
        public Int32 IdConsulta { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("idAtencion")]
        public string? IdAtencion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Int32</tipo_dato>
        [Required]
        [JsonProperty("idPaciente")]
        public Int32 IdPaciente { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("fechaConsulta")]
        public string? FechaConsulta { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Int32</tipo_dato>
        [Required]
        [JsonProperty("idProfesional")]
        public Int32 IdProfesional { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("analisis")]
        public string? Analisis { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("plan")]
        public string? Plan { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Conceptos</tipo_dato>
        [Required]
        [JsonProperty("conceptos")]
        public ConsultaConceptos[]? Conceptos { get; set; }
    }
}
