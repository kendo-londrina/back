using System.Threading;
using System.Threading.Tasks;
using KenLo.Infra.Data.EF;
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
        var graduacaoExemplo = _fixture.GetGraduacaoValida();
        var graduacaoRepository = new GraduacaoRepository(dbContext);

        await graduacaoRepository.Insert(graduacaoExemplo, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var dbGraduacao = await (_fixture.CreateDbContext(true))
            .Graduacoes.FindAsync(graduacaoExemplo.Id);
        dbGraduacao.Should().NotBeNull();
        dbGraduacao!.Name.Should().Be(graduacaoExemplo.Nome);
        dbGraduacao.Description.Should().Be(graduacaoExemplo.Descricao);
        dbGraduacao.IsActive.Should().Be(graduacaoExemplo.Ativo);
        dbGraduacao.CreatedAt.Should().Be(graduacaoExemplo.CriadoEm);
    }
}
