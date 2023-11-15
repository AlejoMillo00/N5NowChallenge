using Application.Common.Models;
using MediatR;

namespace Application.UseCases.PermissionOperation;

public sealed class UpdatePermissionRequest : IRequest<ServiceResponse>
{
    public int Id { get; set; }
#nullable enable
    public string? EmployeeForename { get; set; }
    public string? EmployeeSurname { get; set; }
    public int? PermissionType { get; set; }
#nullable disable
}
