using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using System.Net;

namespace Application.UseCases.PermissionOperation;

public sealed class CreatePermissionHandler : IRequestHandler<CreatePermissionRequest, ServiceResponse>
{
    private readonly IPermissionUoW _unitOfWork;
    
    public CreatePermissionHandler(IPermissionUoW unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResponse> Handle(CreatePermissionRequest request, CancellationToken cancellationToken)
    {
        ServiceResponse sr = new();

        var permissionTypeSr = await _unitOfWork
            .PermissionTypeQueries.GetAsync(request.PermissionType);

        if (!permissionTypeSr.Success)
        {
            sr.AddErrors(permissionTypeSr.Errors, HttpStatusCode.InternalServerError);
            return sr;
        }

        if (permissionTypeSr.Content == null)
        {
            sr.AddError("Invalid permission type", HttpStatusCode.BadRequest);
            return sr;
        }

        Permission permission = new()
        {
            EmployeeForename = request.EmployeeForename,
            EmployeeSurname = request.EmployeeSurname,
            PermissionType = permissionTypeSr.Content.Id,
            PermissionDate = DateOnly.FromDateTime(DateTime.UtcNow),
        };

        sr = await _unitOfWork
            .PermissionCommands.CreateAsync(permission);

        if (!sr.Success)
            return sr;

        sr.Message = "Permission requested successfully.";
        sr.StatusCode = HttpStatusCode.Created;
        return sr;
    }
}
