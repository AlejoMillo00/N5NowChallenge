namespace Application.Models;

public sealed class PermissionDto
{
    public int Id { get; set; }
    public int PermissionType { get; set; }
    public string EmployeeForename { get; set; }
    public string EmployeeSurname { get; set; }
    public string PermissionDate { get; set; }
}
