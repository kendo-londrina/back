namespace KenLo.Domain.SeedWork.SearchableRepository;

public interface ISearchableRepository<Taggregate>
    where Taggregate : AggregateRoot
{
    Task<SearchOutput<Taggregate>> List(
        SearchInput input,
        CancellationToken cancellationToken
    );
}
