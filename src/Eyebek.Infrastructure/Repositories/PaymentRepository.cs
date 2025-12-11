using Microsoft.EntityFrameworkCore;
using Eyebek.Application.Interfaces;
using Eyebek.Domain.Entities;
using Eyebek.Infrastructure.Persistence;

namespace Eyebek.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _db;
    public PaymentRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(Payment payment)
    {
        _db.Payments.Add(payment);
        await _db.SaveChangesAsync();
    }

    public Task<List<Payment>> GetByCompanyAsync(int companyId)
        => _db.Payments.Where(x => x.CompanyId == companyId)
            .OrderByDescending(x => x.PaymentDate)
            .ToListAsync();
}