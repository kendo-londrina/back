using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace KenLo.IntegrationTests.Infra.Data.EF.Repositories;

[Collection(nameof(GraduacaoRepositoryFixture))]
public class GraduacaoRepositoryTest
{
    private readonly GraduacaoRepositoryFixture _fixture;

    public GraduacaoRepositoryTest(GraduacaoRepositoryFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Insert))]
    public async Task Insert()
    {
        KendoLondrinaDbContext dbContext = _fixture.CreateDbContext();
        var graduacaoExemplo = _fixture.GetGraduacao();
        var graduacaoRepository = new GraduacaoRepository(dbContext);

        await graduacaoRepository.Insert(graduacaoExemplo, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var dbGraduacao = await (_fixture.CreateDbContext(true))
            .Graduacoes.FindAsync(graduacaoExemplo.Id);
        dbGraduacao.Should().NotBeNull();
        dbGraduacao!.Name.Should().Be(graduacaoExemplo.Name);
        dbGraduacao.Description.Should().Be(graduacaoExemplo.Description);
        dbGraduacao.IsActive.Should().Be(graduacaoExemplo.IsActive);
        dbGraduacao.CreatedAt.Should().Be(graduacaoExemplo.CreatedAt);
    }
}
