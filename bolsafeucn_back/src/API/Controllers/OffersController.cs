using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bolsafeucn_back.src.Application.Services.Interfaces;
using bolsafeucn_back.src.Application.DTOs;
using bolsafeucn_back.src.Domain.Models;

using bolsafeucn_back.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace bolsafeucn_back.src.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        private readonly IOfferService _offerService;
        private readonly IJobApplicationService _jobApplicationService;

        public OffersController(IOfferService offerService, IJobApplicationService jobApplicationService)
        {
            _offerService = offerService;
            _jobApplicationService = jobApplicationService;
        }

        // GET: api/Offers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OfferDetailsDto>> GetOffer(int id)
        {
            var offerDetails = await _offerService.GetOfferDetailsAsync(id);
            if (offerDetails == null)
            {
                return NotFound(new { message = "Oferta no encontrada" });
            }
            return Ok(new { message = "Detalles de la oferta recuperados con éxito", data = offerDetails });
        }

        // POST: api/Offers/{id}/apply
        [HttpPost("{id}/apply")]
        public async Task<ActionResult<JobApplicationResponseDto>> ApplyToOffer(int id, [FromBody] CreateJobApplicationDto dto, [FromQuery] string studentId)
        {
            try
            {
                dto.OfertaLaboralId = id;
                var application = await _jobApplicationService.CreateApplicationAsync(studentId, dto);
                return Ok(new GenericResponse<JobApplicationResponseDto>("Postulación creada exitosamente", application));
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(new GenericResponse<object>(ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new GenericResponse<object>(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new GenericResponse<object>(ex.Message));
            }
        }

        // GET: api/Offers/my-applications
        [HttpGet("my-applications")]
        public async Task<ActionResult<IEnumerable<JobApplicationResponseDto>>> GetMyApplications([FromQuery] string studentId)
        {
            var applications = await _jobApplicationService.GetStudentApplicationsAsync(studentId);
            return Ok(new GenericResponse<IEnumerable<JobApplicationResponseDto>>("Postulaciones recuperadas exitosamente", applications));
        }
    }
}