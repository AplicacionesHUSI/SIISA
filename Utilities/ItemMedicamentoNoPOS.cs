using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HUSI_SIISA.Utilities
{
    /// <summary>
	/// Modelo de Datos para los Medicamentos No POS
	/// </summary>
    public class ItemMedicamentoNoPOS
    {
        /// <summary>
		/// ID del producto. Equivalente a idProductoSAHICO en SAHI.
		/// </summary>
		/// <tipo_dato>Integer</tipo_dato>
		/// <requerido>true</requerido>
        [Required]
        [JsonProperty("id_Producto")]
        public int Id_Producto { get; set; }

        /// <summary>
        /// Nombre o Descripcion del producto.
        /// </summary>
        /// <tipo_dato>String</tipo_dato>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("nombre_Medicamento")]
        public string? Nombre_Medicamento { get; set; }

        /// <summary>
        /// Codigo CUM del medicamento o Producto
        /// </summary>
        /// <tipo_dato>String</tipo_dato>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("codigo_Cum")]
        public string? Codigo_Cum { get; set; }

        /// <summary>
        /// Registro Sanitario del medicamento.
        /// </summary>
        /// <tipo_dato>String</tipo_dato>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("registro_Sanitario")]
        public string? Registro_Sanitario { get; set; }

        /// <summary>
        /// numero mi press autorizacion medicamento no pos
        /// </summary>
        [Required]
        [JsonProperty("num_MiPres")]
        public string? Num_MiPres { get; set; }

        /// <summary>
        /// Formulacion del medicamento.
        /// </summary>
        /// <tipo_dato>Complejo FormulacionStruct</tipo_dato>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("formulacion")]
        public FormulacionStruct Formulacion { get; set; }

        /// <summary>
        /// Cantidad por ciclo.
        /// </summary>
        /// <tipo_dato>smallint</tipo_dato>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("cantidad_Ciclo")]
        public short Cantidad_Ciclo { get; set; }

        /// <summary>
        /// Cantidad para el tratamiento.
        /// </summary>
        /// <tipo_dato>Smallint</tipo_dato>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("cantidad_Tratamiento")]
        public short Cantidad_Tratamiento { get; set; }

        /// <summary>
        /// Duracion del tratamiento.
        /// </summary>
        /// <tipo_dato>Cantidad</tipo_dato>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("duracion_Tratamiento")]
        public string? Duracion_Tratamiento { get; set; }

        /// <summary>
        /// Fecha de Vencimiento de la Justificacion
        /// </summary>
        /// <tipo_dato>Datetime</tipo_dato>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("vencimiento_Justificacion")]
        public DateTime Vencimiento_Justificacion { get; set; }

        /// <summary>
        /// Tipo del Medicamento o Producto.
        /// </summary>
        /// <tipo_dato>String</tipo_dato>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("tipo_Medicamento")]
        public string? Tipo_Medicamento { get; set; }

        /// <summary>
        /// Informacion de la aplicacion del producto o medicamento
        /// </summary>
        /// <tipo_dato>String</tipo_dato>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("aplicacion")]
        public string? Aplicacion { get; set; }
    }

    /// <summary>
    /// Estructura Generica para la formulacion
    /// </summary>
    public struct FormulacionStruct
    {
        /// <summary>
        /// Campo Uno de La formulacion
        /// </summary>
        /// <tipo_campo>String</tipo_campo>
        /// <requerido>SI</requerido>
        public string Campo1;

        /// <summary>
        /// Campo Dos de la formulacion
        /// </summary>
        /// <tipo_campo>String</tipo_campo>
        /// <requerido>SI</requerido>
        public string Campo2;

        /// <summary>
        /// Campo Tres de la Formulacion
        /// </summary>
        /// <tipo_campo>String</tipo_campo>
        /// <requerido>SI</requerido>
        public string Campo3;

        /// <summary>
        /// Campo Cuatro de la Formulacion
        /// </summary>
        /// <tipo_campo>String</tipo_campo>
        /// <requerido>SI</requerido>
        public string Campo4;
    }
}
