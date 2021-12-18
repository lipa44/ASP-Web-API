using AutoMapper;
using ReportsDataAccessLayer.DataTransferObjects;
using ReportsLibrary.Employees;
using ReportsLibrary.Entities;
using ReportsLibrary.Tasks;

namespace ReportsWebApiLayer.AutoMapping;

public class DomainResponseProfile : Profile
{
    public DomainResponseProfile()
    {
        CreateMap<Employee, EmployeeDto>();
        CreateMap<Employee, FullEmployeeDto>()
            .ForMember(
                employee => employee.Tasks,
                opt =>
                    opt.MapFrom(employee => employee.Tasks))
            .ForMember(
                employee => employee.Subordinates,
                opt =>
                    opt.MapFrom(src => src.Subordinates));

        CreateMap<ReportsTask, ReportsTaskDto>();
        CreateMap<ReportsTask, FullReportsTaskDto>()
            .ForMember(
                task => task.TaskComments,
                opt =>
                    opt.MapFrom(src => src.Comments))
            .ForMember(
                task => task.TaskModifications,
                opt =>
                    opt.MapFrom(src => src.Modifications));

        CreateMap<WorkTeam, WorkTeamDto>();
        CreateMap<WorkTeam, FullWorkTeamDto>()
            .ForMember(
                workTeam => workTeam.Report,
                opt =>
                    opt.MapFrom(src => new ReportDto
                    {
                        OwnerId = src.Report.Owner.Id,
                        Id = src.Id,
                        Modifications = src.Report.Modifications,
                    }));

        CreateMap<Sprint, SprintDto>();
        CreateMap<Report, ReportDto>();
    }
}