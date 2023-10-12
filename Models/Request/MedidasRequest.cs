using HUSI_SIISA.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace HUSI_SIISA.Models.Request
{
    /// <summary>
    /// Contrado de Datos para la Medidas
    /// Oficina de Tecnologia
    /// Integracion y Arquitectura
    /// </summary>

    public class MedidasRequest
    {
        /// <summary>
        /// ID o Numero de la Consulta
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        [Required]
        [JsonProperty("idconsulta")]
        public Int32 IdConsulta {get; set;}

        /// <summary>
        /// Peso del paciente en Kilogramos
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>SmallInt</tipo_dato>
        [Required]
        [JsonProperty("peso")]
        public Int16 Peso {get; set;}

        /// <summary>
        /// Talla del paciente
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Smallint</tipo_dato>
        [Required]
        [JsonProperty("talla")]
        public Int16 Talla {get; set;}

        /// <summary>
        /// Indice de superficie Corporal
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>SmallInt</tipo_dato>
        [Required]
        [JsonProperty("indice_super_corporal")]
        public Int16 Indice_Super_Corporal {get; set;}

        /// <summary>
        /// Indice de Masa Corporal
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Smallint</tipo_dato>
        [Required]
        [JsonProperty("ind_masa_corporal")]
        public Int16 Ind_Masa_Corporal {get; set;}

        /// <summary>
        /// Frecuencia Cardiaca
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>SmallInt</tipo_dato>
        [Required]
        [JsonProperty("frecuencia_cardiaca")]
        public Int16 Frecuencia_Cardiaca {get; set;}

        /// <summary>
        /// Temperatura del paciente
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Smallint</tipo_dato>
        [Required]
        [JsonProperty("temperatura")]
        public Int16 Temperatura {get; set;}

        /// <summary>
        /// Presion Arteral Sistolica
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Smallint</tipo_dato>
        [Required]
        [JsonProperty("presion_art_sist")]
        public Int16 Presion_Art_Sist {get; set;}

        /// <summary>
        /// Presion Arterial Diastolica
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>SmallInt</tipo_dato>
        [Required]
        [JsonProperty("presion_art_dias")]
        public Int16 Presion_Art_Dias {get; set;}

        /// <summary>
        /// Frecuencia Respiratoria
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
        [Required]
        [JsonProperty("frec_respiratoria")]
        public Int16 Frec_Respiratoria {get; set;}

        /// <summary>
        /// Estado del paciente
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("estado")]
        public string? Estado {get; set;}

        /// <summary>
        /// Valor de Glucometria
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>SmalInt</tipo_dato>
        [Required]
        [JsonProperty("glucometria")]
        public Int16 Glucometria {get; set;}

        /// <summary>
        /// valor de Saturacion de Oxigeno
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>SmallInt</tipo_dato>
        [Required]
        [JsonProperty("saturacion_oxigeno")]
        public Int16 Saturacion_Oxigeno {get; set;}

        /// <summary>
        /// id del paciente
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>int</tipo_dato>
        [Required]
        [JsonProperty("idPaciente")]
        public string? IdPaciente { get; set; }

        /// <summary>
        /// Número de atención
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>string</tipo_dato>
        [Required]
        [JsonProperty("nroAtencion")]
        public string? NroAtencion { get; set; }

        /// <summary>
        /// Numero de consulta
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>string</tipo_dato>
        [Required]
        [JsonProperty("nroConsulta")]
        public string? NroConsulta { get; set; }

        /// <summary>
        /// Listado de Escalas
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Complejo - indices</tipo_dato>
        [Required]
        [JsonProperty("idSolicitudSAHICO")]
        public List<indices>? Escalas {get; set;}


    }

    /// <summary>
    /// Contrato de Datos Para Indices
    /// </summary>
    public class indices
    {
        /// <summary>
        /// Nombre de la Escala
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("nombre_escala")]
        public string? Nombre_Escala {get; set;}

        /// <summary>
        /// Valor de la Escala
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
        [Required]
        [JsonProperty("valor")]
        public string? Valor {get; set;}
    }
}
