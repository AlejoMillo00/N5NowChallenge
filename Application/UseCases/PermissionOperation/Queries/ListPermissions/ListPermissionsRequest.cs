using Application.Common.Models;
using Application.Models;
using MediatR;

namespace Application.UseCases.PermissionOperation;

public sealed class ListPermissionsRequest : IRequest<ServiceResponse<List<PermissionDto>>>
{
}
