using Application.Common.Models;
using MediatR;

namespace Application.UseCases.PermissionOperation;

public sealed class CreatePermissionRequest : IRequest<ServiceResponse>
{
    public string EmployeeForename { get; set; }
    public string EmployeeSurname { get; set; }
    public int PermissionType { get; set; }
}
