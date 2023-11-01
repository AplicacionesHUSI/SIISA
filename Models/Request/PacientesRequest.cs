using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HUSI_SIISA.Models.Request
{
    /// <summary>
    /// Estructura de datos para la peticion del Servicio de Atenciones
    /// </summary>
    public class PacientesRequest
    {

		public int IdCliente
		{ get; set; }
		public string NomCliente
		{ get; set; }

		public string ApeCliente
		{ get; set; }

		public byte IdTipoDoc
		{ get; set; }

		public string NumDocumento
		{ get; set; }

		public Byte IdSexo
		{ get; set; }

		public byte IdFactorRh
		{ get; set; }

		public byte IdGrupSangre
		{ get; set; }

		private DateTime fecNacimiento;
		public DateTime FecNacimiento
		{
			get { if (fecNacimiento > DateTime.Parse("1900-01-01")) { return fecNacimiento; } else { return DateTime.Now; } }
			set { if (value > DateTime.Parse("1900-01-01")) { fecNacimiento = value; } else { fecNacimiento = DateTime.Now; } }
		}

		public int IdLugarNac
		{ get; set; }

		public byte IdEstadoCivil
		{ get; set; }

		public int IdLugarCliente
		{ get; set; }

		public byte IdZona
		{ get; set; }

		public string DirCasa
		{ get; set; }

		public string DirOficina
		{ get; set; }

		public string TelCasa
		{ get; set; }

		public string TelOficina
		{ get; set; }

		public byte IdOcupacion
		{ get; set; }

		public int IdAfiliaciontipo
		{ get; set; }

		public byte IdNivel
		{ get; set; }

		public string NomPadre
		{ get; set; }

		public string NomMadre
		{ get; set; }

		public string NroHistoria
		{ get; set; }

		public double ValSaldo
		{ get; set; }

		public int IdReligion
		{ get; set; }

		public bool IndHabilitado
		{ get; set; }

		public int IdLugarDocu
		{ get; set; }

		public bool IndMuerto
		{ get; set; }

		public int IdClienteMadre
		{ get; set; }

		public string Cod_DipoNac
		{ get; set; }

		public string Cod_dipoCliente
		{ get; set; }

		public string Cod_usuario
		{ get; set; }

		public int idtercero
		{ get; set; }

		public string CorreoE
		{ get; set; }

		public string TelCelular
		{ get; set; }

		public bool indAuditoria
		{ get; set; }

		public DateTime CreateDate
		{ get; set; }

		public string Observaciones
		{ get; set; }

		public string autoEnvMsjCel
		{ get; set; }

		public string autoEnvMsjEmail
		{ get; set; }

		public string EpsAseg
		{ get; set; }
		public string Estado
		{ get; set; }

	}
}
