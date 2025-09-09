
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RIPSEnvio
{
    public class JsonRip {

        public string codigoUnicoValidacion { get; set; }
        public RipsModel rips { get; set; }
        public string xmlFevFile { get; set; }

    }
        public class RipsModel
        {

            public string numDocumentoIdObligado { get; set; }
            public string numFactura { get; set; }
            public object tipoNota { get; set; }
            public object numNota { get; set; }
            public List<Usuario> usuarios { get; set; }
            // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);



        }
        public class Consulta
        {
            public string codPrestador { get; set; }
            public string fechaInicioAtencion { get; set; }
            public string numAutorizacion { get; set; }
            public string codConsulta { get; set; }
            public string modalidadGrupoServicioTecSal { get; set; }
            public string grupoServicios { get; set; }
            public int codServicio { get; set; }
            public string finalidadTecnologiaSalud { get; set; }
            public string causaMotivoAtencion { get; set; }
            public string codDiagnosticoPrincipal { get; set; }

        [JsonProperty("codDiagnosticoRelacionado1", NullValueHandling = NullValueHandling.Ignore)]
        public string codDiagnosticoRelacionado1 { get; set; }

        [JsonProperty("codDiagnosticoRelacionado2", NullValueHandling = NullValueHandling.Ignore)]
        public object codDiagnosticoRelacionado2 { get; set; }

        [JsonProperty("codDiagnosticoRelacionado3", NullValueHandling = NullValueHandling.Ignore)]
        public object codDiagnosticoRelacionado3 { get; set; }
            public string tipoDiagnosticoPrincipal { get; set; }
            public string tipoDocumentoIdentificacion { get; set; }
            public string numDocumentoIdentificacion { get; set; }
            public double vrServicio { get; set; }
            public string conceptoRecaudo { get; set; }
            public double valorPagoModerador { get; set; }
            public string numFEVPagoModerador { get; set; }
            public int consecutivo { get; set; }
        }

        public class Hospitalizacion
        {
            public string codPrestador { get; set; }
            public string viaIngresoServicioSalud { get; set; }
            public string fechaInicioAtencion { get; set; }
            public string numAutorizacion { get; set; }
            public string causaMotivoAtencion { get; set; }
            public string codDiagnosticoPrincipal { get; set; }
            public string codDiagnosticoPrincipalE { get; set; }
            public object codDiagnosticoRelacionadoE1 { get; set; }
            public object codDiagnosticoRelacionadoE2 { get; set; }
            public object codDiagnosticoRelacionadoE3 { get; set; }
            public object codComplicacion { get; set; }
            public string condicionDestinoUsuarioEgreso { get; set; }
            public object codDiagnosticoCausaMuerte { get; set; }
            public string fechaEgreso { get; set; }
            public int consecutivo { get; set; }

        }


    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Medicamento
        {
            public string codPrestador { get; set; }
            public object numAutorizacion { get; set; }
            public string idMIPRES { get; set; }
            public string fechaDispensAdmon { get; set; }
            public string codDiagnosticoPrincipal { get; set; }

        [JsonProperty("codDiagnosticoRelacionado", NullValueHandling = NullValueHandling.Ignore)]
        public object codDiagnosticoRelacionado { get; set; }
            public string tipoMedicamento { get; set; }
            public string codTecnologiaSalud { get; set; }
            public object nomTecnologiaSalud { get; set; }
            public int concentracionMedicamento { get; set; }
            public int unidadMedida { get; set; }
            public string formaFarmaceutica { get; set; }
            public int unidadMinDispensa { get; set; }
            public int cantidadMedicamento { get; set; }
            public int diasTratamiento { get; set; }
            public string tipoDocumentoIdentificacion { get; set; }
            public string numDocumentoIdentificacion { get; set; }
            public double vrUnitMedicamento { get; set; }
            public long vrServicio { get; set; }
            public string conceptoRecaudo { get; set; }
            public int valorPagoModerador { get; set; }
            public object numFEVPagoModerador { get; set; }
            public int consecutivo { get; set; }
        }

        public class OtrosServicio
        {
            public string codPrestador { get; set; }
            public object numAutorizacion { get; set; }
            public string idMIPRES { get; set; }
            public string fechaSuministroTecnologia { get; set; }
            public string tipoOS { get; set; }
            public string codTecnologiaSalud { get; set; }
            public string nomTecnologiaSalud { get; set; }
            public int cantidadOS { get; set; }
            public string tipoDocumentoIdentificacion { get; set; }
            public string numDocumentoIdentificacion { get; set; }
            public double vrUnitOS { get; set; }
            public double vrServicio { get; set; }
            public string conceptoRecaudo { get; set; }
            public double valorPagoModerador { get; set; }
            public string numFEVPagoModerador { get; set; }
            public int consecutivo { get; set; }
        }

        public class Procedimiento
        {
            public string codPrestador { get; set; }
            public string fechaInicioAtencion { get; set; }
            public object idMIPRES { get; set; }
            public object numAutorizacion { get; set; }
            public string codProcedimiento { get; set; }
            public string viaIngresoServicioSalud { get; set; }
            public string modalidadGrupoServicioTecSal { get; set; }
            public string grupoServicios { get; set; }
            public int codServicio { get; set; }
            public string finalidadTecnologiaSalud { get; set; }
            public string tipoDocumentoIdentificacion { get; set; }
            public string numDocumentoIdentificacion { get; set; }
            public string codDiagnosticoPrincipal { get; set; }
            public string codDiagnosticoRelacionado { get; set; }
            public string codComplicacion { get; set; }
            public long vrServicio { get; set; }
            public string conceptoRecaudo { get; set; }
            public int valorPagoModerador { get; set; }
            public string numFEVPagoModerador { get; set; }
            public int consecutivo { get; set; }
        }

        public class RecienNacido
        {
            public string codPrestador { get; set; }
            public string tipoDocumentoIdentificacion { get; set; }
            public string numDocumentoIdentificacion { get; set; }
            public string fechaNacimiento { get; set; }
            public int edadGestacional { get; set; }
            public int numConsultasCPrenatal { get; set; }
            public string codSexoBiologico { get; set; }
            public int peso { get; set; }
            public string codDiagnosticoPrincipal { get; set; }
            public string condicionDestinoUsuarioEgreso { get; set; }
            public object codDiagnosticoCausaMuerte { get; set; }
            public string fechaEgreso { get; set; }
            public int consecutivo { get; set; }
        }



        public class Servicios
        {
            public List<Consulta> consultas { get; set; }
            public List<Procedimiento> procedimientos { get; set; }
            public List<Urgencia> urgencias { get; set; }
            public List<Hospitalizacion> hospitalizacion { get; set; }
            public List<RecienNacido> recienNacidos { get; set; }
            public List<Medicamento> medicamentos { get; set; }
            public List<OtrosServicio> otrosServicios { get; set; }
        }

        public class Urgencia
        {
            public string codPrestador { get; set; }
            public string fechaInicioAtencion { get; set; }
            public string causaMotivoAtencion { get; set; }
            public string codDiagnosticoPrincipal { get; set; }
            public string codDiagnosticoPrincipalE { get; set; }
            public object codDiagnosticoRelacionadoE1 { get; set; }
            public object codDiagnosticoRelacionadoE2 { get; set; }
            public object codDiagnosticoRelacionadoE3 { get; set; }
            public string condicionDestinoUsuarioEgreso { get; set; }
            public object codDiagnosticoCausaMuerte { get; set; }
            public string fechaEgreso { get; set; }
            public int consecutivo { get; set; }
        }

        public class Usuario
        {
            public string tipoDocumentoIdentificacion { get; set; }
            public string numDocumentoIdentificacion { get; set; }
            public string tipoUsuario { get; set; }
            public string fechaNacimiento { get; set; }
            public string codSexo { get; set; }
            public string codPaisResidencia { get; set; }
            public string codMunicipioResidencia { get; set; }
            public string codZonaTerritorialResidencia { get; set; }
            public string incapacidad { get; set; }
            public int consecutivo { get; set; }
            public string codPaisOrigen { get; set; }
            public Servicios servicios { get; set; }
        }

    
}