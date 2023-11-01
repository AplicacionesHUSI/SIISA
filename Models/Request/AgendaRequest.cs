using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HUSI_SIISA.Models.Request
{
    /// <summary>
    /// Estructura de datos para la peticion del Servicio de Atenciones
    /// </summary>
    public class AgendaRequest
    {

        [Required]
        [JsonProperty("idMedico")]
        public string? idMedico { get; set; }

        [Required]
        [JsonProperty("idPaciente")]
        public string? idPaciente { get; set; }
        [Required]
        [JsonProperty("FechaIni")]
        public string? FechaIni { get; set; }
        [Required]
        [JsonProperty("FechaFin")]
        public string? FechaFin { get; set; }

        [Required]
        [JsonProperty("EstadoCita")]
        public string? EstadoCita { get; set; }
      
    }
}
