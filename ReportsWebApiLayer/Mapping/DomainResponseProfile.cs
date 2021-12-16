using AutoMapper;
using ReportsLibrary.Employees;
using ReportsLibrary.Entities;
using ReportsLibrary.Tasks;
using ReportsWebApiLayer.DataTransferObjects;

namespace ReportsWebApiLayer.Mapping;

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
        CreateMap<Sprint, SprintDto>();
    }
}