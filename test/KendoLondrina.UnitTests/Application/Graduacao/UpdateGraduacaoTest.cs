using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using KenLo.Application.Exceptions;
using KenLo.Application.UseCases.Graduacao;
using KenLo.Domain.Exceptions;
using Moq;
using domain = KenLo.Domain.Entity;

namespace KenLo.UnitTests.Application.Graduacao;

[Collection(nameof(UpdateGraduacaoFixture))]
public class UpdateGraduacaoTest
{
    private readonly UpdateGraduacaoFixture _fixture;
    public UpdateGraduacaoTest(UpdateGraduacaoFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory(DisplayName = nameof(UpdateGraduacao))]
    [Trait("Application", "UpdateGraduacao - Use Cases")]
    [MemberData(
        nameof(UpdateGraduacaoDataGenerator.GetGraduacoes),
        parameters: 10,
        MemberType = typeof(UpdateGraduacaoDataGenerator)
    )]
    public async Task UpdateGraduacao(
        domain.Graduacao graduacaoExemplo,
        UpdateGraduacaoInput input)
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        repositoryMock.Setup(x => x.Read(
            graduacaoExemplo.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(graduacaoExemplo);        
        var useCase = new UpdateGraduacao(
            unitOfWorkMock.Object, repositoryMock.Object
        );

        GraduacaoModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Nome.Should().Be(input.Nome);
        output.Descricao.Should().Be(input.Descricao);
        output.Ativo.Should().Be((bool)input.Ativo!);
        repositoryMock.Verify(x => x.Read(
            graduacaoExemplo.Id,
            It.IsAny<CancellationToken>())
        , Times.Once);
        repositoryMock.Verify(x => x.Update(
            graduacaoExemplo,
            It.IsAny<CancellationToken>())
        , Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(
            It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Theory(DisplayName = nameof(UpdateGraduacaoApenasNome))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateGraduacaoDataGenerator.GetGraduacoes),
        parameters: 10,
        MemberType = typeof(UpdateGraduacaoDataGenerator)
    )]
    public async Task UpdateGraduacaoApenasNome(
        domain.Graduacao graduacaoExemplo,
        UpdateGraduacaoInput inputExemplo)
    {
        var input = new UpdateGraduacaoInput(
            inputExemplo.Id,
            inputExemplo.Nome
        );        
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        repositoryMock.Setup(x => x.Read(
            graduacaoExemplo.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(graduacaoExemplo);        
        var useCase = new UpdateGraduacao(
            unitOfWorkMock.Object, repositoryMock.Object
        );

        GraduacaoModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Nome.Should().Be(input.Nome);
        output.Descricao.Should().Be(graduacaoExemplo.Descricao);
        output.Ativo.Should().Be(graduacaoExemplo.Ativo!);
        repositoryMock.Verify(x => x.Read(
            graduacaoExemplo.Id,
            It.IsAny<CancellationToken>())
        , Times.Once);
        repositoryMock.Verify(x => x.Update(
            graduacaoExemplo,
            It.IsAny<CancellationToken>())
        , Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(
            It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact(DisplayName = nameof(NotFoundExceptionQuandoNaoEncontrarGraduacao))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    public async Task NotFoundExceptionQuandoNaoEncontrarGraduacao()
    {
        var input = _fixture.GetInput();
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        repositoryMock.Setup(x => x.Read(
            input.Id,
            It.IsAny<CancellationToken>())
        ).ThrowsAsync(new NotFoundException(""));
        var useCase = new UpdateGraduacao(
            unitOfWorkMock.Object, repositoryMock.Object
        );

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();
        repositoryMock.Verify(x => x.Read(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }

    [Theory(DisplayName = nameof(EntityValidationExceptionQuandoInputInvalido))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateGraduacaoDataGenerator.GetInvalidInputs),
        parameters: 12,
        MemberType = typeof(UpdateGraduacaoDataGenerator)
    )]
    public async Task EntityValidationExceptionQuandoInputInvalido(
        UpdateGraduacaoInput input,
        string expectedExceptionMessage
    )
    {
        var graduacaoExemplo = _fixture.GetGraduacao();
        input.Id = graduacaoExemplo.Id;
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        repositoryMock.Setup(x => x.Read(
            graduacaoExemplo.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(graduacaoExemplo);
        var useCase = new UpdateGraduacao(
            unitOfWorkMock.Object,
            repositoryMock.Object
        );

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>()
            .WithMessage(expectedExceptionMessage);
        repositoryMock.Verify(x => x.Read(
            graduacaoExemplo.Id,
            It.IsAny<CancellationToken>()),
        Times.Once);
    }
}
