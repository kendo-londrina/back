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

[Collection(nameof(DeleteGraduacaoFixture))]
public class DeleteGraduacaoTest
{
    private readonly DeleteGraduacaoFixture _fixture;
    public DeleteGraduacaoTest(DeleteGraduacaoFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteGraduacao))]
    [Trait("Application", "DeleteGraduacao - Use Cases")]
    public async Task DeleteGraduacao()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var graduacaoExemplo = _fixture.GetGraduacao();
        repositoryMock.Setup(x => x.Read(
            graduacaoExemplo.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(graduacaoExemplo);        
        var input = new DeleteGraduacaoInput(graduacaoExemplo.Id);
        var useCase = new DeleteGraduacao(
            unitOfWorkMock.Object, repositoryMock.Object
        );

        await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(x => x.Read(
            graduacaoExemplo.Id,
            It.IsAny<CancellationToken>()
        ), Times.Once);
        repositoryMock.Verify(x => x.Delete(
            graduacaoExemplo,
            It.IsAny<CancellationToken>()
        ), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(
            It.IsAny<CancellationToken>()
        ), Times.Once);        

    }

}
