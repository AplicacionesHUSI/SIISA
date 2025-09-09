namespace HUSI_SIISA.Utilities
{/*
    public class CitasDD
    {
        public int? appointmentID { get; set; }
        public DateTime fechaCita { get; set; }

        public string? estadoCita { get; set; }

        public string? nombreServicio { get; set; }
        public string? consultorioRemoteID { get; set; }
        public string? planAseguradora { get; set; }

        public int? remoteID { get; set; }
        public int? servicioID { get; set; }
        public int? pacienteRemoteID { get; set; }
        public string? observaciones { get; set; }
        public string? consultorio { get; set; }
        public string? doctorRemoteID { get; set; }
        public string? tipoServicioRemoteID { get; set; }
    }

    */
    public class CitasDD
    {
        public int? appointmentID { get; set; }
        public DateTime fechaCita { get; set; }
        public string? horaInicial { get; set; }
        public string horaFinal { get; set; }
        public int? duracion { get; set; }
        public string? estadoCita { get; set; }
        public string? identificacionMedico { get; set; }
        public string? nombreMedico { get; set; }
        public string? cupsServicio { get; set; }
        public string? nombrePlanAseguradora { get; set; }
        public string? nombreServicio { get; set; }
        public string? consultorioRemoteID { get; set; }
        public string? planAseguradora { get; set; }
        public string? nombrePaciente { get; set; }
        public string? documentoPaciente { get; set; }
        public string? tipoDocumentoPaciente { get; set; }
        public string? correoElectronico { get; set; }
        public string? celular { get; set; }
        public bool? citaExtra { get; set; }
        public object? remoteID { get; set; }
        public string modalidad { get; set; }
        public int? doctorID { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int? servicioID { get; set; }
        public int? pacienteRemoteID { get; set; }
        public string? observaciones { get; set; }
        public string? consultorio { get; set; }
        public string? doctorRemoteID { get; set; }
        public string? tipoServicioRemoteID { get; set; }
        public bool agendadorAlterno { get; set; }
    }


}
