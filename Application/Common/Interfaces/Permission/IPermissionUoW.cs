namespace Application.Common.Interfaces;
public interface IPermissionUoW
{
    IPermissionTypeQueries PermissionTypeQueries { get; }
    IPermissionCommands PermissionCommands { get; }
    IPermissionQueries PermissionQueries { get; }
}
