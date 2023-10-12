using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HUSI_SIISA.Models.Response
{
    /// <summary>
    /// Estructura de datos para la respuesta del Servicio de Atenciones
    /// </summary>
    public class AtencionResponse
    {
        /// <summary>
        /// ID del Paciente en SAHi. Numero Unico de identificacion del paciente.
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        [Required]
        [JsonProperty("idcliente")]
        [JsonPropertyOrder(1)]
        public Int32 IdCliente { get; set; }

        /// <summary>
        /// Numero de Atencion en SAHI.
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        [Required]
        [JsonProperty("nroatencion")]
        [JsonPropertyOrder(2)]
        public Int32 NroAtencion { get; set; }

        /// <summary>
        /// Fecha de la Atencion.
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Datetime</tipo_dato>
        [Required]
        [JsonProperty("fechaatencion")]
        [JsonPropertyOrder(3)]
        public DateTime FechaAtencion { get; set; }

        /// <summary>
        /// Tipo de Atencion en SAHI
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        [Required]
        [JsonProperty("tipoatencion")]
        [JsonPropertyOrder(4)]
        public int TipoAtencion { get; set; }

        /// <summary>
        /// Nombre o Descripcion del Tipo de Atencion.
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("nombretipoatn")]
        [JsonPropertyOrder(5)]
        public string? NombreTipoAtn { get; set; }

        /// <summary>
        /// Tipo de Atencion Base. Definida en SAHI
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        [Required]
        [JsonProperty("tipobaseatencion")]
        [JsonPropertyOrder(6)]
        public int TipoBaseAtencion { get; set; }

        /// <summary>
        /// Nombre o Descripcion del tipo de Atencion Base
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("nomatnbase")]
        [JsonPropertyOrder(7)]
        public string? NomAtnBase { get; set; }

        /// <summary>
        /// Nombre del Paciente
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("nombrepaciente")]
        [JsonPropertyOrder(8)]
        public string? NombrePaciente { get; set; }

        /// <summary>
        /// Apellidos del Paciente
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("apellidospaciente")]
        [JsonPropertyOrder(9)]
        public string? ApellidosPaciente { get; set; }

        /// <summary>
        /// IdTercero
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>int</tipo_dato>
        [Required]
        [JsonProperty("IdTercero")]
        [JsonPropertyOrder(10)]
        public int IdTercero { get; set; }

        /// <summary>
        /// CodTercero
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Int</tipo_dato>
        [Required]
        [JsonProperty("CodTercero")]
        [JsonPropertyOrder(11)]
        public string? CodTercero { get; set; }

        /// <summary>
        /// NomTercero
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("NomTercero")]
        [JsonPropertyOrder(9)]
        public string? NomTercero { get; set; }
    }
}
