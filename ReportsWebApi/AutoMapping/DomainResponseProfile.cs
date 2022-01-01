using AutoMapper;
using ReportsDomain.Employees;
using ReportsDomain.Entities;
using ReportsDomain.Tasks;
using ReportsWebApi.DataTransferObjects;

namespace ReportsWebApi.AutoMapping;

public class DomainResponseProfile : Profile
{
    public DomainResponseProfile()
    {
        CreateMap<Employee, EmployeeDto>();
        CreateMap<Employee, EmployeeFullDto>()
            .ForMember(
                fullEmployeeDto => fullEmployeeDto.Tasks,
                opt =>
                    opt.MapFrom(employee => employee.Tasks))
            .ForMember(
                fullEmployeeDto => fullEmployeeDto.Subordinates,
                opt =>
                    opt.MapFrom(src => src.Subordinates))
            .ForMember(
                fullEmployeeDto => fullEmployeeDto.Role,
                opt =>
                    opt.MapFrom(src => src.Role.ToString()))
            .ForMember(
                fullEmployeeDto => fullEmployeeDto.ChiefData,
                opt =>
                    opt.MapFrom(src => src.Chief.ToString()));

        CreateMap<ReportsTask, ReportsTaskDto>();
        CreateMap<ReportsTask, ReportsTaskFullDto>()
            .ForMember(
                fullReportsTaskDto => fullReportsTaskDto.TaskComments,
                opt =>
                    opt.MapFrom(src => src.Comments))
            .ForMember(
                fullReportsTaskDto => fullReportsTaskDto.TaskModifications,
                opt =>
                    opt.MapFrom(src => src.Modifications))
            .ForMember(
                fullReportsTaskDto => fullReportsTaskDto.OwnerData,
                opt =>
                    opt.MapFrom(src => src.Owner.ToString()));

        CreateMap<WorkTeam, WorkTeamDto>();
        CreateMap<WorkTeam, WorkTeamFullDto>()
            .ForMember(
                fullWorkTeamDto => fullWorkTeamDto.Employees,
                opt =>
                    opt.MapFrom(src => src.Employees))
            .ForMember(
                fullWorkTeamDto => fullWorkTeamDto.Sprints,
                opt =>
                    opt.MapFrom(src => src.Sprints));

        CreateMap<Sprint, SprintDto>();

        CreateMap<Report, ReportDto>()
            .ForMember(
                reportDto => reportDto.OwnerData,
                opt =>
                    opt.MapFrom(src => src.Owner.ToString()));

        CreateMap<Report, ReportFullDto>()
            .ForMember(
                fullReportDto => fullReportDto.Modifications,
                opt =>
                    opt.MapFrom(src => src.Modifications))
            .ForMember(
                fullReportDto => fullReportDto.OwnerData,
                opt =>
                        opt.MapFrom(src => src.Owner.ToString()));

        CreateMap<TaskModification, TaskModificationDto>()
            .ForMember(
                taskModificationDto => taskModificationDto.Action,
                opt =>
                    opt.MapFrom(src => src.Action.ToString()));
    }
}