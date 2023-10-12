using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace HUSI_SIISA.Models.Response
{
    /// <summary>
    /// Estructura de datos para la peticion del Servicio de CitaQuimioterapia
    /// </summary>
    /// 
    public class CitaQuimioterapiaResponse
    {
        /// <summary>
        /// Valor para representar el estado de la operacion o transaccion
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>bool</tipo_dato>
        [Required]
        [JsonProperty("resultado")]
        [JsonPropertyOrder(1)]
        public bool Resultado { get; set; }

        /// <summary>
        /// Mensaje de aclaracion del resultado
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>string</tipo_dato>
        [Required]
        [JsonProperty("mensaje")]
        [JsonPropertyOrder(2)]
        public string? Mensaje { get; set; }

        /// <summary>
        /// Detalles tecnicos del mensaje. en especial cuando se trata de fallas no esperadas
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>bool</tipo_dato>
        [Required]
        [JsonProperty("detalleMensaje")]
        [JsonPropertyOrder(3)]
        public string? DetalleMensaje { get; set; }
    }
}
