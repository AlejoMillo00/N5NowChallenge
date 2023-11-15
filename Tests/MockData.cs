using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Moq;
using System.Net;

namespace Tests;

public static class MockData
{
    public static List<PermissionType> PermissionTypes = new List<PermissionType>
    {
        new PermissionType
        {
            Id = 1,
            Description = "Test Permission Type",
        },
        new PermissionType
        {
            Id = 2,
            Description = "Test Permission Type 2",
        },
    };

    public static List<Permission> Permissions = new List<Permission>
    {
        new Permission
        {
            Id = 1,
            EmployeeForename = "Test 1",
            EmployeeSurname = "Test |",
            PermissionType = 1,
            PermissionDate = new DateTime(2023, 11, 15),
        },
        new Permission
        {
            Id = 2,
            EmployeeForename = "Test 2",
            EmployeeSurname = "Test 2",
            PermissionType = 2,
            PermissionDate = new DateTime(2023, 11, 15),
        },
    };

    public static IPermissionUoW MockPermissionUoW()
    {
        var mockPermissionUoW = new Mock<IPermissionUoW>();
        var mockPermissionTypeQueries = MockPermissionTypeQueries();
        var mockPermissionQueries = MockPermissionQueries();
        var mockPermissionCommands = MockPermissionCommands();

        mockPermissionUoW.Setup(m => m.PermissionTypeQueries).Returns(mockPermissionTypeQueries);
        mockPermissionUoW.Setup(m => m.PermissionQueries).Returns(mockPermissionQueries);
        mockPermissionUoW.Setup(m => m.PermissionCommands).Returns(mockPermissionCommands);

        return mockPermissionUoW.Object;
    }

    public static IPermissionTypeQueries MockPermissionTypeQueries()
    {
        var mockPermissionTypeQueries = new Mock<IPermissionTypeQueries>();

        mockPermissionTypeQueries.Setup(m => m.GetAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) =>
            {
                var permissionType = PermissionTypes.FirstOrDefault(x => x.Id == id);
                return new ServiceResponse<PermissionType>
                {
                    Content = permissionType,
                };
            });

        return mockPermissionTypeQueries.Object;
    }

    public static IPermissionQueries MockPermissionQueries()
    {
        var mockPermissionQueries = new Mock<IPermissionQueries>();

        mockPermissionQueries.Setup(m => m.GetAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => {
                var permission = Permissions.FirstOrDefault(x => x.Id == id);
                return new ServiceResponse<Permission>
                {
                    Content = permission,
                };
            });

        mockPermissionQueries.Setup(m => m.ListAsync())
            .ReturnsAsync(() => {
                return new ServiceResponse<List<Permission>>
                {
                    Content = Permissions,
                };
            });

        return mockPermissionQueries.Object;
    }

    public static IPermissionCommands MockPermissionCommands()
    {
        var mockPermissionQueries = new Mock<IPermissionCommands>();

        mockPermissionQueries.Setup(m => m.CreateAsync(It.IsAny<Permission>()))
            .ReturnsAsync((Permission permission) => {
                permission.Id = Permissions.Last()?.Id + 1 ?? 1;
                Permissions.Add(permission);
                return new ServiceResponse<Permission>
                {
                    Content = permission,
                    StatusCode = HttpStatusCode.Created,
                };
            });

        mockPermissionQueries.Setup(m => m.UpdateAsync(It.IsAny<Permission>()))
            .ReturnsAsync((Permission permission) => {
                var permissionIndex = Permissions.FindIndex(x => x.Id == permission.Id);
                if(permissionIndex == -1)
                    return new ServiceResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                    };

                Permissions[permissionIndex] = permission;
                return new ServiceResponse
                {
                    StatusCode = HttpStatusCode.OK,
                };
            });

        return mockPermissionQueries.Object;
    }

    public static IElasticService MockElasticService()
    {
        var mockElasticService = new Mock<IElasticService>();

        mockElasticService.Setup(m => m.IndexAsync(It.IsAny<string>(), It.IsAny<It.IsAnyType>()))
            .ReturnsAsync(new ServiceResponse());

        return mockElasticService.Object;
    }

    public static ITopicService MockTopicService()
    {
        var mockTopicService = new Mock<ITopicService>();

        mockTopicService.Setup(m => m.ProduceMessageAsync(It.IsAny<It.IsAnyType>()))
            .ReturnsAsync(new ServiceResponse());

        return mockTopicService.Object;
    }

    public static IMapper MockMapper()
    {
        var mockMapper = new Mock<IMapper>();

        mockMapper.Setup(m => m.Map<PermissionDto>(It.IsAny<Permission>()))
            .Returns((Permission src) =>
            {
                return new PermissionDto
                {
                    Id = src.Id,
                    EmployeeForename = src.EmployeeForename,
                    EmployeeSurname = src.EmployeeSurname,
                    PermissionDate = src.PermissionDate.ToShortDateString(),
                    PermissionType = src.PermissionType,
                };
            });

        mockMapper.Setup(m => m.Map<List<PermissionDto>>(It.IsAny<List<Permission>>()))
            .Returns((List<Permission> src) =>
            {
                var response = new List<PermissionDto>();

                for(int i = 0; i < src.Count; i++)
                {
                    response.Add(new PermissionDto
                    {
                        Id = src[i].Id,
                        EmployeeForename = src[i].EmployeeForename,
                        EmployeeSurname = src[i].EmployeeSurname,
                        PermissionDate = src[i].PermissionDate.ToShortDateString(),
                        PermissionType = src[i].PermissionType,
                    });
                }

                return response;
            });

        return mockMapper.Object;
    }
}
