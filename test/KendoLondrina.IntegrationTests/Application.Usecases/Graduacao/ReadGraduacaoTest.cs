using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using KenLo.Application.Exceptions;
using KenLo.Application.UseCases.Graduacao;
using KenLo.Infra.Data.EF.Repositories;

namespace KenLo.IntegrationTests.Application.Usecases.Graduacao;

[Collection(nameof(ReadGraduacaoFixture))]
public class ReadGraduacaoTest
{
    private readonly ReadGraduacaoFixture _fixture;

    public ReadGraduacaoTest(ReadGraduacaoFixture theFixture)
    {
        _fixture = theFixture;
    }

    [Fact(DisplayName = nameof(ReadGraduacao))]
    public async Task ReadGraduacao()
    {
        // criar um objeto Graduacao
        var graduacao = _fixture.GetGraduacao();
        // salvar este objeto no BD
        var context = _fixture.CreateDbContext();
        context.Graduacoes.Add(graduacao);
        context.SaveChanges();

        var repository = new GraduacaoRepository(context);
        var useCase = new ReadGraduacao(repository);
        var input = new ReadGraduacaoInput(graduacao.Id);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Nome.Should().Be(graduacao.Nome);
        output.Descricao.Should().Be(graduacao.Descricao);
        output.Ativo.Should().Be(graduacao.Ativo);
        output.Id.Should().Be(graduacao.Id);
        output.CriadoEm.Should().Be(graduacao.CriadoEm);
    }

    [Fact(DisplayName = nameof(NotFoundExceptionQuandoGraduacaoNaoExistir))]
    public async Task NotFoundExceptionQuandoGraduacaoNaoExistir()
    {
        // criar um objeto Graduacao
        var graduacao = _fixture.GetGraduacao();
        // salvar este objeto no BD
        var context = _fixture.CreateDbContext();
        context.Graduacoes.Add(graduacao);
        context.SaveChanges();

        var exampleGuid = Guid.NewGuid();
        var repository = new GraduacaoRepository(context);
        var useCase = new ReadGraduacao(repository);
        var input = new ReadGraduacaoInput(exampleGuid);

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Graduacao {input.Id} not found");
    }
}
