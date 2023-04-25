using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HUSI_SIISA.Utilities
{
    /// <summary>
    /// Modelo de Datos para profesionales
    /// </summary>
    public class Profesional
    {
        /// <summary>
        /// ID del profesional. Corresponde al ID personal en SAHI
        /// </summary>
        /// <tipo_campo>Integer</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("id_profesional")]
        public Int32 Id_profesional { get; set; }

        /// <summary>
        /// Nombres del profesional
        /// </summary>
        /// <tipo_campo>String</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("nombres_profesional")]
        public string? Nombres_profesional { get; set; }

        /// <summary>
        /// Apellidos del profesional
        /// </summary>
        /// <tipo_campo>String</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("apellidos_profesional")]
        public string? Apellidos_profesional { get; set; }

        /// <summary>
        /// ID Especialidad. Corresponde con el idEspecialidad en SAHI
        /// </summary>
        /// <tipo_campo>Integer</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("id_especialidad")]
        public Int32 Id_especialidad { get; set; }

        /// <summary>
        /// Relacion del Profesional con la consulta.
        /// </summary>
        /// <tipo_campo>String</tipo_campo>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("relacion")]
        public string? Relacion { get; set; }
    }
}
