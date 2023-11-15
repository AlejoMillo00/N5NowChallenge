using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Services;

internal sealed class PermissionCommands : IPermissionCommands
{
    private readonly ApplicationDbContext _context;

    public PermissionCommands(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse> CreateAsync(Permission permission)
    {
        ServiceResponse sr = new();
        try
        {
            await _context.Permissions.AddAsync(permission);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            sr.AddError(ex);
        }

        return sr;
    }

    public async Task<ServiceResponse> UpdateAsync(Permission permission)
    {
        ServiceResponse sr = new();
        try
        {
            _context.Permissions.Update(permission);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            sr.AddError(ex);
        }

        return sr;
    }
}
