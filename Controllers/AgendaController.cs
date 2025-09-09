using Azure;
using HUSI_SIISA.DBContext;
using HUSI_SIISA.Models.Request;
using HUSI_SIISA.Models.Response;
using HUSI_SIISA.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using NLog;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace HUSI_SIISA.Controllers
{
    /// <summary>
    /// Implememntacion del Servicio Atenciones. permite realizar varias operaciones con la entidad Atencion de SAHI.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AgendaController : ControllerBase
    {
        private static Logger logSahico = LogManager.GetCurrentClassLogger();
        private static HttpClient _httpClient;

        // Inyección de dependencias mediante el constructor
        public AgendaController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        // POST: api/Agenda/ConsultarAgenda
        /// <summary>
        /// Operacion ConsultarAgenda
        /// </summary>
        /// <param name="agendaRequest">Estructura con los parametros para consumo del servicio</param>
        /// <returns>Estructura de datos para AgendaResponse</returns>
        /// <remarks>
        /// Sample request:
        ///     POST api/Agenda/ConsultarAgenda
        ///     {
        ///         "idMedico": "",
        ///         "idPaciente": 0,
        ///         "fechaIni": 0
        ///         "fechaFin": 0
        ///         "Estado": 0
        ///     }
        /// </remarks>
        [HttpPost]
        [Route("ConsultarAgenda")]
        public ActionResult ConsultarAgenda([FromBody] AgendaRequest agendaRequest)
        {

            logSahico.Info("ConsultarAgenda");
            try
            {
                List<AgendaResponse> lar = new List<AgendaResponse>();

                // Obtener el DoctorID desde la base de datos usando el idMedico de la solicitud
                var doctorId = ObtenerDoctorId(agendaRequest.idMedico);

                // Verificar si se obtuvo el DoctorID
                if (doctorId == null)
                {
                    // Si no se encontró el DoctorID, devolvemos un error
                    return BadRequest(new { mensaje = "No se encontró el DoctorID para el idMedico proporcionado." });
                }

                // Llamamos al nuevo método de la API externa para obtener las citas
                var citasDD = ObtenerCitasDDAsync(doctorId.Value, agendaRequest).Result;

                // Verificamos si la API externa devolvió citas
                if (citasDD != null && citasDD.Any())
                {
                    // Adaptamos las citas de la API externa al formato que espera el consumidor
                    foreach (var citaDD in citasDD)
                    {
                        // Separar el nombre completo en NombreMedico y ApellidoMedico
                        string nombreCompletoMedico = citaDD.nombreMedico;
                        string nombreMedico = string.Empty;
                        string apellidoMedico = string.Empty;
                        if (string.IsNullOrWhiteSpace(nombreCompletoMedico))
                        {
                            // Si el nombre está vacío, asignamos valores por defecto
                            nombreCompletoMedico = string.Empty;
                        }
                        else
                        {
                            // Llamamos al método para separar nombre y apellido
                            SepararNombreApellido(nombreCompletoMedico);

                            // Luego asignamos los valores de nombre y apellido
                            string[] nombreYApellidoDoc = nombreCompletoMedico.Split(' ');
                            nombreMedico = nombreYApellidoDoc[0];
                            apellidoMedico = nombreYApellidoDoc.Length > 1 ? string.Join(" ", nombreYApellidoDoc.Skip(1)) : "";
                        }

                        // Separar el nombre completo en NombreMedico y ApellidoMedico
                        string nombreCompletoPaciente = citaDD.nombrePaciente;
                        string nombrePaciente = string.Empty;
                        string apellidoPaciente = string.Empty;
                        if (string.IsNullOrWhiteSpace(nombreCompletoPaciente))
                        {
                            // Si el nombre está vacío, asignamos valores por defecto
                            nombreCompletoPaciente = string.Empty;
                        }
                        else
                        {
                            // Llamamos al método para separar nombre y apellido
                            SepararNombreApellido(nombreCompletoPaciente);

                            // Luego asignamos los valores de nombre y apellido
                            string[] nombreYApellidoPac = nombreCompletoPaciente.Split(' ');
                            nombrePaciente = nombreYApellidoPac[0];
                            apellidoPaciente = nombreYApellidoPac.Length > 1 ? string.Join(" ", nombreYApellidoPac.Skip(1)) : "";
                        }

                        // Obtener el IdAsegurador utilizando la consulta
                        int? idAsegurador = ObtenerIdAsegurador(citaDD.planAseguradora);

                        // Obtener el NombreAsegurador usando el IdAsegurador
                        string nombreAsegurador = string.Empty;
                        if (idAsegurador.HasValue)
                        {
                            nombreAsegurador = ObtenerNombreAsegurador(idAsegurador.Value);
                        }

                        AgendaResponse agendaResponse = new AgendaResponse
                        {
                            FechaCita = citaDD.fechaCita,
                            CodigoCita = Convert.ToInt64(citaDD.appointmentID),
                            IdMedico = Convert.ToInt64(agendaRequest.idMedico),
                            NombreMedico = nombreMedico,
                            ApellidoMedico = apellidoMedico,
                            IdPaciente = citaDD.pacienteRemoteID.HasValue ? citaDD.pacienteRemoteID.Value : 0,//citaDD.pacienteRemoteID!.Value,
                            NombrePaciente = nombrePaciente,
                            ApellidoPaciente = apellidoPaciente,
                            TipoDocumento = citaDD.tipoDocumentoPaciente,
                            NumDocumento = citaDD.documentoPaciente,
                            FecQuierePaciente = citaDD.fechaCreacion,
                            IdServicio = Convert.ToInt64(citaDD.servicioID) ,
                            NombreServicio = citaDD.nombreServicio,
                            NombreCortoServicio = citaDD.nombreServicio,
                            PrimeraVezOControl = citaDD.nombreServicio?.ToLower().Contains("control") == true ? 1 : 0,
                            IdAtencionTipo = citaDD.tipoServicioRemoteID,
                            IdAsegurador = idAsegurador?.ToString() ?? string.Empty,
                            NombreAsegurador = nombreAsegurador, // Asignar el NombreAsegurador
                            IdConsultorio = citaDD.consultorioRemoteID,
                            NombreConsultorio = citaDD.consultorio,
                            IdConvenio = citaDD.planAseguradora,
                            NombreConvenio = citaDD.nombrePlanAseguradora,
                            EstadoCita = "01",//Don Doctor nos envian solo citas en estado A = Activas, se devuelve 01, por que es lo que espera SIISA.
                            EsExtra = citaDD.citaExtra!.Value ? "S" : "N",
                            TieneServicioControl = "NO",
                            TieneMasControles = "N",
                            RutasCuidado = citaDD.agendadorAlterno
                        };
                        lar.Add(agendaResponse);
                    }
                }

                // Si no se encontraron citas ni en la base de datos ni en la API externa
                if (!lar.Any())
                {
                    AgendaResponse agendaResponse = new AgendaResponse
                    {
                        FechaCita = DateTime.Now,
                        CodigoCita = 0,
                        IdMedico = 0,
                        NombreMedico = "Agenda no encontrada",
                        ApellidoMedico = "Agenda no encontrada",
                        IdPaciente = 0,
                        NombrePaciente = "Agenda no encontrada",
                        ApellidoPaciente = "Agenda no encontrada",
                        TipoDocumento = "",
                        NumDocumento = "",
                        FecQuierePaciente = DateTime.Now,
                        IdServicio = 0,
                        NombreServicio = "Agenda no encontrada",
                        NombreCortoServicio = "",
                        PrimeraVezOControl = 0,
                        IdAtencionTipo = "",
                        IdAsegurador = "",
                        NombreAsegurador = "",
                        IdConsultorio = "",
                        NombreConsultorio = "Agenda no encontrada",
                        IdConvenio = "",
                        NombreConvenio = "",
                        EstadoCita = "",
                        EsExtra = "",
                        TieneServicioControl = "",
                        TieneMasControles = ""
                    };

                    return NotFound(agendaResponse); // Si no hay citas, respondemos con NotFound
                }

                return Ok(lar); // Si hay citas, devolvemos la lista
            }
            catch (SqlException ex)
            {
                ErrorResponse errorResponse = new();
                logSahico.Error("Error en Base de Datos :: " + ex.Message);
                errorResponse.Codigo = 500;
                errorResponse.Mensaje = "Error con la operacion en la base de datos, comuníquese con el administrador.";
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
            catch (Exception ex3)
            {
                ErrorResponse errorResponse = new();
                logSahico.Error("Error en Base de Datos :: " + ex3.Message);
                logSahico.Info("Se ha presentado una Excepcion:" + ex3.InnerException);
                logSahico.Info("Se ha presentado una Excepcion:" + ex3.StackTrace);
                errorResponse.Codigo = 500;
                errorResponse.Mensaje = "Se ha presentado una Excepcion General No controlada, comuníquese con el administrador.";
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        private int? ObtenerDoctorId(string idMedico)
        {
            DBConnection conn = new DBConnection();
            string query = "SELECT DoctorID FROM hcePersonal WHERE IdPersonal = @idMedico";

            try
            {
                using (var connection = new SqlConnection(conn.getCs()))
                {
                    connection.Open(); // Abre la conexión sincrónicamente
                    using (var command = new SqlCommand(query, connection))
                    {
                        // Agregar el parámetro para evitar SQL injection
                        command.Parameters.AddWithValue("@idMedico", idMedico);

                        // Ejecutamos la consulta y obtenemos el resultado sincrónicamente
                        var result = command.ExecuteScalar();

                        // Si el resultado es DBNull, retornamos null
                        return result != DBNull.Value ? Convert.ToInt32(result) : (int?)null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error al obtener el DoctorID: {ex.Message}");
                return null;
            }
        }

        private async Task<List<CitasDD>> ObtenerCitasDDAsync(int doctorId, AgendaRequest agendaRequest)
        {
            logSahico.Info($"ObtenerCitasDDAsync ...");
            try
            {
                // Agregar el header de autorización o cualquier otro header
                _httpClient.DefaultRequestHeaders.Clear();
                //prod
                //_httpClient.DefaultRequestHeaders.Add("clientSha", "3eac46aafcf9ced2c2330d3c10830276eb09af4882fd6eb5a69cf4889428c962");
                //pru
                _httpClient.DefaultRequestHeaders.Add("clientSha", "6a20204c9e1eadc661f35a15c85785e87bd8a44e88630d5c8cb1b221514db544");

                // Definir las fechas como variables de tipo string con el formato yyyy-MM-dd
                string startDate = Convert.ToDateTime(agendaRequest.FechaIni).ToString("yyyy-MM-dd");
                string endDate = Convert.ToDateTime(agendaRequest.FechaFin).ToString("yyyy-MM-dd");

                // Hacer la solicitud GET con las fechas parametrizables
                //prod
                //logSahico.Info($"Solicitud: https://integration-sahi-prod-001.azurewebsites.net/Appointment/List?DoctorID={doctorId}&startDate={startDate}&endDate={endDate}&state=A");
                //var response = await _httpClient.GetAsync($"https://integration-sahi-prod-001.azurewebsites.net/Appointment/List?DoctorID={doctorId}&startDate={startDate}&endDate={endDate}&state=A");

                //pruebas
                //logSahico.Info($"Solicitud: https://integration-sahi-test-001.azurewebsites.net/Appointment/List?DoctorID={doctorId}&startDate={startDate}&endDate={endDate}&state=A");
                var response = await _httpClient.GetAsync($"https://integration-sahi-test-001.azurewebsites.net/Appointment/List?DoctorID={doctorId}&startDate={startDate}&endDate={endDate}&state=A");


                // Registrar el código de estado de la respuesta
                logSahico.Info($"Código de respuesta HTTP: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    // Si la respuesta no es exitosa, registrar más detalles
                    var errorContent = await response.Content.ReadAsStringAsync();
                    logSahico.Info($"Error en la respuesta: {errorContent}");
                }

                // Asegurar que la respuesta sea exitosa
                response.EnsureSuccessStatusCode(); // Esto lanzará una excepción si el código de estado no es 2xx

                // Leer el contenido de la respuesta como un string
                var json = await response.Content.ReadAsStringAsync();

                // Deserializar el JSON en una lista de objetos CitasDD
                var citas = JsonSerializer.Deserialize<List<CitasDD>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Esto permite que se ignore la diferencia de mayúsculas y minúsculas
                }) ?? new List<CitasDD>();

                var logRes = Newtonsoft.Json.JsonConvert.SerializeObject(citas);
                logSahico.Info($"Citas {logRes}");
                return citas;
            }
            catch (HttpRequestException httpEx)
            {
                logSahico.Error($"Error de solicitud HTTP: {httpEx.Message}");
                logSahico.Info($"Detalles del error: {httpEx.StackTrace}");
                return new List<CitasDD>();
            }
            catch (Exception ex)
            {
                logSahico.Error($"Error inesperado al obtener citas: {ex.Message}");
                return new List<CitasDD>();
            }
        }

        private void SepararNombreApellido(string nombreCompleto)
        {
            // Verificar que el nombre completo no sea nulo o vacío
            if (!string.IsNullOrWhiteSpace(nombreCompleto))
            {
                // Separamos la cadena usando el espacio en blanco como delimitador
                var partesNombre = nombreCompleto.Split(' ');

                // Asumimos que el primer elemento es el nombre y el último es el apellido
                string nombre = partesNombre[0]; // Primer palabra como nombre
                string apellido = partesNombre.Length > 1 ? partesNombre[partesNombre.Length - 1] : ""; // Última palabra como apellido

                // Si deseas tratar con el caso de que haya más de dos palabras (por ejemplo, "Juan de la Cruz")
                if (partesNombre.Length > 2)
                {
                    apellido = string.Join(" ", partesNombre.Skip(1)); // Unir todas las palabras después del primer nombre
                }

                // Aquí puedes asignar los valores a los campos correspondientes
                Console.WriteLine("Nombre: " + nombre);
                Console.WriteLine("Apellido: " + apellido);
            }
            else
            {
                Console.WriteLine("El nombre completo está vacío.");
            }
        }

        private int? ObtenerIdAsegurador(string planAseguradora)
        {
            DBConnection conn = new DBConnection();
            string query = "SELECT idtercero FROM CONContrato WHERE idcontrato = @planAseguradora";

            try
            {
                using (var connection = new SqlConnection(conn.getCs()))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@planAseguradora", planAseguradora);

                        var result = command.ExecuteScalar();
                        return result != DBNull.Value ? Convert.ToInt32(result) : (int?)null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el IdAsegurador: {ex.Message}");
                return null;
            }
        }

        private string ObtenerNombreAsegurador(int idAsegurador)
        {
            DBConnection conn = new DBConnection();
            string query = "SELECT NomTercero FROM genTercero WHERE IdTercero = @idAsegurador";

            try
            {
                using (var connection = new SqlConnection(conn.getCs()))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idAsegurador", idAsegurador);

                        var result = command.ExecuteScalar();
                        return result != DBNull.Value ? result.ToString() : string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el NombreAsegurador: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
