using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HUSI_SIISA.Models.Request
{
    /// <summary>
    /// Estructura de datos para la peticion del Servicio de Esquemas
    /// </summary>
    public class EsquemaRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("atencion")]
        public string? Atencion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("orden")]
        public string? Orden { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("idEsquema")]
        public string? IdEsquema { get; set; }
    }
}
