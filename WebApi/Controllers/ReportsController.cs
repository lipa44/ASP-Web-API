using AutoMapper;
using DataAccess.Dto;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Interfaces;
using WebApi.Extensions;
using WebApi.Filters;

namespace WebApi.Controllers;

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
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<ReportDto>>> GetReports(
        [FromQuery] int takeAmount,
        [FromQuery] int pageNumber)
    {
        IReadOnlyCollection<Report> reports = await _reportsService.GetReports();

        var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

        return Ok(IndexViewModelExtensions<ReportDto>
            .ToIndexViewModel(_mapper.Map<List<ReportDto>>(reports), paginationFilter));
    }

    [HttpGet("{reportId:guid}", Name = "GetReport")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReportFullDto>> GetReport([FromRoute] Guid reportId)
    {
        Report report = await _reportsService.FindReportById(reportId);

        if (report is null) return NotFound();

        return Ok(_mapper.Map<ReportFullDto>(report));
    }

    [HttpGet("byEmployee/{employeeId:guid}", Name = "GetReportsByEmployeeId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReportFullDto>> GetReportByEmployeeId(
        [FromRoute] Guid employeeId)
    {
        Report employeeReport = await _reportsService.GetReportByEmployeeId(employeeId);

        if (employeeReport is null) return NotFound();

        return Ok(_mapper.Map<ReportFullDto>(employeeReport));
    }

    // [HttpPost("{employeeId:guid}")]
    // [ProducesResponseType(StatusCodes.Status201Created)]
    // public async Task<IActionResult> CreateReport([FromRoute] Guid employeeId)
    // {
    //     Report newReport = await _reportsService.CreateReport(employeeId);
    //
    //     return CreatedAtRoute(
    //         "GetReport", new { reportId = newReport.Id }, _mapper.Map<ReportDto>(newReport));
    // }
    [HttpPut("{employeeId:guid}/commit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CommitChangesToReport([FromRoute] Guid employeeId)
    {
        Report committedReport = await _reportsService.CommitChangesToReport(employeeId);

        return Ok(_mapper.Map<ReportFullDto>(committedReport));
    }

    [HttpPut("{workTeamId:guid}/state")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SetReportAsDone([FromRoute] Guid workTeamId, [FromQuery] Guid changerId)
    {
        Report doneReport = await _reportsService.SetReportAsDone(workTeamId, changerId);

        return Ok(_mapper.Map<ReportFullDto>(doneReport));
    }

    // [HttpPost("{workTeamId:guid}/report")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    // public async Task<IActionResult> GenerateWorkTeamReportForSprint(
    //     [FromRoute] Guid workTeamId,
    //     [FromQuery] Guid changerId)
    // {
    //     Report generatedReport = await _reportsService.GenerateWorkTeamReport(workTeamId, changerId);
    //
    //     return Ok(_mapper.Map<ReportFullDto>(generatedReport));
    // }
}