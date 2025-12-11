using Eyebek.Domain.Entities;

namespace Eyebek.Application.Interfaces;

public interface IAuditRepository
{
    Task AddAsync(Audit audit);
}