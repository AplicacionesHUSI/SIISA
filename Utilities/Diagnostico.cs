using HUSI_SIISA.Models.Request;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace HUSI_SIISA.Utilities
{
    /// <summary>
    /// Estructura para la Gestion de los Diagnosticos
    /// Oficina de Tecnologia
    /// Integracion y Arquitectura 
    /// </summary>
    public class Diagnostico
    {
        /// <summary>
        /// ID Consulta o Numero de la consulta
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        [Required]
        [JsonProperty("idconsulta")]
        public Int32 IdConsulta {get; set;}

        /// <summary>
        /// ID del Paciente en SAHI. Se puede obtener del servicio Atencion
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        [Required]
        [JsonProperty("idpaciente")]
        public Int32 IdPaciente {get; set;}

        /// <summary>
        /// ID Atencion del Paciente.
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        [Required]
        [JsonProperty("idatencion")]
        public Int32 IdAtencion {get; set;}

        /// <summary>
        /// FEcha en que se realiza el Diagnostico. formato AAAA-mm-DD HH:mm:ss
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        [Required]
        [JsonProperty("fecha")]
        public DateTime Fecha {get; set;}

        /// <summary>
        /// listado de Diagnosticos
        /// <requerido>SI</requerido>
        /// <tipo_dato>Complejo ItemDiagnostico</tipo_dato>
        /// </summary>
        [Required]
        [JsonProperty("items_dx")]
        public List<ItemDiagnostico>? Items_Dx {get; set;}
    }
}
