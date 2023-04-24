using Newtonsoft.Json;

namespace HUSI_SIISA.Utilities
{
    /// <summary>
    /// Estructura de datos para la lista de Conceptos
    /// </summary>
    public class ConsultaConceptos
    {
        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [JsonProperty("objetivo")]
        public string? Objetivo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [JsonProperty("subjetivo")]
        public string? Subjetivo { get; set; }
    }
}
