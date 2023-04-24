using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace HUSI_SIISA.Models.Response
{
    /// <summary>
    /// Estructura de datos para la respuesta del Servicio de Inventario
    /// </summary>
    public class InventarioResponse
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
        /// Resultado de la transacción 
        /// 00 transaccion ya realizada
        /// 01 exito
        /// 02 fallo
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>bool</tipo_dato>
        [Required]
        [JsonProperty("resultadoTransaccion")]
        [JsonPropertyOrder(2)]
        public string? ResultadoTransaccion { get; set; }

        /// <summary>
        /// Mensaje de aclaracion del resultado
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>string</tipo_dato>
        [Required]
        [JsonProperty("mensaje")]
        [JsonPropertyOrder(3)]
        public string? Mensaje { get; set; }

        /// <summary>
        /// Detalles tecnicos del mensaje. en especial cuando se trata de fallas no esperadas
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>bool</tipo_dato>
        [Required]
        [JsonProperty("detalleMensaje")]
        [JsonPropertyOrder(4)]
        public string? DetalleMensaje { get; set; }

        /// <summary>
		/// ID o Numero de Atencion en SAHI - Es el valor Recibido en el consumo del Servicio
		/// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Int32</tipo_dato>
        [Required]
        [JsonProperty("atencion")]
        [JsonPropertyOrder(5)]
        public Int32 Atencion { get; set; }

        /// <summary>
        /// ID o Numero del paciente
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>string</tipo_dato>
        [Required]
        [JsonProperty("documento")]
        [JsonPropertyOrder(5)]
        public string? Documento { get; set; }

        /// <summary>
        /// ID ConsecutivoRegistro transaccion
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Int32</tipo_dato>
        [Required]
        [JsonProperty("consecutivoRegistro")]
        [JsonPropertyOrder(6)]
        public Int32 ConsecutivoRegistro { get; set; }
    }
}
