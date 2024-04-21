using KenLo.Domain.Entity;
using KenLo.Domain.SeedWork;
using KenLo.Domain.SeedWork.SearchableRepository;

namespace KenLo.Domain.Repository;
public interface IGraduacaoRepository :
    IGenericRepository<Graduacao>,
    ISearchableRepository<Graduacao>
{
    public Task<IReadOnlyList<Graduacao>> ListarGraduacoesPorIds(
        List<Guid> ids,
        CancellationToken cancellationToken
    );
}
