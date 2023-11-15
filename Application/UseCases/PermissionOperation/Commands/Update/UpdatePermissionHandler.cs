using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models;
using MediatR;
using Serilog;
using System.Net;
using System.Text.Json;

namespace Application.UseCases.PermissionOperation;

public sealed class UpdatePermissionHandler : IRequestHandler<UpdatePermissionRequest, ServiceResponse>
{
    private readonly IPermissionUoW _unitOfWork;
    private readonly IElasticService _elasticService;
    private readonly ITopicService _topicService;

    public UpdatePermissionHandler(
        IPermissionUoW unitOfWork,
        IElasticService elasticService,
        ITopicService topicService)
    {
        _unitOfWork = unitOfWork;
        _elasticService = elasticService;
        _topicService = topicService;
    }

    public async Task<ServiceResponse> Handle(UpdatePermissionRequest request, CancellationToken cancellationToken)
    {
        Log.Information("MODIFY operation started at {0} - Payload: {1}", 
            DateTime.UtcNow, JsonSerializer.Serialize(request));

        ServiceResponse sr = new();

        var permissionSr = await _unitOfWork
            .PermissionQueries.GetAsync(request.Id);

        if (!permissionSr.Success)
        {
            sr.AddErrors(permissionSr.Errors, HttpStatusCode.InternalServerError);
            Log.Error("MODIFY operation failed - Error: {0}", sr.Errors);
            return sr;
        }

        if(permissionSr.Content == null)
        {
            sr.AddError("Permission not found.", HttpStatusCode.NotFound);
            Log.Error("MODIFY operation failed - Error: {0}", sr.Errors);
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
                Log.Error("MODIFY operation failed - Error: {0}", sr.Errors);
                return sr;
            }

            if (permissionTypeSr.Content == null)
            {
                sr.AddError("Invalid permission type", HttpStatusCode.BadRequest);
                Log.Error("MODIFY operation failed - Error: {0}", sr.Errors);
                return sr;
            }

            permissionDb.PermissionType = permissionTypeSr.Content.Id;
        }

        permissionDb.EmployeeForename = request.EmployeeForename ?? permissionDb.EmployeeForename;
        permissionDb.EmployeeSurname = request.EmployeeSurname ?? permissionDb.EmployeeSurname;
        permissionDb.PermissionDate = DateTime.UtcNow;

        sr = await _unitOfWork
            .PermissionCommands.UpdateAsync(permissionDb);

        if (!sr.Success)
        {
            Log.Error("MODIFY operation failed - Error: {0}", sr.Errors);
            return sr;
        }

        var elasticSr = await _elasticService
            .IndexAsync("permission", permissionDb);

        if (!elasticSr.Success)
            Log.Error("Error saving permission into elastic - Error: {0}",
                elasticSr.Errors);
        else
            Log.Information("Permission registry saved into elastic successfully");

        var topicSr = await _topicService.ProduceMessageAsync(new PermissionMesssageModel
        {
            Id = Guid.NewGuid(),
            NameOperation = "modify",
        });

        if (!topicSr.Success)
            Log.Error("Error persiting permission into kafka - Error: {0}",
                topicSr.Errors);
        else
            Log.Information("Permission persited into kafka successfully");

        sr.Message = "Permission modified successfully.";
        sr.StatusCode = HttpStatusCode.OK;

        Log.Information("MODIFY operation finished successfully at {0}", DateTime.UtcNow);

        return sr;
    }
}
