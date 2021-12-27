namespace ReportsWebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        Report report = await _reportsService.GetReportByIdAsync(reportId);
        return Ok(_mapper.Map<FullReportDto>(report));
    }

    [HttpGet("byEmployee/{employeeId}", Name = "GetReportsByEmployeeId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public ActionResult<IReadOnlyCollection<FullReportDto>> GetReportsByEmployeeId([FromRoute] Guid employeeId)
    {
        IReadOnlyCollection<Report> employeeReports = _reportsService.GetReportsByEmployeeIdAsync(employeeId);
        return Ok(_mapper.Map<List<FullReportDto>>(employeeReports));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateReport(Guid employeeId)
    {
        Report newReport = await _reportsService.CreateReport(employeeId);

        return CreatedAtRoute(
            "GetReport", new { reportId = newReport.Id }, _mapper.Map<ReportDto>(newReport));
    }

    [HttpPut("commit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CommitChangesToReport([FromQuery] Guid employeeId)
    {
        Report committedReport = await _reportsService.CommitChangesToReport(employeeId);
        return Ok(_mapper.Map<FullReportDto>(committedReport));
    }

    [HttpPut("setDone")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetReportAsDone([FromQuery] Guid changerId, [FromQuery] Guid workTeamId)
    {
        Report doneReport = await _reportsService.SetReportAsDone(workTeamId, changerId);
        return Ok(_mapper.Map<FullReportDto>(doneReport));
    }

    [HttpPut("{workTeamId}/generateReportForSprint")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GenerateWOrkTeamReportForSprint([FromRoute] Guid workTeamId, Guid changerId)
    {
        Report generatedReport = await _reportsService.GenerateWorkTeamReport(workTeamId, changerId);
        return Ok(_mapper.Map<FullReportDto>(generatedReport));
    }
}