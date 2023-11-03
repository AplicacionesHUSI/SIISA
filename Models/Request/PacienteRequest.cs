using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HUSI_SIISA.Models.Request
{
    /// <summary>
    /// Estructura de datos para la peticion del Servicio de Atenciones
    /// </summary>
    public class PacienteRequest
    {

        [Required]
        [JsonProperty("Documento")]
        public string? Documento { get; set; }
      
        [Required]
        [JsonProperty("Tipo")]
        public byte Tipo { get; set; }
    }
}
