using KenLo.Application.Interfaces;

namespace KenLo.Infra.Data.EF;

public class UnitOfWork : IUnitOfWork
{
    private readonly KendoLondrinaDbContext _context;
    public UnitOfWork(KendoLondrinaDbContext context) => _context = context;

    public Task Commit(CancellationToken cancellationToken)
        => _context.SaveChangesAsync(cancellationToken);

    public Task Rollback(CancellationToken cancellationToken)
        => Task.CompletedTask;
}