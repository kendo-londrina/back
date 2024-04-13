using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using KenLo.Application.Exceptions;
using KenLo.Application.UseCases.Graduacao;
using Moq;

namespace KenLo.UnitTests.Application.Graduacao;

[Collection(nameof(ReadGraduacaoFixture))]
public class ReadGraduacaoTest
{
    private readonly ReadGraduacaoFixture _fixture;

    public ReadGraduacaoTest(ReadGraduacaoFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ReadGraduacao))]
    [Trait("Application", "ReadGraduacao - Use Cases")]
    public async Task ReadGraduacao()
    {
        var graduacao = _fixture.ObterGraduacao();
        var repositoryMock = _fixture.GetRepositoryMock();
        repositoryMock.Setup(x => x.Read(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(graduacao);
        var input = new ReadGraduacaoInput(graduacao.Id);
        var useCase = new ReadGraduacao(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(x => x.Read(
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

    [Fact(DisplayName = nameof(NotFoundExceptionQuandoGraduacaoNaoExistir))]
    [Trait("Application", "ReadGraduacao - Use Cases")]
    public async Task NotFoundExceptionQuandoGraduacaoNaoExistir()
    {
        var exampleGuid = Guid.NewGuid();
        var repositoryMock = _fixture.GetRepositoryMock();
        repositoryMock.Setup(x => x.Read(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        )).ThrowsAsync(
            new NotFoundException($"Graduacao {exampleGuid} was not found.")
        );
        var input = new ReadGraduacaoInput(exampleGuid);
        var useCase = new ReadGraduacao(repositoryMock.Object);

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();
        repositoryMock.Verify(x => x.Read(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}
