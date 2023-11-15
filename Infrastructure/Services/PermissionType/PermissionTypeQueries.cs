using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

internal sealed class PermissionTypeQueries : IPermissionTypeQueries
{
    private readonly ApplicationDbContext _context;

    public PermissionTypeQueries(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<PermissionType>> GetAsync(int id)
    {
        ServiceResponse<PermissionType> sr = new();

        try
        {
            sr.Content = await _context.PermissionTypes
            .FirstOrDefaultAsync(x => x.Id == id); ;
        }
        catch (Exception ex)
        {
            sr.AddError(ex);
        }

        return sr;
    }
}
