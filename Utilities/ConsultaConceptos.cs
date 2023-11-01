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
        [JsonProperty("titulo")]
        public string? Titulo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [JsonProperty("Cuerpo")]
        public string? Cuerpo { get; set; }
    }
}
