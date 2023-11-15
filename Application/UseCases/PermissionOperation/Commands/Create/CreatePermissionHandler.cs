using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Serilog;
using System.Net;
using System.Text.Json;

namespace Application.UseCases.PermissionOperation;

public sealed class CreatePermissionHandler : IRequestHandler<CreatePermissionRequest, ServiceResponse>
{
    private readonly IPermissionUoW _unitOfWork;
    private readonly IElasticService _elasticService;
    private readonly ITopicService _topicService;
    
    public CreatePermissionHandler(
        IPermissionUoW unitOfWork,
        IElasticService elasticService,
        ITopicService topicService)
    {
        _unitOfWork = unitOfWork;
        _elasticService = elasticService;
        _topicService = topicService;
    }

    public async Task<ServiceResponse> Handle(CreatePermissionRequest request, CancellationToken cancellationToken)
    {
        Log.Information("REQUEST operation started at {0} - Payload: {1}", 
            DateTime.UtcNow, JsonSerializer.Serialize(request));

        ServiceResponse sr = new();

        var permissionTypeSr = await _unitOfWork
            .PermissionTypeQueries.GetAsync(request.PermissionType);

        if (!permissionTypeSr.Success)
        {
            sr.AddErrors(permissionTypeSr.Errors, HttpStatusCode.InternalServerError);
            Log.Error("REQUEST operation failed - Error: {0}", sr.Errors);
            return sr;
        }

        if (permissionTypeSr.Content == null)
        {
            sr.AddError("Invalid permission type", HttpStatusCode.BadRequest);
            Log.Error("REQUEST operation failed - Error: {0}", sr.Errors);
            return sr;
        }

        Permission permission = new()
        {
            EmployeeForename = request.EmployeeForename,
            EmployeeSurname = request.EmployeeSurname,
            PermissionType = permissionTypeSr.Content.Id,
            PermissionDate = DateTime.UtcNow,
        };

        var createSr = await _unitOfWork
            .PermissionCommands.CreateAsync(permission);

        if (!createSr.Success)
        {
            Log.Error("REQUEST operation failed - Error: {0}", createSr.Errors);
            return sr;
        }

        var elasticSr = await _elasticService
            .IndexAsync("permission", createSr.Content);

        if (!elasticSr.Success)
            Log.Error("Error saving permission into elastic - Error: {0}",
                elasticSr.Errors);
        else
            Log.Information("Permission registry saved into elastic successfully");

        var topicSr = await _topicService.ProduceMessageAsync(new PermissionMesssageModel
        {
            Id = Guid.NewGuid(),
            NameOperation = "request",
        });

        if (!topicSr.Success)
            Log.Error("Error persiting permission into kafka - Error: {0}",
                topicSr.Errors);
        else
            Log.Information("Permission persited into kafka successfully");

        sr.Message = "Permission requested successfully.";
        sr.StatusCode = HttpStatusCode.Created;

        Log.Information("REQUEST operation finished successfully at {0}", DateTime.UtcNow);

        return sr;
    }
}
