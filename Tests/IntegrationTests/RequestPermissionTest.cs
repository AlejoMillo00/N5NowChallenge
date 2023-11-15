using Application.Models;
using Application.UseCases.PermissionOperation;
using System.Net;

namespace Tests.IntegrationTests;

public sealed class RequestPermissionTest
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Should_Request_Permission_OK(int permissionType)
    {
        var permissionUoW = MockPermissionUoW();
        var elasticService = MockElasticService();
        var topicService = MockTopicService();
        var handler = new CreatePermissionHandler(
            permissionUoW, elasticService, topicService);

        var request = new CreatePermissionRequest
        {
            EmployeeForename = "Test",
            EmployeeSurname = "Test",
            PermissionType = permissionType,
        };

        var permissionsCountBeforeRequest = Permissions.Count;

        var sr = await handler.Handle(request, CancellationToken.None);

        Assert.NotNull(sr);
        Assert.True(sr.Success);
        Assert.True(Permissions.Count == permissionsCountBeforeRequest + 1);
    }

    [Theory]
    [InlineData(9313)]
    [InlineData(8373)]
    public async Task Should_Failed_With_Invalid_PermissionType(int permissionType)
    {
        var permissionUoW = MockPermissionUoW();
        var elasticService = MockElasticService();
        var topicService = MockTopicService();
        var handler = new CreatePermissionHandler(
            permissionUoW, elasticService, topicService);

        var request = new CreatePermissionRequest
        {
            EmployeeForename = "Test",
            EmployeeSurname = "Test",
            PermissionType = permissionType,
        };

        var permissionsCountBeforeRequest = Permissions.Count;

        var sr = await handler.Handle(request, CancellationToken.None);

        Assert.NotNull(sr);
        Assert.Equal(HttpStatusCode.BadRequest, sr.StatusCode);
        Assert.False(sr.Success);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Should_Get_Permission_Ok(int id)
    {
        var mapper = MockMapper();
        var permissionQueries = MockPermissionQueries();
        var elasticService = MockElasticService();
        var topicService = MockTopicService();
        var handler = new GetPermissionHandler(
            permissionQueries, mapper, elasticService, topicService);

        var request = new GetPermissionRequest
        {
            Id = id,
        };

        var sr = await handler.Handle(request, CancellationToken.None);

        Assert.NotNull(sr);
        Assert.True(sr.Success);
        Assert.NotNull(sr.Content);
        Assert.IsType<PermissionDto>(sr.Content);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 1)]
    public async Task Should_Modify_Permission_OK(int id, int permissionType)
    {
        var permissionUoW = MockPermissionUoW();
        var elasticService = MockElasticService();
        var topicService = MockTopicService();
        var handler = new UpdatePermissionHandler(
            permissionUoW, elasticService, topicService);

        var request = new UpdatePermissionRequest
        {
            Id = id,
            PermissionType = permissionType,
        };

        var permissionTypeBeforeUpdate = Permissions.FirstOrDefault(x => x.Id == id).PermissionType;

        var sr = await handler.Handle(request, CancellationToken.None);

        var updatedPermission = Permissions.FirstOrDefault(x => x.Id == id);

        Assert.NotNull(sr);
        Assert.True(sr.Success);
        Assert.NotEqual(permissionTypeBeforeUpdate, updatedPermission.PermissionType);
        Assert.Equal(permissionType, updatedPermission.PermissionType);
    }

    [Theory]
    [InlineData(1, 3)]
    [InlineData(2, 3)]
    public async Task Should_Modify_Permission_Fail_With_Invalid_PermissionType(int id, int permissionType)
    {
        var permissionUoW = MockPermissionUoW();
        var elasticService = MockElasticService();
        var topicService = MockTopicService();
        var handler = new UpdatePermissionHandler(
            permissionUoW, elasticService, topicService);

        var request = new UpdatePermissionRequest
        {
            Id = id,
            PermissionType = permissionType,
        };

        var sr = await handler.Handle(request, CancellationToken.None);

        Assert.NotNull(sr);
        Assert.False(sr.Success);
        Assert.Equal(HttpStatusCode.BadRequest, sr.StatusCode);
    }

    [Theory]
    [InlineData(99, 2)]
    [InlineData(999, 1)]
    public async Task Should_Modify_Permission_Fail_NotFound(int id, int permissionType)
    {
        var permissionUoW = MockPermissionUoW();
        var elasticService = MockElasticService();
        var topicService = MockTopicService();
        var handler = new UpdatePermissionHandler(
            permissionUoW, elasticService, topicService);

        var request = new UpdatePermissionRequest
        {
            Id = id,
            PermissionType = permissionType,
        };

        var sr = await handler.Handle(request, CancellationToken.None);

        Assert.NotNull(sr);
        Assert.False(sr.Success);
        Assert.Equal(HttpStatusCode.NotFound, sr.StatusCode);
    }

    [Theory]
    [InlineData(99)]
    [InlineData(999)]
    public async Task Should_Fail_Get_Permission_NotFound(int id)
    {
        var mapper = MockMapper();
        var permissionQueries = MockPermissionQueries();
        var elasticService = MockElasticService();
        var topicService = MockTopicService();
        var handler = new GetPermissionHandler(
            permissionQueries, mapper, elasticService, topicService);

        var request = new GetPermissionRequest
        {
            Id = id,
        };

        var sr = await handler.Handle(request, CancellationToken.None);

        Assert.NotNull(sr);
        Assert.Equal(HttpStatusCode.NotFound, sr.StatusCode);
        Assert.False(sr.Success);
        Assert.Null(sr.Content);
    }

    [Fact]
    public async Task Should_List_Permissions_Ok()
    {
        var mapper = MockMapper();
        var permissionQueries = MockPermissionQueries();
        var topicService = MockTopicService();
        var handler = new ListPermissionsHandler(
            permissionQueries, mapper, topicService);

        var sr = await handler.Handle(new(), CancellationToken.None);

        Assert.NotNull(sr);
        Assert.True(sr.Success);
        Assert.Equal(Permissions.Count, sr.Content.Count);
    }
}
