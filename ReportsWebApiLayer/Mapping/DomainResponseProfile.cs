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
                e => e.Tasks,
                opt =>
                    opt.MapFrom(e => e.Tasks));

        CreateMap<ReportsTask, ReportsTaskDto>();
        CreateMap<ReportsTask, FullReportsTaskDto>()
            .ForMember(
                t => t.TaskComments,
                opt =>
                    opt.MapFrom(src => src.Comments))
            .ForMember(
                t => t.TaskModifications,
                opt =>
                    opt.MapFrom(src => src.Modifications));

        CreateMap<WorkTeam, WorkTeamDto>();
        CreateMap<Sprint, SprintDto>();
    }
}