using System.ServiceModel;
using HUSI_SIISA.Models.Request;
using HUSI_SIISA.Models.Response;
using HUSI_SIISA.Utilities;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace HUSI_SIISA.Controllers
{
    /// <summary>
    /// Modelo de control servidio de insumos.
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class InsumosController : ControllerBase
    {
        private static Logger logSahico = LogManager.GetCurrentClassLogger();

        // POST: api/Insumos/ActualizarInsumos
        /// <summary>
        /// Operacion Actualización de Insumos
        /// </summary>
        /// <param name="insumosRequest">Estructura con los parametros para consumo del servicio</param>
        /// <returns>Estructura de datos para actualizacion de insumos</returns>
        [HttpPost]
        [Route("ActualizarInsumos")]
        public InsumosResponse ActualizarInsumos([FromBody] InsumosRequest insumosRequest)
        {

            InsumosResponse insumosResponse = validarEntrada(insumosRequest);

            try
            {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                if (insumosRequest.ID_Consulta > 0 && insumosRequest.ID_Paciente > 0 && insumosRequest.Atencion > 0 && insumosRequest.Items_Insumos.Count > 0)
                {
                    insumosResponse.Resultado = true;
                    insumosResponse.Mensaje = "Operacion de Actualizacion Exitosa";
                    insumosResponse.DetalleMensaje = "";
                    return insumosResponse;
                }
                else
                {
                    insumosResponse.Resultado = false;
                    insumosResponse.Mensaje = "Operacion de Actualizacion Fallida";
                    insumosResponse.DetalleMensaje = "La Informacion recibida en los argumentos presenta errores.";
                    return insumosResponse;
                }
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
            }
            catch (ServerTooBusyException serverTooBusyException)
            {
                insumosResponse.Resultado = false;
                insumosResponse.Mensaje = "Operacion Actualizacion Fallida";
                insumosResponse.DetalleMensaje = "No hay respuesta del servidor." + serverTooBusyException.StackTrace;
                return insumosResponse;
            }
            catch (ArgumentException argumentException)
            {
                insumosResponse.Resultado = false;
                insumosResponse.Mensaje = "Operacion Actualizacion Fallida";
                insumosResponse.DetalleMensaje = "Existen errores en los argumentos de Invocacion del Servicio." + argumentException.StackTrace;
                return insumosResponse;
            }
            catch (Exception exp1)
            {
                insumosResponse.Resultado = false;
                insumosResponse.Mensaje = "Operacion Actualizacion Fallida";
                insumosResponse.DetalleMensaje = "Se ha presentado una Excepcion de Tipo General:" + exp1.StackTrace;
                return insumosResponse;
            }
        }

        // POST: api/Insumos/ActualizarInsumos
        /// <summary>
        /// Operacion Actualización de Insumos
        /// </summary>
        /// <param name="insumosRequest">Estructura con los parametros para consumo del servicio</param>
        /// <returns>Estructura de datos para actualizacion de insumos</returns>
        [HttpPost]
        [Route("IngresarInsumos")]
        public InsumosResponse IngresarInsumos([FromBody] InsumosRequest insumosRequest)
        {

            var util = new Utilidades();
            logSahico.Info(util.GetXMLFromObject(insumosRequest));
            //entry validation 
            InsumosResponse rptaServicio = validarEntrada(insumosRequest);
            if (!rptaServicio.Resultado)
            {
                logSahico.Info($" No se puede procesar el mensaje porque falta informacion::	{rptaServicio.Mensaje}");
                return rptaServicio;
            }
            //end validation

            logSahico.Info("Ingreso de Insumos Recibido:Atencion:" + insumosRequest.Atencion + "  Esquema:" + insumosRequest.Esquema + "   idPaciente:" + insumosRequest.ID_Paciente + "   idConsulta:" + insumosRequest.ID_Consulta + "  Nro Ciclo:" + insumosRequest.Numero_Ciclo);// + " Items:" + insumosIng.Items_Insumos.ToArray().ToString() + "  Profesionales:" + insumosIng.Profesionales);
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
            logSahico.Info("Nro de Insumos: " + insumosRequest.Items_Insumos.Count);
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
            foreach (itemInsumo insumo in insumosRequest.Items_Insumos)
            {
                logSahico.Info("ID: " + insumo.ID_Insumo);
                logSahico.Info("Nombre: " + insumo.Nombre_Insumo);
                logSahico.Info("Nro aplicaciones: " + insumo.Numero_Aplicaciones);
                logSahico.Info("Cantidad Aplicacion: " + insumo.Cantdad_Aplicacion);
            }

            try
            {
                if (insumosRequest.ID_Consulta > 0 && insumosRequest.ID_Paciente > 0 && insumosRequest.Atencion > 0 && insumosRequest.Items_Insumos.Count > 0)
                {
                    rptaServicio.IdNotaSAHI = 1;
                    rptaServicio.Resultado = true;
                    rptaServicio.Mensaje = "Operacion Exitosa";
                    rptaServicio.DetalleMensaje = "";
                    return rptaServicio;
                }
                else
                {
                    rptaServicio.Resultado = false;
                    rptaServicio.Mensaje = "Operacion Insercion Fallida";
                    rptaServicio.DetalleMensaje = "La Informacion recibida en los argumentos presenta errores.";
                    return rptaServicio;
                }
            }
            catch (ServerTooBusyException srvEx1)
            {
                rptaServicio.Resultado = false;
                rptaServicio.Mensaje = "Operacion Insercion  Fallida";
                rptaServicio.DetalleMensaje = "No hay respuesta del servidor." + srvEx1.StackTrace;
                return rptaServicio;
            }
            catch (ArgumentException argExp)
            {
                rptaServicio.Resultado = false;
                rptaServicio.Mensaje = "Operacion Insercion Fallida";
                rptaServicio.DetalleMensaje = "Existen errores en los argumentos de Invocacion del Servicio." + argExp.StackTrace;
                return rptaServicio;
            }
            catch (Exception exp1)
            {
                rptaServicio.Resultado = false;
                rptaServicio.Mensaje = "Operacion Insercion  Fallida";
                rptaServicio.DetalleMensaje = "Se ha presentado una Excepcion de Tipo General:" + exp1.StackTrace;
                return rptaServicio;
            }
        }
        private InsumosResponse validarEntrada(InsumosRequest insumosRequest)
        {
            var response = new InsumosResponse
            {
                Resultado = true,
                IdNotaSAHI = 0
            };
            var msg = "Se ha presentado una Excepcion: El siguiente campo es vacio";
            if (insumosRequest.ID_Paciente == 0)
            {
                response.Resultado = false;
                msg += ", idPaciente";
            }
            else response.IdPaciente = insumosRequest.ID_Paciente;
            if (insumosRequest.ID_Consulta == 0)
            {
                response.Resultado = false;
                msg += ", idconsulta";
            }
            else response.ConsultaSahico = insumosRequest.ID_Consulta;
            if (insumosRequest.Atencion == 0)
            {
                response.Resultado = false;
                msg += ", idAtencion";
            }
            else response.Atencion = insumosRequest.Atencion;
            response.Mensaje = msg;
            return response;
        }

    }
}
