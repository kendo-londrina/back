using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using KenLo.Application.UseCases.Graduacao;
using KenLo.Infra.Data.EF;
using KenLo.Infra.Data.EF.Repositories;

namespace KenLo.IntegrationTests.Application.Usecases.Graduacao;

[Collection(nameof(DeleteGraduacaoFixture))]
public class DeleteGraduacaoTest
{
    private readonly DeleteGraduacaoFixture _fixture;
    public DeleteGraduacaoTest(DeleteGraduacaoFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteGraduacao))]
    public async Task DeleteGraduacao()
    {
        var graduacao = _fixture.GetGraduacao();
        var arrangeContext = _fixture.CreateDbContext();
        await arrangeContext.Graduacoes.AddAsync(graduacao);
        await arrangeContext.SaveChangesAsync();

        var actionContext = _fixture.CreateDbContext(true);
        var repository = new GraduacaoRepository(actionContext);
        var unitOfWork = new UnitOfWork(actionContext);

        var input = new DeleteGraduacaoInput(graduacao.Id);
        var useCase = new DeleteGraduacao(unitOfWork, repository);
        await useCase.Handle(input, CancellationToken.None);

        var dbGraduacao = await _fixture.CreateDbContext(true)
            .Graduacoes.FindAsync(input.Id);
        dbGraduacao.Should().BeNull();
    }

}
