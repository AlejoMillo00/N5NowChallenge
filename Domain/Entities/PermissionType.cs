namespace Domain.Entities;

public sealed class PermissionType
{
    public PermissionType()
    {
        Permissions = new HashSet<Permission>();
    }

    public int Id { get; set; }
    public string Description { get; set; }
    public ICollection<Permission> Permissions { get; set;}
}
