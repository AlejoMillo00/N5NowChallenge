using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System.Net;

namespace Application.UseCases.PermissionOperation;

public sealed class UpdatePermissionHandler : IRequestHandler<UpdatePermissionRequest, ServiceResponse>
{
    private readonly IPermissionUoW _unitOfWork;

    public UpdatePermissionHandler(IPermissionUoW unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResponse> Handle(UpdatePermissionRequest request, CancellationToken cancellationToken)
    {
        ServiceResponse sr = new();

        var permissionSr = await _unitOfWork
            .PermissionQueries.GetAsync(request.Id);

        if (!permissionSr.Success)
        {
            sr.AddErrors(permissionSr.Errors, HttpStatusCode.InternalServerError);
            return sr;
        }

        if(permissionSr.Content == null)
        {
            sr.AddError("Permission not found.", HttpStatusCode.NotFound);
            return sr;
        }

        var permissionDb = permissionSr.Content;

        if(request.PermissionType.HasValue && 
            request.PermissionType.Value != permissionDb.PermissionType)
        {
            var permissionTypeSr = await _unitOfWork
                .PermissionTypeQueries.GetAsync(request.PermissionType.Value);

            if (!permissionTypeSr.Success)
            {
                sr.AddErrors(permissionSr.Errors, HttpStatusCode.InternalServerError);
                return sr;
            }

            if (permissionTypeSr.Content == null)
            {
                sr.AddError("Invalid permission type", HttpStatusCode.BadRequest);
                return sr;
            }

            permissionDb.PermissionType = permissionTypeSr.Content.Id;
        }

        permissionDb.EmployeeForename = request.EmployeeForename ?? permissionDb.EmployeeForename;
        permissionDb.EmployeeSurname = request.EmployeeSurname ?? permissionDb.EmployeeSurname;
        permissionDb.PermissionDate = DateOnly.FromDateTime(DateTime.UtcNow);

        sr = await _unitOfWork
            .PermissionCommands.UpdateAsync(permissionDb);

        if (!sr.Success)
            return sr;

        sr.Message = "Permission modified successfully.";
        sr.StatusCode = HttpStatusCode.OK;
        return sr;
    }
}
