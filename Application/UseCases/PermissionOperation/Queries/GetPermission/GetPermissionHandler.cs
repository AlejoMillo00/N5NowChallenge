using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models;
using AutoMapper;
using MediatR;
using System.Net;

namespace Application.UseCases.PermissionOperation;

public sealed class GetPermissionHandler : IRequestHandler<GetPermissionRequest, ServiceResponse<PermissionDto>>
{
    private readonly IPermissionQueries _permissionQueries;
    private readonly IMapper _mapper;

    public GetPermissionHandler(IPermissionQueries permissionQueries, IMapper mapper)
    {
        _permissionQueries = permissionQueries;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<PermissionDto>> Handle(GetPermissionRequest request, CancellationToken cancellationToken)
    {
        ServiceResponse<PermissionDto> sr = new();

        var permissionSr = await _permissionQueries.GetAsync(request.Id);

        if (!permissionSr.Success)
        {
            sr.AddErrors(permissionSr.Errors, HttpStatusCode.InternalServerError);
            return sr;
        }

        if(permissionSr.Content == null)
        {
            sr.AddError("Permission not found", HttpStatusCode.NotFound);
            return sr;
        }

        sr.Content = _mapper.Map<PermissionDto>(permissionSr.Content);

        sr.StatusCode = HttpStatusCode.OK;
      
        return sr;
    }
}
