using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using KenLo.Infra.Data.EF;
using KenLo.Infra.Data.EF.Repositories;
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

        await graduacaoRepository.Create(graduacaoExemplo, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var dbGraduacao = await (_fixture.CreateDbContext(true))
            .Graduacoes.FindAsync(graduacaoExemplo.Id);
        dbGraduacao.Should().NotBeNull();
        dbGraduacao!.Nome.Should().Be(graduacaoExemplo.Nome);
        dbGraduacao.Descricao.Should().Be(graduacaoExemplo.Descricao);
        dbGraduacao.Ativo.Should().Be(graduacaoExemplo.Ativo);
        dbGraduacao.CriadoEm.Should().Be(graduacaoExemplo.CriadoEm);
    }
}
