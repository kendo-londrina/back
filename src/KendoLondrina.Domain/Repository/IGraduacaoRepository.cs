using KenLo.Domain.Entity;
using KenLo.Domain.SeedWork;

namespace KenLo.Domain.Repository;
public interface IGraduacaoRepository : IGenericRepository<Graduacao>
{
    // public Task<IReadOnlyList<Guid>> GetIdsListByIds(
    //     List<Guid> ids,
    //     CancellationToken cancellationToken
    // );

    public Task<IReadOnlyList<Graduacao>> ListarGraduacoesPorIds(
        List<Guid> ids,
        CancellationToken cancellationToken
    );
}
