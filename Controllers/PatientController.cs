using HospitalManagement.Data.Entities;
using HospitalManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly PatientService _service;

    public PatientController(PatientService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
        => Ok(await _service.GetAllAlphabeticalAsync(page, pageSize));

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string name)
        => Ok(await _service.SearchByNameAsync(name));

    // Recherche par numéro de dossier (query critique étape 7)
    [HttpGet("file/{fileNumber}")]
    public async Task<IActionResult> GetByFileNumber(string fileNumber)
    {
        var patient = await _service.GetByFileNumberAsync(fileNumber);
        return patient is null ? NotFound() : Ok(patient);
    }

    // Compter les patients par département (query critique étape 7)
    [HttpGet("stats/by-department")]
    public async Task<IActionResult> CountByDepartment()
        => Ok(await _service.CountPatientsByDepartmentAsync());

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Patient patient)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = await _service.CreateAsync(patient);
        return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Patient patient)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var updated = await _service.UpdateAsync(id, patient);
            return updated is null ? NotFound() : Ok(updated);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // Conflit détecté → on retourne 409 Conflict
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}