using System.Linq;
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

    public async Task<SearchOutput<Graduacao>> Search(SearchInput input, CancellationToken cancellationToken)
    {
        var query = _graduacoes.AsNoTracking();
        var toSkip = (input.Page - 1) * input.PerPage;
        query = AddOrderToQuery(query, input.OrderBy, input.Order);
        if(!String.IsNullOrWhiteSpace(input.Search))
            query = query.Where(x => x.Nome.Contains(input.Search));        

        var total = await query.CountAsync();
        var items = await query
            .Skip(toSkip).Take(input.PerPage)
            .ToListAsync();
        return new(input.Page, input.PerPage, total, items);
    }

    private IQueryable<Graduacao> AddOrderToQuery(
        IQueryable<Graduacao> query,
        string orderProperty,
        SearchOrder order
    )
    { 
        var orderedQuery = (orderProperty.ToLower(), order) switch
        {
            ("nome", SearchOrder.Asc) => query.OrderBy(x => x.Nome)
                .ThenBy(x => x.Id),
            ("nome", SearchOrder.Desc) => query.OrderByDescending(x => x.Nome)
                .ThenByDescending(x => x.Id),
            ("id", SearchOrder.Asc) => query.OrderBy(x => x.Id),
            ("id", SearchOrder.Desc) => query.OrderByDescending(x => x.Id),
            ("criadoem", SearchOrder.Asc) => query.OrderBy(x => x.CriadoEm),
            ("criadoem", SearchOrder.Desc) => query.OrderByDescending(x => x.CriadoEm),
            _ => query.OrderBy(x => x.Nome)
                .ThenBy(x => x.Id)
        };
        return orderedQuery;
    }
}