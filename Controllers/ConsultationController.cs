using HospitalManagement.Data;
using HospitalManagement.Data.Entities;
using HospitalManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsultationController : ControllerBase
{
    private readonly ConsultationService _service;
    private readonly AppDbContext _context;

    public ConsultationController(ConsultationService service, AppDbContext context)
    {
        _service = service;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var consultations = await _context.Consultations
            .Include(c => c.Patient)
            .Include(c => c.Doctor)
            .AsNoTracking()
            .ToListAsync();
        return Ok(consultations);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var consultation = await _context.Consultations
            .Include(c => c.Patient)
            .Include(c => c.Doctor)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
        return consultation is null ? NotFound() : Ok(consultation);
    }

    [HttpPost]
    public async Task<IActionResult> Schedule([FromBody] Consultation consultation)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = await _service.ScheduleAsync(consultation);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] ConsultationStatus status)
    {
        var updated = await _service.UpdateStatusAsync(id, status);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpPatch("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var cancelled = await _service.CancelAsync(id);
        return cancelled ? NoContent() : NotFound();
    }

    [HttpGet("patient/{patientId}/upcoming")]
    public async Task<IActionResult> GetUpcomingForPatient(int patientId)
        => Ok(await _service.GetUpcomingForPatientAsync(patientId));

    [HttpGet("doctor/{doctorId}/today")]
    public async Task<IActionResult> GetTodayForDoctor(int doctorId)
        => Ok(await _service.GetTodayForDoctorAsync(doctorId));
}