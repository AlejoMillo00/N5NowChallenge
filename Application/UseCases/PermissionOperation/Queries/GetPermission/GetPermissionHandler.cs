using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Serilog;
using System.Net;

namespace Application.UseCases.PermissionOperation;

public sealed class GetPermissionHandler : IRequestHandler<GetPermissionRequest, ServiceResponse<PermissionDto>>
{
    private readonly IPermissionQueries _permissionQueries;
    private readonly IMapper _mapper;
    private readonly IElasticService _elasticService;
    private readonly ITopicService _topicService;

    public GetPermissionHandler(
        IPermissionQueries permissionQueries, 
        IMapper mapper,
        IElasticService elasticService,
        ITopicService topicService)
    {
        _permissionQueries = permissionQueries;
        _mapper = mapper;
        _elasticService = elasticService;
        _topicService = topicService;
    }

    public async Task<ServiceResponse<PermissionDto>> Handle(GetPermissionRequest request, CancellationToken cancellationToken)
    {
        Log.Information("GET operation started at {0} - Id: {1}",
            DateTime.UtcNow, request.Id);

        ServiceResponse<PermissionDto> sr = new();

        var permissionSr = await _permissionQueries.GetAsync(request.Id);

        if (!permissionSr.Success)
        {
            sr.AddErrors(permissionSr.Errors, HttpStatusCode.InternalServerError);
            Log.Error("GET operation failed - Error: {0}", sr.Errors);
            return sr;
        }

        if(permissionSr.Content == null)
        {
            sr.AddError("Permission not found", HttpStatusCode.NotFound);
            Log.Error("GET operation failed - Error: {0}", sr.Errors);
            return sr;
        }

        sr.Content = _mapper.Map<PermissionDto>(permissionSr.Content);

        var elasticSr = await _elasticService
            .IndexAsync("permission", permissionSr.Content);

        if (!elasticSr.Success)
            Log.Error("Error saving permission into elastic - Error: {0}",
                elasticSr.Errors);
        else
            Log.Information("Permission registry saved into elastic successfully");

        var topicSr = await _topicService.ProduceMessageAsync(new PermissionMesssageModel
        {
            Id = Guid.NewGuid(),
            NameOperation = "get",
        });

        if (!topicSr.Success)
            Log.Error("Error persiting permission into kafka - Error: {0}",
                topicSr.Errors);
        else
            Log.Information("Permission persited into kafka successfully");

        sr.StatusCode = HttpStatusCode.OK;

        Log.Information("GET operation finished successfully at {0}", DateTime.UtcNow);

        return sr;
    }
}
