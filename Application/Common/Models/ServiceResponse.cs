using System.Net;

namespace Application.Common.Models;

public partial class ServiceResponse<T>
{
    public T Content { get; set; }
    public string Message { get; set; }
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public List<ServiceError> Errors { get; }
    public bool Success { get => !Errors.Any(); }
    public Dictionary<string, string> Headers { get; set; }

    public ServiceResponse()
    {
        Errors = new List<ServiceError>();
        Headers = new Dictionary<string, string>();
    }

    public ServiceResponse(T content)
    {
        Errors = new List<ServiceError>();
        Headers = new Dictionary<string, string>();
        Content = content;
    }

    public ServiceResponse(HttpStatusCode statusCode)
    {
        Errors = new List<ServiceError>();
        Headers = new Dictionary<string, string>();
        StatusCode = statusCode;
    }

    public ServiceResponse(T content, HttpStatusCode statusCode)
    {
        Errors = new List<ServiceError>();
        Headers = new Dictionary<string, string>();
        StatusCode = statusCode;
        Content = content;
    }

    public void AddErrors(IList<ServiceError> errors)
    {
        Errors.AddRange(errors);
        StatusCode = HttpStatusCode.InternalServerError;
    }

    public void AddErrors(IList<ServiceError> errors, HttpStatusCode statusCode)
    {
        Errors.AddRange(errors);
        StatusCode = statusCode;
    }

    public void AddError(ServiceError error)
    {
        Errors.Add(error);
        StatusCode = HttpStatusCode.InternalServerError;
    }

    public void AddError(Exception ex)
    {
        var finalException = ex;

        if(ex.InnerException != null)
        {
            finalException = ex.InnerException;
        }

        Errors.Add(new ServiceError
        {
            Code = finalException.Message,
        });
        StatusCode = HttpStatusCode.InternalServerError;
    }

    public void AddError(string code)
    {
        Errors.Add(new ServiceError
        {
            Code = code,
            Message = code,
        });
        StatusCode = HttpStatusCode.InternalServerError;
    }

    public void AddError(string code, string message)
    {
        Errors.Add(new ServiceError
        {
            Code = code,
            Message = message
        });
        StatusCode = HttpStatusCode.InternalServerError;
    }

    public void AddError(string code, HttpStatusCode statusCode)
    {
        Errors.Add(new ServiceError
        {
            Code = code,
            Message = code,
        });
        StatusCode = statusCode;
    }
}

public sealed class ServiceResponse : ServiceResponse<object> { }

