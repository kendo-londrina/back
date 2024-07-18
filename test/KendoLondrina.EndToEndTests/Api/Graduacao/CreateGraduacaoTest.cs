using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using KenLo.Application.UseCases.Graduacao;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KenLo.EndToEndTests.Api.Graduacao;

[Collection(nameof(CreateGraduacaoFixture))]
public class CreateGraduacaoTest
{
    private readonly CreateGraduacaoFixture _fixture;

    public CreateGraduacaoTest(CreateGraduacaoFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact()]
    public async Task CreateGraduacao()
    {
        var input = _fixture.getExampleInput();

        var (response, output) = await _fixture
            .ApiClient.Post<GraduacaoModelOutput>(
                "/graduacoes",
                input
            );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.Created);

        output.Should().NotBeNull();
        output!.Nome.Should().Be(input.Nome);
        output.Descricao.Should().Be(input.Descricao);
        output.Ativo.Should().Be(input.Ativo);
        output.Id.Should().NotBeEmpty();
        output.CriadoEm.Should().NotBeSameDateAs(default);

        var dbGraduacao = await Base.BaseFixture.CreateDbContext()
            .Graduacoes.FindAsync(output.Id);
        dbGraduacao.Should().NotBeNull();
        dbGraduacao!.Nome.Should().Be(input.Nome);
        dbGraduacao.Descricao.Should().Be(input.Descricao);
        dbGraduacao.Ativo.Should().Be(input.Ativo);
        dbGraduacao.Id.Should().NotBeEmpty();
        dbGraduacao.CriadoEm.Should().NotBeSameDateAs(default);
    }

    [Theory(DisplayName = nameof(ErrorWhenCantInstantiateAggregate))]
    [MemberData(
        nameof(CreateGraduacaoDataGenerator.GetInvalidInputs),
        MemberType = typeof(CreateGraduacaoDataGenerator)
    )]
    public async Task ErrorWhenCantInstantiateAggregate(
        CreateGraduacaoInput input,
        string expectedDetail
    ){
        var (response, output) = await _fixture.
            ApiClient.Post<ProblemDetails>(
                "/graduacoes",
                input
            );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        output.Should().NotBeNull();
        output!.Title.Should().Be("One or more validation errors ocurred");
        output.Type.Should().Be("UnprocessableEntity");
        output.Status.Should().Be((int)StatusCodes.Status422UnprocessableEntity);
        output.Detail.Should().Be(expectedDetail);
    }
}
