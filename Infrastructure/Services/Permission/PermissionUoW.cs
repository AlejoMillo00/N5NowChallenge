using Application.Common.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Services;

internal sealed class PermissionUoW : IPermissionUoW
{
    private readonly ApplicationDbContext _context;
    private PermissionTypeQueries _permissionTypeQueries = null;
    private PermissionCommands _permissionCommands = null;
    private PermissionQueries _permissionQueries = null;

    public PermissionUoW(ApplicationDbContext context)
    {
        _context = context;
    }

    public IPermissionTypeQueries PermissionTypeQueries => 
        _permissionTypeQueries ??= new PermissionTypeQueries(_context);

    public IPermissionCommands PermissionCommands => 
        _permissionCommands ??= new PermissionCommands(_context);

    public IPermissionQueries PermissionQueries =>
        _permissionQueries ??= new PermissionQueries(_context);
}
