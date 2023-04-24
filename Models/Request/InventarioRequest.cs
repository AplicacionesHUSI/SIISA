using HUSI_SIISA.Utilities;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HUSI_SIISA.Models.Request
{
    /// <summary>
    /// Estructura de datos para la peticion del Servicio de Inventario
    /// </summary>
    public class InventarioRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Int32</tipo_dato>
        [Required]
        [JsonProperty("idSolicitudSAHICO")]
        public Int32 IdSolicitudSAHICO { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Int32</tipo_dato>
        [Required]
        [JsonProperty("idDevolucionSAHICO")]
        public Int32 IdDevolucionSAHICO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>int</tipo_dato>
        [Required]
        [JsonProperty("idAtencion")]
        public int IdAtencion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("numDocumento")]
        public string? NumDocumento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("fecRegistro")]
        public string? FecRegistro { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>int</tipo_dato>
        [Required]
        [JsonProperty("idTipoSolicitud")]
        public int IdTipoSolicitud { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>int</tipo_dato>
        [Required]
        [JsonProperty("idTipoProducto")]
        public int IdTipoProducto { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Int</tipo_dato>
        [Required]
        [JsonProperty("idUbicacionRec")]
        public int IdUbicacionRec { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Int</tipo_dato>
        [Required]
        [JsonProperty("idUbicacionEnt")]
        public int IdUbicacionEnt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Int</tipo_dato>
        [Required]
        [JsonProperty("idPersonal")]
        public int IdPersonal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Int</tipo_dato>
        [Required]
        [JsonProperty("idCausa")]
        public int IdCausa { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("otraCausa")]
        public string? OtraCausa { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Lista Productos</tipo_dato>
        [Required]
        [JsonProperty("productos")]
        public List<Producto>? Productos { get; set; }
    }
}
