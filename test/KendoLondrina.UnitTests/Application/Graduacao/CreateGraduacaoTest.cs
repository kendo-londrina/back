using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using KenLo.Application.UseCases.Graduacao;
using KenLo.Domain.Exceptions;
using Moq;
using domain = KenLo.Domain.Entity;

namespace KenLo.UnitTests.Application.Graduacao;

[Collection(nameof(CreateGraduacaoFixture))]
public class CreateGraduacaoTest
{
    private readonly CreateGraduacaoFixture _fixture;

    public CreateGraduacaoTest(CreateGraduacaoFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CreateGraduacao))]
    [Trait("Application", "CreateGraduacao - Use Cases")]
    public async void CreateGraduacao()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var useCase = new CreateGraduacao(
            unitOfWorkMock.Object, repositoryMock.Object
        );
        var input = _fixture.GetInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Create(
                It.IsAny<domain.Graduacao>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
        unitOfWorkMock.Verify(
            uow => uow.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );
        output.Should().NotBeNull();
        output.Nome.Should().Be(input.Nome);
        output.Descricao.Should().Be(input.Descricao);
        output.Ativo.Should().Be(input.Ativo);
        output.Id.Should().NotBeEmpty();
        output.CriadoEm.Should().NotBeSameDateAs(default);
    }

    [Fact(DisplayName = nameof(CreateGraduacaoApenasComNome))]
    [Trait("Application", "CreateGraduacao - Use Cases")]
    public async void CreateGraduacaoApenasComNome()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var useCase = new CreateGraduacao(
            unitOfWorkMock.Object, repositoryMock.Object
        );
        var input = new CreateGraduacaoInput(
            _fixture.GetNomeValido()
        );

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Create(
                It.IsAny<domain.Graduacao>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
        unitOfWorkMock.Verify(
            uow => uow.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );
        output.Should().NotBeNull();
        output.Nome.Should().Be(input.Nome);
        output.Descricao.Should().Be("");
        output.Ativo.Should().BeTrue();
        output.Id.Should().NotBeEmpty();
        output.CriadoEm.Should().NotBeSameDateAs(default);
    }

    [Fact(DisplayName = nameof(CreateGraduacaoApenasComNomeDescricao))]
    [Trait("Application", "CreateGraduacao - Use Cases")]
    public async void CreateGraduacaoApenasComNomeDescricao()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var useCase = new CreateGraduacao(
            unitOfWorkMock.Object, repositoryMock.Object
        );
        var input = new CreateGraduacaoInput(
            _fixture.GetNomeValido(),
            _fixture.GetDescricaoValida()
        );

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Create(
                It.IsAny<domain.Graduacao>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
        unitOfWorkMock.Verify(
            uow => uow.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );
        output.Should().NotBeNull();
        output.Nome.Should().Be(input.Nome);
        output.Descricao.Should().Be(input.Descricao);
        output.Ativo.Should().BeTrue();
        output.Id.Should().NotBeEmpty();
        output.CriadoEm.Should().NotBeSameDateAs(default);
    }

    [Theory(DisplayName = nameof(ExceptionQuandoNaoInstanciarGraduacao))]
    [Trait("Application", "CreateGraduacao - Use Cases")]
    [MemberData(
        nameof(CreateGraduacaoDataGenerator.GetInvalidInputs),
        parameters: 24,
        MemberType = typeof(CreateGraduacaoDataGenerator)
    )]
    public async void ExceptionQuandoNaoInstanciarGraduacao(
        CreateGraduacaoInput input,
        string exceptionMessage
    )
    {
        var useCase = new CreateGraduacao(
            _fixture.GetUnitOfWorkMock().Object,
            _fixture.GetRepositoryMock().Object
        );

        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(exceptionMessage);
    }
}
