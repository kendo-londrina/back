using System;
using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KenLo.EndToEndTests.Api.Graduacao;

[Collection(nameof(DeleteGraduacaoFixture))]
public class DeleteGraduacaoTest : IDisposable
{
    private readonly DeleteGraduacaoFixture _fixture;

    public DeleteGraduacaoTest(DeleteGraduacaoFixture fixture) 
        => _fixture = fixture;
    public void Dispose()
        => _fixture.CleanPersistence();

    [Fact(DisplayName = nameof(DeleteGraduacao))]
    public async void DeleteGraduacao()
    {
        var graduacoes = _fixture.GetGraduacoes(20);
        await _fixture.Persistence.InsertList(graduacoes);
        var graduacao = graduacoes[10];

        var (response, output) = await _fixture.ApiClient.Delete<object>(
            $"/graduacoes/{graduacao.Id}"
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should()
            .Be((HttpStatusCode)StatusCodes.Status204NoContent);
        output.Should().BeNull();
        var persistenceGraduacao = await _fixture.Persistence
            .GetById(graduacao.Id);
        persistenceGraduacao.Should().BeNull();
    }

    [Fact(DisplayName = nameof(ErrorWhenNotFound))]
    public async void ErrorWhenNotFound()
    {
        var graduacoes = _fixture.GetGraduacoes(20);
        await _fixture.Persistence.InsertList(graduacoes);
        var randomGuid = Guid.NewGuid();

        var (response, output) = await _fixture.ApiClient.Delete<ProblemDetails>(
            $"/graduacoes/{randomGuid}"
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should()
            .Be((HttpStatusCode)StatusCodes.Status404NotFound);
        output.Should().NotBeNull();
        output!.Title.Should().Be("Not Found");
        output!.Type.Should().Be("NotFound");
        output!.Status.Should().Be((int)StatusCodes.Status404NotFound);
        output!.Detail.Should().Be($"Graduacao {randomGuid} not found");
    }

}