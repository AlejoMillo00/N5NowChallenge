using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models;
using AutoMapper;
using MediatR;
using Serilog;
using System.Net;

namespace Application.UseCases.PermissionOperation;

public sealed class ListPermissionsHandler : IRequestHandler<ListPermissionsRequest, ServiceResponse<List<PermissionDto>>>
{
    private readonly IPermissionQueries _permissionQueries;
    private readonly IMapper _mapper;
    private readonly ITopicService _topicService;

    public ListPermissionsHandler(
        IPermissionQueries permissionQueries, 
        IMapper mapper,
        ITopicService topicService)
    {
        _permissionQueries = permissionQueries;
        _mapper = mapper;
        _topicService = topicService;
    }

    public async Task<ServiceResponse<List<PermissionDto>>> Handle(ListPermissionsRequest request, CancellationToken cancellationToken)
    {
        Log.Information("LIST operation started at {0}", DateTime.UtcNow);

        ServiceResponse<List<PermissionDto>> sr = new();

        var permissionsSr = await _permissionQueries.ListAsync();

        if(!permissionsSr.Success)
        {
            sr.AddErrors(permissionsSr.Errors, HttpStatusCode.InternalServerError);
            Log.Error("LIST operation failed - Error: {0}", sr.Errors);
            return sr;
        }

        sr.Content = _mapper.Map<List<PermissionDto>>(permissionsSr.Content);

        var topicSr = await _topicService.ProduceMessageAsync(new PermissionMesssageModel
        {
            Id = Guid.NewGuid(),
            NameOperation = "list",
        });

        if (!topicSr.Success)
            Log.Error("Error persiting permission into kafka - Error: {0}",
                topicSr.Errors);
        else
            Log.Information("Permission persited into kafka successfully");

        sr.StatusCode = sr.Content.Count > 0 
            ? HttpStatusCode.OK 
            : HttpStatusCode.NoContent;

        Log.Information("LIST operation finished successfully at {0}", DateTime.UtcNow);

        return sr;
    }
}
