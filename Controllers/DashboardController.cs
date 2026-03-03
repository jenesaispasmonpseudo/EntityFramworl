using HospitalManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly DashboardService _service;

    public DashboardController(DashboardService service)
    {
        _service = service;
    }

    // GET api/Dashboard/patient/1
    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetPatientDetail(int patientId)
    {
        var result = await _service.GetPatientDetailAsync(patientId);
        return result is null ? NotFound() : Ok(result);
    }

    // GET api/Dashboard/doctor/1/planning
    [HttpGet("doctor/{doctorId}/planning")]
    public async Task<IActionResult> GetDoctorPlanning(int doctorId)
    {
        var result = await _service.GetDoctorPlanningAsync(doctorId);
        return result is null ? NotFound() : Ok(result);
    }

    // GET api/Dashboard/departments/stats
    [HttpGet("departments/stats")]
    public async Task<IActionResult> GetDepartmentStats()
    {
        var result = await _service.GetDepartmentStatsAsync();
        return Ok(result);
    }
}