using AutoMapper;
using Domain.Entities;
using Domain.Tasks;
using Presentation.DataTransferObjects;

namespace Presentation.AutoMapping;

public class DomainResponseProfile : Profile
{
    public DomainResponseProfile()
    {
        CreateMap<Employee, EmployeeDto>();
        CreateMap<Employee, EmployeeFullDto>()
            .ForMember(
                employeeFullDto => employeeFullDto.Tasks,
                opt =>
                    opt.MapFrom(employee => employee.Tasks))
            .ForMember(
                employeeFullDto => employeeFullDto.Subordinates,
                opt =>
                    opt.MapFrom(employee => employee.Subordinates))
            .ForMember(
                employeeFullDto => employeeFullDto.Role,
                opt =>
                    opt.MapFrom(src => src.Role.ToString()));

        CreateMap<ReportsTask, ReportsTaskDto>();
        CreateMap<ReportsTask, ReportsTaskFullDto>()
            .ForMember(
                reportsTaskFullDto => reportsTaskFullDto.TaskComments,
                opt =>
                    opt.MapFrom(src => src.Comments))
            .ForMember(
                reportsTaskFullDto => reportsTaskFullDto.TaskModifications,
                opt =>
                    opt.MapFrom(src => src.Modifications));

        CreateMap<WorkTeam, WorkTeamDto>()
            .ForMember(
                workTeamDto => workTeamDto.TeamLeadData,
                opt =>
                    opt.MapFrom(src => src.TeamLead.ToString()));
        CreateMap<WorkTeam, WorkTeamFullDto>()
            .ForMember(
                workTeamFullDto => workTeamFullDto.Employees,
                opt =>
                    opt.MapFrom(src => src.Employees))
            .ForMember(
                workTeamFullDto => workTeamFullDto.Sprints,
                opt =>
                    opt.MapFrom(src => src.Sprints))
            .ForMember(
                workTeamFullDto => workTeamFullDto.TeamLeadData,
                opt =>
                    opt.MapFrom(src => src.TeamLead.ToString()));

        CreateMap<Sprint, SprintDto>();
        CreateMap<Sprint, SprintFullDto>()
            .ForMember(
            sprintFullDto => sprintFullDto.Tasks,
            opt =>
                opt.MapFrom(src => src.Tasks));

        CreateMap<Report, ReportDto>()
            .ForMember(
                reportDto => reportDto.OwnerData,
                opt =>
                    opt.MapFrom(src => src.Owner.ToString()))
            .ForMember(
                reportDto => reportDto.State,
                opt =>
                    opt.MapFrom(src => src.States.ToString()));

        CreateMap<Report, ReportFullDto>()
            .ForMember(
                reportFullDto => reportFullDto.Modifications,
                opt =>
                    opt.MapFrom(src => src.Modifications))
            .ForMember(
                reportFullDto => reportFullDto.OwnerData,
                opt =>
                    opt.MapFrom(src => src.Owner.ToString()))
            .ForMember(
                reportDto => reportDto.State,
                opt =>
                    opt.MapFrom(src => src.States.ToString()));

        CreateMap<TaskModification, TaskModificationDto>()
            .ForMember(
                taskModificationDto => taskModificationDto.Action,
                opt =>
                    opt.MapFrom(src => src.Action.ToString()));
    }
}