using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace HUSI_SIISA.Models.Request
{
    /// <summary>
    /// Estructura para la Gestion de los Diagnosticos
    /// Oficina de Tecnologia
    /// Integracion y Arquitectura 
    /// </summary>
    public class DiagnosticosRequest
    {
        /// <summary>
        /// ID Consulta o Numero de la consulta
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>Integer</tipo_dato>

        [JsonProperty("idconsulta")]
        public Int32 IdConsulta {get; set;}

        /// <summary>
        /// ID del Paciente en SAHI. Se puede obtener del servicio Atencion
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
       
        [JsonProperty("idpaciente")]
        public Int32 IdPaciente {get; set;}

        /// <summary>
        /// ID Atencion del Paciente.
        /// </summary>
        /// <requerido>NO</requerido>
        /// <tipo_dato>Integer</tipo_dato>
   
        [JsonProperty("idatencion")]
        public Int32 IdAtencion {get; set;}

        /// <summary>
        /// FEcha en que se realiza el Diagnostico. formato AAAA-mm-DD HH:mm:ss
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
    
        [JsonProperty("fecha")]
        public DateTime Fecha {get; set;}

        /// <summary>
        /// Destino
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Integer</tipo_dato>
    
        [JsonProperty("dxDestino")]
        public Int32 DxDestino { get; set; }

        /// <summary>
        /// listado de Diagnosticos
        /// <requerido>SI</requerido>
        /// <tipo_dato>Complejo ItemDiagnostico</tipo_dato>
        /// </summary>
        [Required]
        [JsonProperty("items_dx")]
        public List<ItemDiagnostico>? Items_Dx {get; set;}
    }

    /// <summary>
    /// Modelo para la gestion de los diagnosticos.
    /// </summary>
    public class ItemDiagnostico
    {

        /// <summary>
        /// Codigo del Diagnostico CIE-10. 
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>string</tipo_dato>
        [Required]
        [JsonProperty("coddx")]
        public string? CodigoDx {get; set;}

        /// <summary>
        /// Nombre del diagnostico o Descripcion
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>string</tipo_dato>
        [Required]
        [JsonProperty("nombredx")]
        public string? NombreDx {get; set;}

        /// <summary>
        /// Tipo del diagnostico.
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>string</tipo_dato>
     
        [JsonProperty("tipo")]
        public string? Tipo {get; set;}

        /// <summary>
        /// Valor de confirmado del Diagnostico.
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Logico(true-false)</tipo_dato>
        [Required]
        [JsonProperty("confirmado")]
        public bool Confirmado {get; set;}

        /// <summary>
        /// Valores del TNM
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Complejo TNM</tipo_dato>
    
        [JsonProperty("tnmDx")]
        public TNM? TnmDx {get; set;}

        /// <summary>
        /// Informacion de la Resolucion 0247
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Complejo Res0247</tipo_dato>
      
        [JsonProperty("infresol0247")]
        public Res0247? InfResol_0247{get; set;}

    }

    /// <summary>
    /// Estructura para la Gestion del TNM
    /// </summary>
    public class TNM
    {
        /// <summary>
        /// Valor para la variable Estado.
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
     
        [JsonProperty("estado")]
        public string? Estado {get; set;}

        /// <summary>
        /// Valor para la variable:Tumor
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
 
        [JsonProperty("tumor")]
        public string? Tumor { get; set; }

        /// <summary>
        /// Valor para la variable: Nodulo
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
  
        [JsonProperty("nodulo")]
        public string? Nodulo {get; set;}

        /// <summary>
        /// Valor para la variable: Metastasis
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>String</tipo_dato>
     
        [JsonProperty("metastasis")]
        public string? Metastasis {get; set; }
    }

    /// <summary>
    /// Estructura Para la Gestion de la informacion de la Resolucion 0247
    /// </summary>
    public class Res0247
    {

        /// <summary>
        /// Valor, para la Fecha de Recoleccion de Muestra. Formato:AAAA-mm-DD HH:mm:ss
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>DateTime</tipo_dato>
     
        [JsonProperty("fec_rec_muestra")]
        public string Fec_Rec_Muestra {get; set; }

        /// <summary>
        ///  valor para la Fecha de Informe Histopatologico Valido
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>Datetime</tipo_dato>
      
        [JsonProperty("fec_inf_histo_val")]
        public string Fec_Inf_Histo_Val {get; set;}

        /// <summary>
        /// Valor, para No Hubo Diaqgnostico por Histopatologia
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>string</tipo_dato>

        [JsonProperty("nh_dx_x_histo")]
        private string? Nh_Dx_x_Histo {get; set;}

        /// <summary>
        /// Valor de Histologia.
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>string</tipo_dato>
      
        [JsonProperty("histologia")]
        public string? Histologia {get; set;}

        /// <summary>
        /// Valor para Grado de Diferenciacion
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>string</tipo_dato>
      
        [JsonProperty("grado_dif")]
        public string? Grado_Dif {get; set;}

        /// <summary>
        /// Valor para el Objetivo del Tratamiento inicial
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>string</tipo_dato>
      
        [JsonProperty("obj_trata_ini")]
        public string? Obj_Trata_Ini {get; set;}

        /// <summary>
        /// Valor para  el Objetivo Intervencion Medica
        /// </summary>
        /// <requerido>SI</requerido>
        /// <tipo_dato>string</tipo_dato>
   
        [JsonProperty("obj_interv_medica")]
        public string? Obj_Interv_Medica {get; set;}
    }
}
