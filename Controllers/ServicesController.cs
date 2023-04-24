using HUSI_SIISA.DBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace HUSI_SIISA.Controllers
{
    // CONTROLLER: Services
    /// <summary>
    /// controlador de services ntrolaObtiene un contador de clientes.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ServicesController : ControllerBase
    {
        // GET: api/Services/Get
        /// <summary>
        /// Obtiene un contador de clientes.
        /// </summary>
        /// <remarks>
        /// Aquí una descripción mas larga si fuera necesario. Obtiene un objeto por su Id.
        /// </remarks>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        [HttpGet(Name = "GetConnection")]
        public Int32 Get()
        {
            DBConnection conn = new DBConnection();
            using (SqlConnection conexion = new SqlConnection(conn.getCs()))
            {
                conexion.Open();
                string strConsultar = string.Empty;
                strConsultar = @"SELECT count(A.idCliente) NumClientes FROM admAtencion A";
                SqlCommand cmdConsultar = new SqlCommand(strConsultar, conexion);
                SqlDataReader rdConsultar = cmdConsultar.ExecuteReader();
                rdConsultar.Read();
                return rdConsultar.GetInt32(0);
            }
        }
    }
}
