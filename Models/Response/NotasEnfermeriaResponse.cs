using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HUSI_SIISA.Models.Response
{
    /// <summary>
    /// Estructura de datos para la respuesta del Servicio de Notas Enferemeria
    /// </summary>
    public class NotasEnfermeriaResponse
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
        /// Detalles tecnicos del mensaje. en especial cuando se trata de fallas no esperadas
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("detalleMensaje")]
        [JsonPropertyOrder(3)]
        public string? DetalleMensaje { get; set; }
    }
}
