using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models;
using AutoMapper;
using MediatR;
using System.Net;

namespace Application.UseCases.PermissionOperation;

public sealed class ListPermissionsHandler : IRequestHandler<ListPermissionsRequest, ServiceResponse<List<PermissionDto>>>
{
    private readonly IPermissionQueries _permissionQueries;
    private readonly IMapper _mapper;

    public ListPermissionsHandler(IPermissionQueries permissionQueries, IMapper mapper)
    {
        _permissionQueries = permissionQueries;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<List<PermissionDto>>> Handle(ListPermissionsRequest request, CancellationToken cancellationToken)
    {
        ServiceResponse<List<PermissionDto>> sr = new();

        var permissionsSr = await _permissionQueries.ListAsync();

        if(!permissionsSr.Success)
        {
            sr.AddErrors(permissionsSr.Errors, HttpStatusCode.InternalServerError);
            return sr;
        }

        sr.Content = _mapper.Map<List<PermissionDto>>(permissionsSr.Content);

        sr.StatusCode = sr.Content.Count > 0 
            ? HttpStatusCode.OK 
            : HttpStatusCode.NoContent;

        return sr;
    }
}
