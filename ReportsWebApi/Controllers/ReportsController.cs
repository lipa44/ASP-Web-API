namespace ReportsWebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReportsDomain.Entities;
using ReportsInfrastructure.Services.Interfaces;
using DataTransferObjects;

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

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<ReportDto>>> GetReports() =>
        Ok(_mapper.Map<List<ReportDto>>(await _reportsService.GetReportsAsync()));

    [HttpGet("{reportId}", Name = "GetReport")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReportFullDto>> GetReport([FromRoute] Guid reportId)
    {
        Report report = await _reportsService.GetReportByIdAsync(reportId);
        return Ok(_mapper.Map<ReportFullDto>(report));
    }

    [HttpGet("byEmployee/{employeeId}", Name = "GetReportsByEmployeeId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IReadOnlyCollection<ReportFullDto>> GetReportsByEmployeeId([FromRoute] Guid employeeId)
    {
        IReadOnlyCollection<Report> employeeReports = _reportsService.GetReportsByEmployeeIdAsync(employeeId);
        return Ok(_mapper.Map<List<ReportFullDto>>(employeeReports));
    }

    [HttpPost("{employeeId}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateReport([FromRoute] Guid employeeId)
    {
        Report newReport = await _reportsService.CreateReport(employeeId);

        return CreatedAtRoute(
            "GetReport", new { reportId = newReport.Id }, _mapper.Map<ReportDto>(newReport));
    }

    [HttpPut("{employeeId}/commit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CommitChangesToReport([FromRoute] Guid employeeId)
    {
        Report committedReport = await _reportsService.CommitChangesToReport(employeeId);
        return Ok(_mapper.Map<ReportFullDto>(committedReport));
    }

    [HttpPut("{workTeamId}/setDone")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> SetReportAsDone([FromRoute] Guid workTeamId, [FromQuery] Guid changerId)
    {
        Report doneReport = await _reportsService.SetReportAsDone(workTeamId, changerId);
        return Ok(_mapper.Map<ReportFullDto>(doneReport));
    }

    [HttpPut("{workTeamId}/generateReportForSprint")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GenerateWOrkTeamReportForSprint([FromRoute] Guid workTeamId, [FromQuery] Guid changerId)
    {
        Report generatedReport = await _reportsService.GenerateWorkTeamReport(workTeamId, changerId);
        return Ok(_mapper.Map<ReportFullDto>(generatedReport));
    }
}