using Api.Common;
using Application.Common.Models;
using Application.Models;
using Application.UseCases.PermissionOperation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class PermissionController : ApiControllerBase
{
    /// <summary>
    /// Requests a permission
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    [HttpPost, Route("request")]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreatePermissionRequest body)
    {
        return Result(await Mediator.Send(body));
    }

    /// <summary>
    /// Requests a permission
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    [HttpPut, Route("modify")]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromBody] UpdatePermissionRequest body)
    {
        return Result(await Mediator.Send(body));
    }

    /// <summary>
    /// Gets a permission
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet, Route("{id}")]
    [ProducesResponseType(typeof(ServiceResponse<PermissionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        return Result(await Mediator.Send(new GetPermissionRequest
        {
            Id = id,
        }));
    }

    /// <summary>
    /// List all permissions
    /// </summary>
    /// <returns></returns>
    [HttpGet, Route("list")]
    [ProducesResponseType(typeof(ServiceResponse<List<PermissionDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> List()
    {
        return Result(await Mediator.Send(new ListPermissionsRequest()));
    }
}