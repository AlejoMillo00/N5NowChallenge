namespace Domain.Entities;

public sealed class Permission
{
    public int Id { get; set; }
    public string EmployeeForename { get; set; }
    public string EmployeeSurname { get; set; }
    public int PermissionType { get; set; }
    public DateTime PermissionDate { get; set; }
    public PermissionType PermissionTypeEntity { get; set; }
}
