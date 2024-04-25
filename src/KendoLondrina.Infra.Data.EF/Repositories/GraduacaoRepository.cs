using KenLo.Application.Exceptions;
using KenLo.Domain.Entity;
using KenLo.Domain.Repository;
using KenLo.Domain.SeedWork.SearchableRepository;
using Microsoft.EntityFrameworkCore;

namespace KenLo.Infra.Data.EF.Repositories;

public class GraduacaoRepository : IGraduacaoRepository
{
    private readonly KendoLondrinaDbContext _context;
    private DbSet<Graduacao> _graduacoes => _context.Set<Graduacao>();
    public GraduacaoRepository(KendoLondrinaDbContext context)
    {
        _context = context;
    }

    public async Task Create(Graduacao aggregate, CancellationToken cancellationToken)
        => await _graduacoes.AddAsync(aggregate, cancellationToken);

    public async Task<Graduacao> Read(Guid id, CancellationToken cancellationToken)
    {
        var graduacao = await _graduacoes.AsNoTracking().FirstOrDefaultAsync(
            x => x.Id == id, cancellationToken);
        NotFoundException.ThrowIfNull(graduacao, $"Graduacao {id} not found");
        return graduacao!;
    }

    public Task Update(Graduacao aggregate, CancellationToken cancellationToken)
    {
        return Task.FromResult(_graduacoes.Update(aggregate));
    }

    public Task Delete(Graduacao aggregate, CancellationToken cancellationToken)
        => Task.FromResult(_graduacoes.Remove(aggregate));

    public async Task<IReadOnlyList<Graduacao>> ListarGraduacoesPorIds(List<Guid> ids, CancellationToken cancellationToken)
        => await _graduacoes.AsNoTracking().Where(x => ids.Contains(x.Id)).ToListAsync();

    public Task<SearchOutput<Graduacao>> List(SearchInput input, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}