using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using KenLo.Api.ApiModels.Response;
using KenLo.Application.UseCases.Graduacao;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KenLo.EndToEndTests.Api.Graduacao;

[Collection(nameof(UpdateGraduacaoFixture))]
public class UpdateGraduacaoTest
    : IDisposable
{
    private readonly UpdateGraduacaoFixture _fixture;

    public UpdateGraduacaoTest(UpdateGraduacaoFixture fixture) 
        => _fixture = fixture;

    public void Dispose()
        => _fixture.CleanPersistence();

    [Fact(DisplayName = nameof(UpdateGraduacao))]
    public async Task UpdateGraduacao()
    {
        var graduacoes = _fixture.GetGraduacoes(20);
        await _fixture.Persistence.InsertList(graduacoes);
        var input = _fixture.getExampleInput();
        input.Id = graduacoes[10].Id;

        var (response, output) = await _fixture.ApiClient
            .Put<GraduacaoModelOutput>(
                $"/graduacoes",
                input
            );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Nome.Should().Be(input.Nome);
        output.Descricao.Should().Be(input.Descricao);
        output.Ativo.Should().Be((bool) input.Ativo!);
        var dbGraduacao = await Base.BaseFixture.CreateDbContext()
            .Graduacoes.FindAsync(output.Id);
        dbGraduacao.Should().NotBeNull();
        dbGraduacao!.Nome.Should().Be(input.Nome);
        dbGraduacao.Descricao.Should().Be(input.Descricao);
        dbGraduacao.Ativo.Should().Be((bool)input.Ativo);
    }


    [Fact(DisplayName = nameof(UpdateGraduacaoApenasNome))]
    public async void UpdateGraduacaoApenasNome()
    {
        var graduacoes = _fixture.GetGraduacoes(20);
        await _fixture.Persistence.InsertList(graduacoes);
        var graduacao = graduacoes[10];
        var input = new UpdateGraduacaoInput(
            graduacao.Id,
            _fixture.GetNomeValido()
        );

        var (response, output) = await _fixture.ApiClient.Put<GraduacaoModelOutput>(
            "/graduacoes",
            input
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Id.Should().Be(graduacao.Id);
        output.Nome.Should().Be(input.Nome);
        output.Descricao.Should().Be(graduacao.Descricao);
        output.Ativo.Should().Be((bool)graduacao.Ativo!);
        var dbGraduacao = await _fixture
            .Persistence.GetById(input.Id);
        dbGraduacao.Should().NotBeNull();
        dbGraduacao!.Nome.Should().Be(input.Nome);
        dbGraduacao.Descricao.Should().Be(graduacao.Descricao);
        dbGraduacao.Ativo.Should().Be(graduacao.Ativo);
    }

    [Fact(DisplayName = nameof(UpdateGraduacaoNomeAndDescricao))]
    public async void UpdateGraduacaoNomeAndDescricao()
    {
        var graduacoes = _fixture.GetGraduacoes(20);
        await _fixture.Persistence.InsertList(graduacoes);
        var graduacao = graduacoes[10];
        var input = new UpdateGraduacaoInput(
            graduacao.Id,
            _fixture.GetNomeValido(),
            _fixture.GetDescricaoValida()
        );

        var (response, output) = await _fixture.ApiClient.Put<GraduacaoModelOutput>(
            "/graduacoes",
            input
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Id.Should().Be(graduacao.Id);
        output.Nome.Should().Be(input.Nome);
        output.Descricao.Should().Be(input.Descricao);
        output.Ativo.Should().Be((bool)graduacao.Ativo!);
        var dbGraduacao = await _fixture
            .Persistence.GetById(input.Id);
        dbGraduacao.Should().NotBeNull();
        dbGraduacao!.Nome.Should().Be(input.Nome);
        dbGraduacao.Descricao.Should().Be(input.Descricao);
        dbGraduacao.Ativo.Should().Be(graduacao.Ativo);
    }

    [Fact(DisplayName = nameof(ErrorWhenNotFound))]
    public async Task ErrorWhenNotFound()
    {
        var graduacoes = _fixture.GetGraduacoes(20);
        await _fixture.Persistence.InsertList(graduacoes);
        var input = _fixture.getExampleInput();
        input.Id = Guid.NewGuid();

        var (response, output) = await _fixture.ApiClient
            .Put<ProblemDetails>(
                $"/graduacoes",
                input
            );        

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
        output.Should().NotBeNull();
        output!.Status.Should().Be((int)StatusCodes.Status404NotFound);
        output.Type.Should().Be("NotFound");
        output.Title.Should().Be("Not Found");
        output.Detail.Should().Be($"Graduacao {input.Id} not found");
    }

    [Theory(DisplayName = nameof(ErrorWhenCantInstantiateAggregate))]
    [MemberData(
        nameof(UpdateGraduacaoDataGenerator.GetInvalidInputs),
        MemberType = typeof(UpdateGraduacaoDataGenerator)
    )]
    public async void ErrorWhenCantInstantiateAggregate(
        UpdateGraduacaoInput input,
        string expectedDetail
    )
    {
        var graduacoes = _fixture.GetGraduacoes(20);
        await _fixture.Persistence.InsertList(graduacoes);
        input.Id = graduacoes[10].Id;

        var (response, output) = await _fixture.ApiClient.Put<ProblemDetails>(
            "/graduacoes",
            input
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status422UnprocessableEntity);
        output.Should().NotBeNull();
        output!.Title.Should().Be("One or more validation errors ocurred");
        output.Type.Should().Be("UnprocessableEntity");
        output.Status.Should().Be((int)StatusCodes.Status422UnprocessableEntity);
        output.Detail.Should().Be(expectedDetail);
    }
}
