namespace Application.Common.Models;
public sealed partial class ServiceError
{
    public string Code { get; set; }
    public string Message { get; set; }

    public override string ToString()
    {
        return $"Error - Code: {Code}, Message: {Message}";
    }
}

