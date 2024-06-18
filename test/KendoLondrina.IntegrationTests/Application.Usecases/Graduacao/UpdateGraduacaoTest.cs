using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using KenLo.Application.Exceptions;
using KenLo.Application.UseCases.Graduacao;
using KenLo.Domain.Exceptions;
using KenLo.Infra.Data.EF;
using KenLo.Infra.Data.EF.Repositories;
using domain = KenLo.Domain.Entity;

namespace KenLo.IntegrationTests.Application.Usecases.Graduacao;

[Collection(nameof(UpdateGraduacaoFixture))]
public class UpdateGraduacaoTest
{
    private readonly UpdateGraduacaoFixture _fixture;
    public UpdateGraduacaoTest(UpdateGraduacaoFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory(DisplayName = nameof(UpdateGraduacao))]
    [MemberData(
        nameof(UpdateGraduacaoDataGenerator.GetGraduacoes),
        parameters: 12,
        MemberType = typeof(UpdateGraduacaoDataGenerator)
    )]
    public async Task UpdateGraduacao(
        domain.Graduacao graduacaoExemplo,
        UpdateGraduacaoInput input)
    {
        var arrangeContext = _fixture.CreateDbContext();
        await arrangeContext.Graduacoes.AddAsync(graduacaoExemplo);
        await arrangeContext.SaveChangesAsync();
        var actionContext = _fixture.CreateDbContext(true);
        var repository = new GraduacaoRepository(actionContext);
        var unitOfWork = new UnitOfWork(actionContext);
        var useCase = new UpdateGraduacao(
            unitOfWork, repository
        );

        GraduacaoModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Nome.Should().Be(input.Nome);
        output.Descricao.Should().Be(input.Descricao);
        output.Ativo.Should().Be((bool)input.Ativo!);

        var dbGraduacao = await _fixture.CreateDbContext(true)
            .Graduacoes.FindAsync(output.Id);
        dbGraduacao.Should().NotBeNull();
        dbGraduacao!.Nome.Should().Be(input.Nome);
        dbGraduacao.Descricao.Should().Be(input.Descricao);
        dbGraduacao.Ativo.Should().Be(input.Ativo.Value);
        dbGraduacao.CriadoEm.Should().Be(output.CriadoEm);        
    }

    [Theory(DisplayName = nameof(UpdateGraduacaoApenasNome))]
    [MemberData(
        nameof(UpdateGraduacaoDataGenerator.GetGraduacoes),
        parameters: 10,
        MemberType = typeof(UpdateGraduacaoDataGenerator)
    )]
    public async Task UpdateGraduacaoApenasNome(
        domain.Graduacao graduacaoExemplo,
        UpdateGraduacaoInput inputExemplo)
    {
        var arrangeContext = _fixture.CreateDbContext();
        await arrangeContext.Graduacoes.AddAsync(graduacaoExemplo);
        await arrangeContext.SaveChangesAsync();
        var actionContext = _fixture.CreateDbContext(true);
        var repository = new GraduacaoRepository(actionContext);
        var unitOfWork = new UnitOfWork(actionContext);

        var input = new UpdateGraduacaoInput(
            inputExemplo.Id,
            inputExemplo.Nome
            // apenas nome sem descricao
        );        
        var useCase = new UpdateGraduacao(unitOfWork, repository);
        GraduacaoModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Nome.Should().Be(input.Nome);
        output.Descricao.Should().Be(graduacaoExemplo.Descricao);
        output.Ativo.Should().Be(graduacaoExemplo.Ativo!);
    }

    [Fact(DisplayName = nameof(NotFoundExceptionQuandoNaoEncontrarGraduacao))]
    public async Task NotFoundExceptionQuandoNaoEncontrarGraduacao()
    {
        var input = _fixture.GetInput();
        var context = _fixture.CreateDbContext();
        var unitOfWork = new UnitOfWork(context);
        var repository = new GraduacaoRepository(context);
        var useCase = new UpdateGraduacao(
            unitOfWork, repository
        );

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();
    }

    [Theory(DisplayName = nameof(EntityValidationExceptionQuandoInputInvalido))]
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
        var graduacao = _fixture.GetGraduacao();
        var context = _fixture.CreateDbContext();
        var unitOfWork = new UnitOfWork(context);
        var repository = new GraduacaoRepository(context);
        await repository.Create(graduacao, CancellationToken.None);
        await context.SaveChangesAsync(CancellationToken.None);

        input.Id = graduacao.Id;
        var useCase = new UpdateGraduacao(unitOfWork, repository);

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>()
            .WithMessage(expectedExceptionMessage);
    }
}
