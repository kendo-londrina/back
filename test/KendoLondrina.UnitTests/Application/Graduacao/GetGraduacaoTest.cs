using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using KenLo.Application.Exceptions;
using KenLo.Application.UseCases.Graduacao;
using Moq;

namespace KenLo.UnitTests.Application.Graduacao;

[Collection(nameof(GetGraduacaoFixture))]
public class GetGraduacaoTest
{
    private readonly GetGraduacaoFixture _fixture;

    public GetGraduacaoTest(GetGraduacaoFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GetGraduacao))]
    [Trait("Application", "GetGraduacao - Use Cases")]
    public async Task GetGraduacao()
    {
        var graduacao = _fixture.ObterGraduacao();
        var repositoryMock = _fixture.GetRepositoryMock();
        repositoryMock.Setup(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(graduacao);
        var input = new GetGraduacaoInput(graduacao.Id);
        var useCase = new GetGraduacao(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        ), Times.Once);
        output.Should().NotBeNull();
        output.Nome.Should().Be(graduacao.Nome);
        output.Descricao.Should().Be(graduacao.Descricao);
        output.Ativo.Should().Be(graduacao.Ativo);
        output.Id.Should().Be(graduacao.Id);
        output.CriadoEm.Should().Be(graduacao.CriadoEm);
    }

    [Fact(DisplayName = nameof(NotFoundExceptionWhenGraduacaoDoesntExist))]
    [Trait("Application", "GetCategory - Use Cases")]
    public async Task NotFoundExceptionWhenGraduacaoDoesntExist()
    {
        var exampleGuid = Guid.NewGuid();
        var repositoryMock = _fixture.GetRepositoryMock();
        repositoryMock.Setup(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        )).ThrowsAsync(
            new NotFoundException($"Graduacao {exampleGuid} was not found.")
        );
        var input = new GetGraduacaoInput(exampleGuid);
        var useCase = new GetGraduacao(repositoryMock.Object);

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();
        repositoryMock.Verify(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}
