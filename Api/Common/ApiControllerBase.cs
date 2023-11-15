using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Common;

public class ApiControllerBase : ControllerBase
{
    private ISender _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();

    public IActionResult Result<T>(ServiceResponse<T> sr)
    {
        AddHeaders(this, sr);
        return new JsonResult(sr) { StatusCode = (int)sr.StatusCode };
    }

    private void AddHeaders<T>(ControllerBase controller, ServiceResponse<T> sr)
    {
        if (sr.Headers.Any())
        {
            foreach (var header in sr.Headers)
                controller.Response.Headers.Add(header.Key, header.Value);
        }
    }
}

