using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HUSI_SIISA.Models.Response
{
    /// <summary>
    /// Estructura de datos para la respuesta del Servicio de Atenciones
    /// </summary>
    public class AgendaResponse
    {

			public DateTime FechaCita { get; set; }
			public double CodigoCita { get; set; }
			public long IdMedico { get; set; }
			public string NombreMedico { get; set; }
			public string ApellidoMedico { get; set; }
			public long IdPaciente { get; set; }
			public string NombrePaciente { get; set; }
			public string ApellidoPaciente { get; set; }
			public string TipoDocumento { get; set; }
			public string NumDocumento { get; set; }
			public DateTime FecQuierePaciente { get; set; }
			public long IdServicio { get; set; }
			public string NombreServicio { get; set; }
			public string NombreCortoServicio { get; set; }
			public int PrimeraVezOControl { get; set; }
			public string IdAtencionTipo { get; set; }
			public string IdAsegurador { get; set; }
			public string NombreAsegurador { get; set; }
			public string IdConsultorio { get; set; }
			public string NombreConsultorio { get; set; }
			public string IdConvenio { get; set; }
			public string NombreConvenio { get; set; }
			public string EstadoCita { get; set; }
			public string EsExtra { get; set; }
			public string TieneServicioControl { get; set; }
			public string TieneMasControles { get; set; }
		
	}
}
