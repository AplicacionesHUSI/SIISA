using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HUSI_SIISA.Models.Response
{
    /// <summary>
    /// Estructura de datos para la respuesta del Servicio de Esquemas
    /// </summary>
    public class EsquemaResponse
    {
        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>Int32</tipo_dato>
        [JsonProperty("idAtencion")]
        [JsonPropertyOrder(1)]
        public Int32 IdAtencion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>Int32</tipo_dato>
        [JsonProperty("idEsquema")]
        [JsonPropertyOrder(2)]
        public Int32 IdEsquema { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>Int32</tipo_dato>
        [JsonProperty("idEsquemaDeAtencion")]
        [JsonPropertyOrder(3)]
        public Int32 IdEsquemaDeAtencion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>Int16</tipo_dato>
        [JsonProperty("idUbicacion")]
        [JsonPropertyOrder(4)]
        public Int16 IdUbicacion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>Int16</tipo_dato>
        [JsonProperty("idMedico")]
        [JsonPropertyOrder(5)]
        public Int16 IdMedico { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>Int32</tipo_dato>
        [JsonProperty("idTraslado")]
        [JsonPropertyOrder(6)]
        public Int32 IdTraslado { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>DateTime</tipo_dato>
        [JsonProperty("fecEsquema")]
        [JsonPropertyOrder(7)]
        public DateTime FecEsquema { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>Boolean</tipo_dato>
        [JsonProperty("indHabilitado")]
        [JsonPropertyOrder(8)]
        public Boolean IndHabilitado { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>Boolean</tipo_dato>
        [JsonProperty("indActivado")]
        [JsonPropertyOrder(9)]
        public Boolean IndActivado { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>DateTime</tipo_dato>
        [JsonProperty("fecCerrado")]
        [JsonPropertyOrder(10)]
        public DateTime FecCerrado { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>String</tipo_dato>
        [JsonProperty("interpretacion")]
        [JsonPropertyOrder(11)]
        public string? Interpretacion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>Int32</tipo_dato>
        [JsonProperty("idConsulta")]
        [JsonPropertyOrder(12)]
        public Int32 IdConsulta { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>Int32</tipo_dato>
        [JsonProperty("idUsuarioInter")]
        [JsonPropertyOrder(13)]
        public Int32 IdUsuarioInter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>Int32</tipo_dato>
        [JsonProperty("estadoApDx")]
        [JsonPropertyOrder(14)]
        public Int32 EstadoApDx { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>DateTime</tipo_dato>
        [JsonProperty("fecInterpretacion")]
        [JsonPropertyOrder(15)]
        public DateTime FecInterpretacion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>Int32</tipo_dato>
        [JsonProperty("idMovimiento")]
        [JsonPropertyOrder(16)]
        public Int32 IdMovimiento { get; set; }

        /// <summary>
        /// idorden
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>Int32</tipo_dato>
        [JsonProperty("idOrden")]
        [JsonPropertyOrder(17)]
        public Int32 IdOrden { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>Int16</tipo_dato>
        [JsonProperty("idEstudiante")]
        [JsonPropertyOrder(18)]
        public Int16 IdEstudiante { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>Int16</tipo_dato>
        [JsonProperty("idDocente")]
        [JsonPropertyOrder(19)]
        public Int16 IdDocente { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>String</tipo_dato>
        [JsonProperty("indEvaluado")]
        [JsonPropertyOrder(20)]
        public string? IndEvaluado { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>Boolean</tipo_dato>
        [JsonProperty("indEstudianteEnEval")]
        [JsonPropertyOrder(21)]
        public Boolean IndEstudianteEnEval { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>DateTime</tipo_dato>
        [JsonProperty("fecEval")]
        [JsonPropertyOrder(22)]
        public DateTime FecEval { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>Boolean</tipo_dato>
        [JsonProperty("indMigrado")]
        [JsonPropertyOrder(23)]
        public Boolean IndMigrado { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>Int16</tipo_dato>
        [JsonProperty("idInterno")]
        [JsonPropertyOrder(24)]
        public Int16 IdInterno { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>String</tipo_dato>
        [JsonProperty("indResCritico")]
        [JsonPropertyOrder(25)]
        public string? IndResCritico { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>DateTime</tipo_dato>
        [JsonProperty("fechaConducta")]
        [JsonPropertyOrder(26)]
        public DateTime FechaConducta { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>String</tipo_dato>
        [JsonProperty("conducta")]
        [JsonPropertyOrder(27)]
        public string? Conducta { get; set; }
    }
}
