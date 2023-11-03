using HUSI_SIISA.DBContext;
using HUSI_SIISA.Models.Request;
using HUSI_SIISA.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NLog;
using System.Data;
using System.Net;

namespace HUSI_SIISA.Controllers
{
    /// <summary>
    /// Implememntacion del Servicio Atenciones. permite realizar varias operaciones con la entidad Atencion de SAHI.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PacientesController : ControllerBase
    {

        private static Logger logSahico = LogManager.GetCurrentClassLogger();
		
		public string PcteEsp
		{ get; set; }
        // POST: api/Pacientes/ConsultaPaciente
        /// <summary>
        /// Operacion ConsultaPaciente
        /// </summary>
        /// <param name="pacienteRequest">Estructura con los parametros para consumo del servicio</param>
        /// <returns>Estructura de datos para AtencionResponse</returns>
        /// <remarks>
        /// Sample request:
        ///     POST api/Pacientes/Consultar
        ///     {
        ///         "numDoc": "",
        ///         "tipoDoc": 0,
        ///         "servicio": 0
        ///     }
        /// </remarks>
        [HttpPost]
        [Route("ConsultaPaciente")]
        public ActionResult ConsultaPaciente(PacienteRequest ppr)
        {

			PacientesRequest pr = new PacientesRequest();
			try
			{
				DBConnection conn = new DBConnection();
				using (SqlConnection conexion = new SqlConnection(conn.getCs()))
				{
					conexion.Open();
					// String query0 = "SELECT IdCliente,NomCliente,ApeCliente,IdTipoDoc,NumDocumento,IdSexo,IdFactorRh,IdGrupSangre,FecNacimiento,IdLugarNac,IdEstadoCivil,IdLugarCliente,IdZona,DirCasa,DirOficina,TelCasa,TelOficina,IdOcupacion,IdAfiliaciontipo,IdNivel,NomPadre,NomMadre,NroHistoria,ValSaldo,IdReligion,IndHabilitado,IdLugarDocu,IndMuerto,IdClienteMadre,Cod_DipoNac,Cod_dipoCliente,Cod_usuario,idtercero,CorreoE,TelCelular,indAuditoria,CreateDate FROM admCliente WHERE NumDocumento='" + documento + "' AND idTipoDoc=" + tipo;
					string query0 = @"SELECT admCliente.IdCliente,NomCliente,ApeCliente,IdTipoDoc,NumDocumento,IdSexo,IdFactorRh,IdGrupSangre,FecNacimiento,IdLugarNac,IdEstadoCivil,IdLugarCliente,IdZona,DirCasa,DirOficina,TelCasa,TelOficina,IdOcupacion,IdAfiliaciontipo,IdNivel,NomPadre,NomMadre,NroHistoria,ValSaldo,IdReligion,IndHabilitado,IdLugarDocu,IndMuerto,IdClienteMadre,Cod_DipoNac,Cod_dipoCliente,Cod_usuario,idtercero,CorreoE,TelCelular,indAuditoria,CreateDate,CASE WHEN hdC.indActivo = 1 THEN '1' ELSE '0' END autoEnvMsjCel,CASE WHEN hdE.indActivo = 1 THEN '1' ELSE '0' END autoEnvMsjEmail,(SELECT dbo.fn_HCapsPacienteAsegurador(admCliente.IdCliente)) as EpsAseg,(SELECT dbo.fncHcePacienteEspecial  (admCliente.IdCliente)) as PcteEsp FROM admCliente
                                    LEFT JOIN (SELECT hd.idCliente, hd.indActivo
				                                    FROM citAutoEnvioMensajes hd
				                                    WHERE hd.fecRegistro = (SELECT MAX(hdi.fecRegistro)
										                                    FROM citAutoEnvioMensajes hdi
										                                    WHERE hdi.idCliente = hd.idCliente AND hdi.idTipo = 1) AND hd.idTipo = 1
										                                    ) hdC ON hdC.idCliente = admCliente.idCliente
                                    LEFT JOIN (SELECT hd.idCliente, hd.indActivo
			                                    FROM citAutoEnvioMensajes hd
			                                    WHERE hd.fecRegistro = (SELECT MAX(hdi.fecRegistro)
									                                    FROM citAutoEnvioMensajes hdi
									                                    WHERE hdi.idCliente = hd.idCliente AND hdi.idTipo = 0) AND hd.idTipo = 0
									                                    ) hdE ON hdE.idCliente = admCliente.idCliente
                                    WHERE admCliente.NumDocumento='" +ppr.Documento + "' AND admCliente.IdTipoDoc=" + ppr.Tipo;
					SqlCommand ComandoSql = new SqlCommand(query0, conexion);
					SqlDataReader Cliente = ComandoSql.ExecuteReader();
					if (Cliente.HasRows)
					{
						Cliente.Read();
						if (Cliente.IsDBNull(0)) { pr.IdCliente = 0; } else { pr.IdCliente = Cliente.GetInt32(0); }
						if (Cliente.IsDBNull(1)) { pr.NomCliente = ""; } else { pr.NomCliente = Cliente.GetString(1); }
						if (Cliente.IsDBNull(2)) { pr.ApeCliente = ""; } else { pr.ApeCliente = Cliente.GetString(2); }
						if (Cliente.IsDBNull(3)) { pr.IdTipoDoc = 0; } else { pr.IdTipoDoc = Cliente.GetByte(3); }
						if (Cliente.IsDBNull(4)) { pr.NumDocumento = ""; } else { pr.NumDocumento = Cliente.GetString(4); }
						if (Cliente.IsDBNull(5)) { pr.IdSexo = 0; } else { pr.IdSexo = (byte)Cliente.GetInt16(5); }
						if (Cliente.IsDBNull(6)) { pr.IdFactorRh = 0; } else { pr.IdFactorRh = (byte)(Cliente.GetInt16(6)); }
						if (Cliente.IsDBNull(7)) { pr.IdGrupSangre = 0; } else { pr.IdGrupSangre = (byte)(Cliente.GetInt16(7)); }
						if (Cliente.IsDBNull(8)) { } else { pr.FecNacimiento = Cliente.GetDateTime(8); }
						if (Cliente.IsDBNull(9)) { pr.IdLugarNac = 0; } else { pr.IdLugarNac = (int)Cliente.GetInt32(9); }
						if (Cliente.IsDBNull(10)) { } else { pr.IdEstadoCivil = Byte.Parse(Cliente.GetInt16(10).ToString()); }
						if (Cliente.IsDBNull(11)) { pr.IdLugarCliente = 0; } else { pr.IdLugarCliente = (int)Cliente.GetInt32(11); }
						if (Cliente.IsDBNull(12)) { pr.IdZona = 0; } else { pr.IdZona = Byte.Parse(Cliente.GetInt16(12).ToString()); }
						if (Cliente.IsDBNull(13)) { pr.DirCasa = ""; } else { pr.DirCasa = Cliente.GetString(13); }
						if (Cliente.IsDBNull(14)) { pr.DirOficina = ""; } else { pr.DirOficina = Cliente.GetString(14); }
						if (Cliente.IsDBNull(15)) { pr.TelCasa = ""; } else { pr.TelCasa = Cliente.GetString(15); }
						if (Cliente.IsDBNull(16)) { pr.TelOficina = ""; } else { pr.TelOficina = Cliente.GetString(16); }
						if (Cliente.IsDBNull(17)) { } else { pr.IdOcupacion = Byte.Parse(Cliente.GetInt16(17).ToString()); }
						if (Cliente.IsDBNull(18)) { pr.IdAfiliaciontipo = 0; } else { pr.IdAfiliaciontipo = (int)Cliente.GetInt16(18); }
						if (Cliente.IsDBNull(19)) { pr.IdNivel = 0; } else { pr.IdNivel = Byte.Parse(Cliente.GetByte(19).ToString()); }
						if (Cliente.IsDBNull(20)) { pr.NomPadre = ""; } else { pr.NomPadre = Cliente.GetString(20); }
						if (Cliente.IsDBNull(21)) { pr.NomMadre = ""; } else { pr.NomMadre = Cliente.GetString(21); }
						if (Cliente.IsDBNull(22)) { pr.NroHistoria = ""; } else { pr.NroHistoria = Cliente.GetString(22); }
						if (Cliente.IsDBNull(23)) { pr.ValSaldo = 0; } else { pr.ValSaldo = Cliente.GetDouble(23); }
						if (Cliente.IsDBNull(24)) { } else { pr.IdReligion = (byte)(Cliente.GetInt32(24)); }
						if (Cliente.IsDBNull(25)) { pr.IndHabilitado = false; } else { pr.IndHabilitado = Cliente.GetBoolean(25); }
						if (Cliente.IsDBNull(26)) { pr.IdLugarDocu = 0; } else { pr.IdLugarDocu = (int)Cliente.GetInt32(26); }
						if (Cliente.IsDBNull(27)) { pr.IndMuerto = false; } else { pr.IndMuerto = Cliente.GetBoolean(27); }
						if (Cliente.IsDBNull(28)) { pr.IdClienteMadre = 0; } else { pr.IdClienteMadre = (int)Cliente.GetInt32(28); }
						if (Cliente.IsDBNull(29)) { pr.Cod_DipoNac = ""; } else { pr.Cod_DipoNac = Cliente.GetString(29); }
						if (Cliente.IsDBNull(30)) { pr.Cod_dipoCliente = ""; } else { pr.Cod_dipoCliente = Cliente.GetString(30); }
						if (Cliente.IsDBNull(31)) { pr.Cod_usuario = ""; } else { pr.Cod_usuario = Cliente.GetString(31); }
						if (Cliente.IsDBNull(32)) { pr.idtercero = 0; } else { pr.idtercero = (int)Cliente.GetInt32(32); }
						if (Cliente.IsDBNull(33)) { pr.CorreoE = ""; } else { pr.CorreoE = Cliente.GetString(33); }
						if (Cliente.IsDBNull(34)) { pr.TelCelular = ""; } else { pr.TelCelular = Cliente.GetString(34); }
						if (Cliente.IsDBNull(35)) { pr.indAuditoria = false; } else { pr.indAuditoria = Cliente.GetBoolean(35); }
						if (Cliente.IsDBNull(36)) { } else { pr.CreateDate = Cliente.GetDateTime(36); }
						if (Cliente.IsDBNull(37)) { pr.autoEnvMsjCel = ""; } else { pr.autoEnvMsjCel = Cliente.GetString(37); }
						if (Cliente.IsDBNull(38)) { pr.autoEnvMsjEmail = ""; } else { pr.autoEnvMsjEmail = Cliente.GetString(38); }
						if (Cliente.IsDBNull(39)) { pr.EpsAseg = "0"; } else { pr.EpsAseg = Cliente.GetInt32(39).ToString(); }
						if (Cliente.IsDBNull(40)) { PcteEsp = "0"; } else { if (Cliente.GetBoolean(40)) { PcteEsp = "1"; } else { PcteEsp = "0"; } }
						pr.Estado = "01 | Proceso Exitoso";
						return Ok(pr);
					}
					else
					{
						pr.Estado = "03 |Paciente con Id=Documento:" + ppr.Documento + " No Existe";
						return NotFound(pr);
					}
				}
			}
			catch (SqlException ex1)
			{
				pr.Estado = "02|Falla SQL en la busqueda:" + ex1.Message;
				return NotFound(pr);
			}
		}




	}
}

