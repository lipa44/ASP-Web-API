using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportsDomain.Entities;
using ReportsInfrastructure.Services.Interfaces;
using ReportsWebApi.DataTransferObjects;
using ReportsWebApi.Extensions;

namespace ReportsWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly IReportsService _reportsService;
    private readonly IMapper _mapper;

    public ReportsController(IReportsService reportsService, IMapper mapper)
    {
        _reportsService = reportsService;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<ReportDto>>> GetReports() =>
        Ok(_mapper.Map<List<ReportDto>>(await _reportsService.GetReportsAsync()));

    [HttpGet("{reportId}", Name = "GetReport")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FullReportDto>> GetReport(Guid reportId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        try
        {
            Report report = await _reportsService.GetReportByIdAsync(reportId);
            return Ok(_mapper.Map<FullReportDto>(report));
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("byEmployee/{employeeId}", Name = "GetReportsByEmployeeId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public ActionResult<IReadOnlyCollection<FullReportDto>> GetReportsByEmployeeId([FromRoute] Guid employeeId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        try
        {
            IReadOnlyCollection<Report> employeeReports = _reportsService.GetReportsByEmployeeIdAsync(employeeId);
            return Ok(_mapper.Map<List<FullReportDto>>(employeeReports));
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateReport(Guid employeeId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        try
        {
            Report newReport = await _reportsService.CreateReport(employeeId);

            return CreatedAtRoute(
                "GetReport", new { reportId = newReport.Id }, _mapper.Map<ReportDto>(newReport));
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpPut("commit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CommitChangesToReport([FromQuery] Guid employeeId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        try
        {
            Report committedReport = await _reportsService.CommitChangesToReport(employeeId);
            return Ok(_mapper.Map<FullReportDto>(committedReport));
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpPut("setDone")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetReportAsDone([FromQuery] Guid changerId, [FromQuery] Guid workTeamId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        try
        {
            Report doneReport = await _reportsService.SetReportAsDone(workTeamId, changerId);
            return Ok(_mapper.Map<FullReportDto>(doneReport));
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpPut("{workTeamId}/generateReportForSprint")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GenerateWOrkTeamReportForSprint([FromRoute] Guid workTeamId, Guid changerId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());

        try
        {
            Report generatedReport = await _reportsService.GenerateWorkTeamReport(workTeamId, changerId);
            return Ok(_mapper.Map<FullReportDto>(generatedReport));
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
}