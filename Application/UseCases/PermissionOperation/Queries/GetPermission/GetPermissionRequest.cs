using Application.Common.Models;
using Application.Models;
using MediatR;

namespace Application.UseCases.PermissionOperation;

public sealed class GetPermissionRequest : IRequest<ServiceResponse<PermissionDto>>
{
    public int Id { get; set; }
}
