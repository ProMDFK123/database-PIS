using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bolsafeucn_back.src.Application.DTOs.JobAplicationDTO
{
    /// <summary>
    /// Obtiene los detalles para que el admin vea al postulante
    /// </summary>
    /// TODO: Falta agregar descripcion la cual no esta agregada en el modelo
    public class ViewApplicantDetailAdminDto
    {
        public string StudentName { get; set; } = string.Empty;
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }
    }
}