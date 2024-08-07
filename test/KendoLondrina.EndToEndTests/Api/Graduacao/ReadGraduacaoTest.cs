using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using KenLo.Api.ApiModels.Response;
using KenLo.Application.UseCases.Graduacao;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KenLo.EndToEndTests.Api.Graduacao;

[Collection(nameof(ReadGraduacaoFixture))]
public class ReadGraduacaoTest
    : IDisposable
{
    private readonly ReadGraduacaoFixture _fixture;

    public ReadGraduacaoTest(ReadGraduacaoFixture fixture) 
        => _fixture = fixture;

    public void Dispose()
        => _fixture.CleanPersistence();

    [Fact(DisplayName = nameof(ReadGraduacao))]
    public async Task ReadGraduacao()
    {
        var graduacoes = _fixture.GetGraduacoes(20);
        await _fixture.Persistence.InsertList(graduacoes);
        var graduacao = graduacoes[10];

        var (response, output) = await _fixture.ApiClient
            .Get<GraduacaoModelOutput>(
                $"/graduacoes/{graduacao.Id}"
            );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Id.Should().Be(graduacao.Id);
        output.Nome.Should().Be(graduacao.Nome);
        output.Descricao.Should().Be(graduacao.Descricao);
        output.Ativo.Should().Be(graduacao.Ativo);        
        output.CriadoEm.Should().Be(graduacao.CriadoEm);

        // var (response, output) = await _fixture.ApiClient
        //     .Get<ApiResponse<GraduacaoModelOutput>>(
        //         $"/graduacoes/{graduacao.Id}"
        //     );

        // response.Should().NotBeNull();
        // response!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status200OK);
        // output.Should().NotBeNull();
        // output!.Data.Should().NotBeNull();
        // output.Data.Id.Should().Be(graduacao.Id);
        // output.Data.Nome.Should().Be(graduacao.Nome);
        // output.Data.Descricao.Should().Be(graduacao.Descricao);
        // output.Data.Ativo.Should().Be(graduacao.Ativo);
        // output.Data.CriadoEm.TrimMillisseconds().Should().Be(
        //     graduacao.CriadoEm.TrimMillisseconds()
        // );
    }

    [Fact(DisplayName = nameof(ErrorWhenNotFound))]
    [Trait("EndToEnd/API", "Category/Get - Endpoints")]
    public async Task ErrorWhenNotFound()
    {
        var graduacoes = _fixture.GetGraduacoes(20);
        await _fixture.Persistence.InsertList(graduacoes);
        var randomGuid = Guid.NewGuid();

        var (response, output) = await _fixture.ApiClient.Get<ProblemDetails>(
            $"/graduacoes/{randomGuid}"
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
        output.Should().NotBeNull();
        output!.Status.Should().Be((int)StatusCodes.Status404NotFound);
        output.Type.Should().Be("NotFound");
        output.Title.Should().Be("Not Found");
        output.Detail.Should().Be($"Graduacao {randomGuid} not found");
    }

}