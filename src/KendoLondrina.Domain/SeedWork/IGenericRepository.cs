namespace KenLo.Domain.SeedWork;
public interface IGenericRepository<TAggregate>
    where TAggregate : AggregateRoot
{
    public Task Create(TAggregate aggregate, CancellationToken cancellationToken);
    public Task<TAggregate> Read(Guid id, CancellationToken cancellationToken);
    public Task Update(TAggregate aggregate, CancellationToken cancellationToken);
    public Task Delete(TAggregate aggregate, CancellationToken cancellationToken);
}
