using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using KenLo.Application.Exceptions;
using KenLo.Infra.Data.EF;
using KenLo.Infra.Data.EF.Repositories;
using Xunit;

namespace KenLo.IntegrationTests.Infra.Data.EF.Repositories;

[Collection(nameof(GraduacaoRepositoryFixture))]
public class GraduacaoRepositoryTest
{
    private readonly GraduacaoRepositoryFixture _fixture;

    public GraduacaoRepositoryTest(GraduacaoRepositoryFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Create))]
    public async Task Create()
    {
        // Arrange
        KendoLondrinaDbContext dbContext = _fixture.CreateDbContext();
        var graduacaoExemplo = _fixture.GetGraduacaoValida();
        var graduacaoRepository = new GraduacaoRepository(dbContext);

        // Act
        await graduacaoRepository.Create(graduacaoExemplo, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        // Assert
        var dbGraduacao = await (_fixture.CreateDbContext(true))
            .Graduacoes.FindAsync(graduacaoExemplo.Id);
        dbGraduacao.Should().NotBeNull();
        dbGraduacao!.Nome.Should().Be(graduacaoExemplo.Nome);
        dbGraduacao.Descricao.Should().Be(graduacaoExemplo.Descricao);
        dbGraduacao.Ativo.Should().Be(graduacaoExemplo.Ativo);
        dbGraduacao.CriadoEm.Should().Be(graduacaoExemplo.CriadoEm);
    }

    [Fact(DisplayName = nameof(Read))]
    public async Task Read()
    {
        // Arrange
        KendoLondrinaDbContext dbContext = _fixture.CreateDbContext();
        var graduacao = _fixture.GetGraduacaoValida();
        var graduacoes = _fixture.GetGraduacaoList(15);
        graduacoes.Add(graduacao);
        await dbContext.AddRangeAsync(graduacoes);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var graduacaoRepository = new GraduacaoRepository(
            _fixture.CreateDbContext(true)
        );

        // Act
        var dbGraduacao = await graduacaoRepository.Read(
            graduacao.Id, 
            CancellationToken.None);

        // Assert
        dbGraduacao.Should().NotBeNull();
        dbGraduacao!.Nome.Should().Be(graduacao.Nome);
        dbGraduacao.Id.Should().Be(graduacao.Id);
        dbGraduacao.Descricao.Should().Be(graduacao.Descricao);
        dbGraduacao.Ativo.Should().Be(graduacao.Ativo);
        dbGraduacao.CriadoEm.Should().Be(graduacao.CriadoEm);
    }

    [Fact(DisplayName = nameof(Update))]
    public async Task Update()
    {
        // Arrange
        KendoLondrinaDbContext dbContext = _fixture.CreateDbContext();
        var graduacao = _fixture.GetGraduacaoValida();
        var graduacaoRepository = new GraduacaoRepository(dbContext);
        await graduacaoRepository.Create(graduacao, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        // Act
        var nomeAlterado = _fixture.GetNomeValido();
        var descricaoAlterada = _fixture.GetDescricaoValida();
        graduacao.Update(nomeAlterado, descricaoAlterada);
        await graduacaoRepository.Update(graduacao, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        // Assert
        var dbGraduacao = await (_fixture.CreateDbContext(true))
            .Graduacoes.FindAsync(graduacao.Id);
        dbGraduacao.Should().NotBeNull();
        dbGraduacao!.Id.Should().Be(graduacao.Id);
        dbGraduacao!.Nome.Should().Be(nomeAlterado);
        dbGraduacao.Descricao.Should().Be(descricaoAlterada);
    }

    [Fact(DisplayName = nameof(Delete))]
    public async Task Delete()
    {
        // Arrange
        KendoLondrinaDbContext dbContext = _fixture.CreateDbContext();
        var graduacao = _fixture.GetGraduacaoValida();
        var graduacaoRepository = new GraduacaoRepository(dbContext);
        await graduacaoRepository.Create(graduacao, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        // Act
        await graduacaoRepository.Delete(graduacao, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        // Assert
       var dbGraduacao = await (_fixture.CreateDbContext(true))
            .Graduacoes.FindAsync(graduacao.Id);
        dbGraduacao.Should().BeNull();
    }

    [Fact(DisplayName = nameof(GetThrowIfNotFound))]
    public async Task GetThrowIfNotFound()
    {
        KendoLondrinaDbContext dbContext = _fixture.CreateDbContext();
        var exampleId = Guid.NewGuid();
        await dbContext.AddRangeAsync(_fixture.GetGraduacaoList(15));
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var graduacaoRepository = new GraduacaoRepository(dbContext);

        var task = async () => await graduacaoRepository.Read(
            exampleId,
            CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Graduacao {exampleId} not found");
    }
}
