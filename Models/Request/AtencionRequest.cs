using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HUSI_SIISA.Models.Request
{
    /// <summary>
    /// Estructura de datos para la peticion del Servicio de Atenciones
    /// </summary>
    public class AtencionRequest
    {
        /// <summary>
        /// Numero de documento del Paciente
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("numDoc")]
        public string? NumDoc { get; set; }

        /// <summary>
        /// Tipo del Documento
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>short</tipo_dato>
        [Required]
        [JsonProperty("tipoDoc")]
        public short TipoDoc { get; set; }

        /// <summary>
        /// Tipo ddel servicio que esta solicitando el numero de atencion
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Int16</tipo_dato>
        [Required]
        [JsonProperty("servicio")]
        public Int16 Servicio { get; set; }
    }
}
