using AutoMapper;
using Stark.Module.System.Domain;
using Stark.Module.System.Models.Results.SysRoles;

namespace Stark.Module.System.Application.Mapper;

/// <summary>
/// 
/// </summary>
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<SysRole, SysRoleResult>().AfterMap((from, to, context) =>
        {
        });
    }
}