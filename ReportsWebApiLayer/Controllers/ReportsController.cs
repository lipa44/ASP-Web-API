using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReportsDataAccessLayer.Services.Interfaces;
using ReportsLibrary.Entities;
using ReportsWebApiLayer.DataTransferObjects;

namespace ReportsWebApiLayer.Controllers;

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

    // GET: api/Reports
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<ReportDto>>> GetReports() =>
         _mapper.Map<List<ReportDto>>(await _reportsService.GetReportsAsync());

    // GET: api/Reports/1
    [HttpGet("{reportId}", Name = "GetReport")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReportDto>> GetReport(Guid reportId)
    {
        try
        {
            Report report = await _reportsService.GetReportByIdAsync(reportId);
            return _mapper.Map<ReportDto>(report);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    // POST: api/Reports?employeeId=2
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateReport(Guid employeeId)
    {
        try
        {
            Report newReport = await _reportsService.CreateReport(employeeId);

            return CreatedAtRoute(
                "GetReport", new { id = newReport.Id }, _mapper.Map<ReportDto>(newReport));
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    // PUT: api/Reports/1/chief?chiefId=5
    [HttpPut("commit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CommitChangesToReport([FromQuery] Guid employeeId)
    {
        try
        {
            await _reportsService.CommitChangesToReport(employeeId);
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    // PUT: api/Reports/1/createReport
    [HttpPut("createReport")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CreateReportForEmployee([FromRoute] Guid ownerId)
    {
        try
        {
            await _reportsService.CreateReport(ownerId);
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    // PUT: api/Reports/1/createReport
    [HttpPut("{workTeamId}/generateReportForSprint")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GenerateWOrkTeamReportForSprint([FromRoute] Guid workTeamId, Guid changerId)
    {
        try
        {
            await _reportsService.GenerateWorkTeamReport(workTeamId, changerId);
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
}