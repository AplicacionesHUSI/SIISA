using HUSI_SIISA.Utilities;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HUSI_SIISA.Models.Request
{
    /// <summary>
    /// Estructura de datos para la peticion del Servicio de Notas Enfermeria
    /// </summary>
    public class NotasEnfermeriaRequest
    {
        /// <summary>
        /// ID o Numero de Nota
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        [Required]
        [JsonProperty("idnota")]
        public Int32 IdNota { get; set; }

        /// <summary>
        /// ID o Numero de consulta
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        [Required]
        [JsonProperty("idConsulta")]
        public Int32 IdConsulta { get; set; }

        /// <summary>
        /// ID o identificador del Paciente en SAHI
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        [Required]
        [JsonProperty("idPaciente")]
        public Int32 IdPaciente { get; set; }

        /// <summary>
        /// ID o Numero de Atencion en SAHI
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        [Required]
        [JsonProperty("idAtencion")]
        public Int32 IdAtencion { get; set; }

        /// <summary>
        /// Consecutivo de Quimioterapia
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        [Required]
        [JsonProperty("consecutivo_qtx")]
        public Int32 Consecutivo_qtx { get; set; }

        /// <summary>
        /// Dias de aplicacion
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Smallint</tipo_dato>
        [Required]
        [JsonProperty("dias_aplicacion")]
        public Int16 Dias_aplicacion { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("estado")]
        public string? Estado { get; set; }

        /// <summary>
        /// Listado de profesionales
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Complejo-Profesionales</tipo_dato>
        [Required]
        [JsonProperty("profesionales")]
        public List<Profesional>? Profesionales { get; set; }

        /// <summary>
        /// FEcha de la Nota de Enfermeria
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Datetime formato YYYY-mm-DD HH:mm:ss</tipo_dato>
        [Required]
        [JsonProperty("fechaNota")]
        public DateTime FechaNota { get; set; }

        /// <summary>
        /// Observaciones Generales de la Nota de enfermeria
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("observaciones")]
        public string? Observaciones { get; set; }

        /// <summary>
        /// Nota de Quimioterapia. Contenido de la Nota que desea Ingresar la Enfermera
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("notaQx")]
        public string? NotaQx { get; set; }

        /// <summary>
        /// Numero de la Nota Destino
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>Int</tipo_dato>
        [JsonProperty("nroNotadestino")]
        public int NroNotadestino { get; set; }
    }
}
