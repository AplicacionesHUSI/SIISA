using HUSI_SIISA.Utilities;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace HUSI_SIISA.Models.Request
{
    /// <summary>
    /// Modelo de Contrato de Datos Para Insumos
    /// </summary>
    public class InsumosRequest
    {
        /// <summary>
        /// Id o Numero de la Consulta
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        [Required]
        [JsonProperty("idconsulta")]
        public Int32 ID_Consulta {get; set;}

        /// <summary>
        /// Numero de Atencion en SAHI
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        [Required]
        [JsonProperty("atencion")]
        public Int32 Atencion {get; set;}

        /// <summary>
        /// Id del paciente en SAHI
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        [Required]
        [JsonProperty("idpaciente")]
        public Int32 ID_Paciente {get; set;}

        /// <summary>
        /// Fecha de la Consulta Asociada al Insumo
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Datetime formato (YYYY-mm-DD HH:mm:ss)</tipo_dato>
        [Required]
        [JsonProperty("fecha")]
        public DateTime Fecha {get; set;}

        /// <summary>
        /// Nombre del Esquema asociado.
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("esquema")]
        public string? Esquema {get; set;}

        /// <summary>
        /// Nombre del Servicio Asociado 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("servicio_solicitado")]
        public string? Servicio_Solicitado {get; set;}

        /// <summary>
        /// Numero del Ciclo
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>SmallInt</tipo_dato>
        [Required]
        [JsonProperty("numero_ciclo")]
        public Int16 Numero_Ciclo {get; set;}

        /// <summary>
        /// Listado de Insumos Requeridos
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Complejo - itemInsumo</tipo_dato>
        [Required]
        [JsonProperty("items_insumos")]
        public List<itemInsumo>? Items_Insumos {get; set;}

        /// <summary>
        /// Observaciones de la formula
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("observaciones")]
        public string? Observaciones {get; set;}

        /// <summary>
        /// Proffesionales Asociados o Relacionados con la Consulta
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Complejo - Profesional</tipo_dato>
        [Required]
        [JsonProperty("profesionales")]
        public List<Profesional>? Profesionales {get; set;}

    }

    /// <summary>
    /// Modelo de Contrato de Datos Para Item Insumo
    /// </summary>
    public class itemInsumo
    {

        /// <summary>
        /// ID o Codigo del Insumo
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        [Required]
        [JsonProperty("id_insumo")]
        public Int32 ID_Insumo {get; set;}

        /// <summary>
        /// Nombre o Descripcion del Insumo
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("nombre_insumo")]
        public string? Nombre_Insumo {get; set;}

        /// <summary>
        /// Cantidad por aplicacion
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>SmallInt</tipo_dato>
        [Required]
        [JsonProperty("cantdad_aplicacion")]
        public Int16 Cantdad_Aplicacion {get; set;}

        /// <summary>
        /// Numero de Aplicaciones
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>SmallInt</tipo_dato>
        [Required]
        [JsonProperty("numero_aplicaciones")]
        public Int16 Numero_Aplicaciones {get; set;}
    }
}
