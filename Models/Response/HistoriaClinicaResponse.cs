using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HUSI_SIISA.Models.Response
{
    /// <summary>
    /// Estructura de datos para la respuesta del Servicio de Historia Clinica
    /// </summary>
    public class HistoriaClinicaResponse
    {
        /// <summary>
        /// Valor para representar el estado de la operacion o transaccion
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>Boolean</tipo_dato>
        [Required]
        [JsonProperty("resultado")]
        [JsonPropertyOrder(1)]
        public Boolean Resultado { get; set; }

        /// <summary>
        /// Mensaje de aclaracion del resultado
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("mensaje")]
        [JsonPropertyOrder(2)]
        public string? Mensaje { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>Int32</tipo_dato>
        [Required]
        [JsonProperty("atencion")]
        [JsonPropertyOrder(3)]
        public Int32 Atencion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>Int32</tipo_dato>
        [Required]
        [JsonProperty("consultaSahico")]
        [JsonPropertyOrder(4)]
        public Int32 ConsultaSahico { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>Int32</tipo_dato>
        [Required]
        [JsonProperty("idPaciente")]
        [JsonPropertyOrder(5)]
        public Int32 IdPaciente { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>Int32</tipo_dato>
        [Required]
        [JsonProperty("idNotaSAHI")]
        [JsonPropertyOrder(6)]
        public Int32 IdNotaSAHI { get; set; }

        /// <summary>
        /// Detalles tecnicos del mensaje. en especial cuando se trata de fallas no esperadas
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("detalleMensaje")]
        [JsonPropertyOrder(7)]
        public string? DetalleMensaje { get; set; }
    }
}
