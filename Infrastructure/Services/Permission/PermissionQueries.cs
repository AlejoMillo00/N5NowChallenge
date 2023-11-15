using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

internal sealed class PermissionQueries : IPermissionQueries
{
    private readonly ApplicationDbContext _context;
    public PermissionQueries(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<Permission>> GetAsync(int id)
    {
        ServiceResponse<Permission> sr = new();
        try
        {
            sr.Content = await _context
                .Permissions.FirstOrDefaultAsync(x => x.Id == id);
        }
        catch (Exception ex)
        {
            sr.AddError(ex);
        }

        return sr;
    }

    public async Task<ServiceResponse<List<Permission>>> ListAsync()
    {
        ServiceResponse<List<Permission>> sr = new();
        try
        {
            sr.Content = await _context.Permissions.ToListAsync(); ;
        }
        catch (Exception ex)
        {
            sr.AddError(ex);
        }

        return sr;
    }
}
