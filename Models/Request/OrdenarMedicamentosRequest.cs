using HUSI_SIISA.Utilities;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HUSI_SIISA.Models.Request
{
    /// <summary>
	/// Modelo General de datos para medicamentos.
	/// </summary>
    public class OrdenarMedicamentosRequest
    {
        /// <summary>
		/// IdConsulta Campo para el ingreso del ID o Numero de Consulta
		/// </summary>
		/// <tipo_dato>Integer</tipo_dato>
		/// <requerido>true</requerido>
        [Required]
        [JsonProperty("idConsulta")]
        public Int32 IdConsulta { get; set; }

        /// <summary>
        /// Atencion: Corresponde al numero de atencion del cliente en SAHI.
        /// </summary>
        /// <tipo_dato>Integer</tipo_dato>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("atencion")]
        public Int32 Atencion { get; set; }

        /// <summary>
        /// ID del paciente en SAHI.
        /// </summary>
        /// <tipo_dato>Integer</tipo_dato>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("idpaciente")]
        public Int32 IdPaciente { get; set; }

        /// <summary>
        /// ID del paciente en SAHI.
        /// </summary>
        /// <tipo_dato>Integer</tipo_dato>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("idMedico")]
        public Int32 IdMedico { get; set; }

        /// <summary>
        /// Fecha de la orden de medicamentos formato:AAAA-mm-DD HH:mm:ss
        /// </summary>
        /// <tipo_dato>Datetime</tipo_dato>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("fecha")]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Lista de Medicamentos Ordenados.
        /// </summary>
        /// <tipo_dato>Complejo-ItemMedicamentoPOS</tipo_dato>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("items_Medicamentos")]
        public List<ItemMedicamentoPOS>? Items_Medicamentos { get; set; }

        /// <summary>
        /// Lista de medicamentos No POS Formulados
        /// </summary>
        /// <tipo_dato>Complejo-ItemMedicamentoNPos</tipo_dato>
        /// <requerido>true</requerido>
        [Required]
        [JsonProperty("items_med_NPos")]
        public List<ItemMedicamentoNoPOS>? Items_med_NPos { get; set; }
    }
}
