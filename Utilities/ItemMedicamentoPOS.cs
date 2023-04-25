using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel.DataAnnotations;

namespace HUSI_SIISA.Utilities
{
    /// <summary>
	/// Estructura para los Medicamentos e Insumos
	/// </summary>
    public class ItemMedicamentoPOS
    {
        /// <summary>
		/// Id del Producto o Medicamento
		/// </summary>
		/// <tipo_dato>Integer</tipo_dato>
		/// <requerido>true</requerido>        
        [Required]
        [JsonProperty("id_Producto")]
        public Int32 Id_Producto { get; set; }

        /// <summary>
        /// Nombre del Producto o Medicamento
        /// </summary>
        /// <tipo_dato>string</tipo_dato>
        /// <requerido>true</requerido>        
        [Required]
        [JsonProperty("nombre_Medicamento")]
        public string? Nombre_Medicamento { get; set; }

        /// <summary>
        /// Tipo del Producto: medicamento o Insumo
        /// </summary>
        /// <tipo_dato>Integer</tipo_dato>
        /// <requerido>true</requerido>        
        [Required]
        [JsonProperty("tipo_Producto")]
        public string? Tipo_Producto { get; set; }

        /// <summary>
        /// Respuesta clinica Esperada.
        /// </summary>
        /// <tipo_dato>string</tipo_dato>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("resp_Clinica_Esp")]
        public string? Resp_Clinica_Esp { get; set; }

        /// <summary>
        /// Duracion del tratamiento
        /// </summary>
        /// <tipo_dato>string</tipo_dato>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("duracion")]
        public string? Duracion { get; set; }

        /// <summary>
        /// Cantidad formulada del medicamento.
        /// </summary>
        /// <tipo_dato>Integer</tipo_dato>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("cantidad")]
        public Int16 Cantidad { get; set; }

        /// <summary>
        /// Codigo de la Presentacion del medicamento formulado
        /// </summary>
        /// <tipo_dato>string</tipo_dato>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("codigoPresentacion")]
        public string? CodigoPresentacion { get; set; }

        /// <summary>
        /// Nombre o Descripcion de la presentacion
        /// </summary>
        /// <tipo_dato>string</tipo_dato>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("nombrePresentacion")]
        public string? NombrePresentacion { get; set; }

        /// <summary>
        /// Forma de Aplicaion del medicamento.
        /// </summary>
        /// <tipo_dato>string</tipo_dato>
        /// <requerido>true</requerido> 
        [Required]
        [JsonProperty("aplicacion")]
        public string? Aplicacion { get; set; }

        /// <summary>
        /// Formaulacion
        /// </summary>
        /// <tipo_dato>string</tipo_dato>
        /// <requerido>false</requerido>
        [Required]
        [JsonProperty("formulacion")]
        public string? Formulacion { get; set; }

        /// <summary>
        /// Observaciones adicionales para tener en cuenta.
        /// </summary>
        /// <tipo_dato>dtring</tipo_dato>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("observaciones")]
        public string? Observaciones { get; set; }
    }
}
