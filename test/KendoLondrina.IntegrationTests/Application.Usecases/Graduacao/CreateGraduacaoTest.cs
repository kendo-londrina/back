using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using KenLo.Application.UseCases.Graduacao;
using KenLo.Domain.Exceptions;
using KenLo.Infra.Data.EF;
using KenLo.Infra.Data.EF.Repositories;

namespace KenLo.IntegrationTests.Application.Usecases.Graduacao;

[Collection(nameof(CreateGraduacaoFixture))]
public class CreateGraduacaoTest
{
    private readonly CreateGraduacaoFixture _fixture;

    public CreateGraduacaoTest(CreateGraduacaoFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CreateGraduacao))]
    public async void CreateGraduacao()
    {
        var context = _fixture.CreateDbContext();
        var unitOfWork = new UnitOfWork(context);
        var repository = new GraduacaoRepository(context);
        var useCase = new CreateGraduacao(unitOfWork, repository);
        var input = new CreateGraduacaoInput(
            _fixture.GetNomeValido(),
            _fixture.GetDescricaoValida(),
            _fixture.GetRandomBoolean()
        );

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Nome.Should().Be(input.Nome);
        output.Descricao.Should().Be(input.Descricao);
        output.Ativo.Should().Be(input.Ativo);
        output.Id.Should().NotBeEmpty();
        output.CriadoEm.Should().NotBeSameDateAs(default);

        var dbGraduacao = await _fixture.CreateDbContext(true)
            .Graduacoes.FindAsync(output.Id);
        dbGraduacao.Should().NotBeNull();
        dbGraduacao!.Nome.Should().Be(input.Nome);
        dbGraduacao.Descricao.Should().Be(input.Descricao);
        dbGraduacao.Ativo.Should().Be(input.Ativo);
        dbGraduacao.CriadoEm.Should().Be(output.CriadoEm);
    }

    [Fact(DisplayName = nameof(CreateGraduacaoApenasComNome))]
    public async void CreateGraduacaoApenasComNome()
    {
        var context = _fixture.CreateDbContext();
        var repository = new GraduacaoRepository(context);
        var unitOfWork = new UnitOfWork(context);
        var useCase = new CreateGraduacao(unitOfWork, repository);

        var input = new CreateGraduacaoInput(
            _fixture.GetNomeValido()
        );

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Nome.Should().Be(input.Nome);
        output.Descricao.Should().Be("");
        output.Ativo.Should().BeTrue();
        output.Id.Should().NotBeEmpty();
        output.CriadoEm.Should().NotBeSameDateAs(default);

        var dbGraduacao = await _fixture.CreateDbContext(true)
            .Graduacoes.FindAsync(output.Id);
        dbGraduacao.Should().NotBeNull();
        dbGraduacao!.Nome.Should().Be(input.Nome);
        dbGraduacao.Descricao.Should().Be("");
        dbGraduacao.Ativo.Should().BeTrue();
        dbGraduacao.CriadoEm.Should().Be(output.CriadoEm);
    }

    [Fact(DisplayName = nameof(CreateGraduacaoApenasComNomeDescricao))]
    public async void CreateGraduacaoApenasComNomeDescricao()
    {
        var context = _fixture.CreateDbContext();
        var repository = new GraduacaoRepository(context);
        var unitOfWork = new UnitOfWork(context);
        var useCase = new CreateGraduacao(unitOfWork, repository);

        var input = new CreateGraduacaoInput(
            _fixture.GetNomeValido(),
            _fixture.GetDescricaoValida()
        );

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Nome.Should().Be(input.Nome);
        output.Descricao.Should().Be(input.Descricao);
        output.Ativo.Should().BeTrue();
        output.Id.Should().NotBeEmpty();
        output.CriadoEm.Should().NotBeSameDateAs(default);

        var dbGraduacao = await _fixture.CreateDbContext(true)
            .Graduacoes.FindAsync(output.Id);
        dbGraduacao.Should().NotBeNull();
        dbGraduacao!.Nome.Should().Be(input.Nome);
        dbGraduacao.Descricao.Should().Be(input.Descricao);
        dbGraduacao.Ativo.Should().BeTrue();
        dbGraduacao.CriadoEm.Should().Be(output.CriadoEm);
    }

    [Theory(DisplayName = nameof(EntityValidationExceptionQuandoNaoInstanciarGraduacao))]
    [MemberData(
        nameof(CreateGraduacaoDataGenerator.GetInvalidInputs),
        parameters: 24,
        MemberType = typeof(CreateGraduacaoDataGenerator)
    )]
    public async void EntityValidationExceptionQuandoNaoInstanciarGraduacao(
        CreateGraduacaoInput input,
        string exceptionMessage
    )
    {
        var context = _fixture.CreateDbContext();
        var useCase = new CreateGraduacao(
            new UnitOfWork(context),
            new GraduacaoRepository(context)
        );

        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(exceptionMessage);
    }
}
