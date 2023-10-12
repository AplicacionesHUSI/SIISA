using HUSI_SIISA.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace HUSI_SIISA.Controllers
{
    /// <summary>
    /// Servicio profesionales.
    /// Oficina de Tecnologia de la Informacion
    /// Integracion y Arquitectura
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProfesionalesController
    {
        /// <summary>
        /// Operacion ActualizarProfesionales
        /// </summary>
        /// <param name="profesional">Modelo de Contrato(Profesional), poblado con la informacion del Profesional que se intenta actualizar</param>
        /// <returns>Valor Logico del Resultado de la Operacion</returns>

        [HttpPost]
        [Route("ActualizarProfesionales")]
        public bool ActualizarProfesionales([FromBody] Profesional profesional)
        {
            if (profesional.Id_profesional > 0 && profesional.Id_especialidad > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Operacion AdicionarProfesionales, para Crear o insertar profesionales en las composiciones.
        /// </summary>
        /// <param name="profesional">Modelo de Contrato(Profesional), poblado con la informacion del Profesional que se intenta Insertar</param>
        /// <returns>Valor Logico del Resultado de la Operacion</returns>

        [HttpPost]
        [Route("AdicionarProfesionales")]
        public bool AdicionarProfesionales([FromBody] Profesional profesional)
        {
            if (profesional.Id_profesional > 0 && profesional.Id_especialidad > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
