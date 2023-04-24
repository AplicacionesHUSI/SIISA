using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HUSI_SIISA.Models.Response
{
    /// <summary>
    /// Estructura de datos para la respuesta de errores
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// HTTP Status Code.
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        [Required]
        [JsonProperty("codigo")]
        [JsonPropertyOrder(1)]
        public int Codigo { get; set; }

        /// <summary>
        /// Mensaje de error.
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("mensaje")]
        [JsonPropertyOrder(2)]
        public string? Mensaje { get; set; }
    }
}
