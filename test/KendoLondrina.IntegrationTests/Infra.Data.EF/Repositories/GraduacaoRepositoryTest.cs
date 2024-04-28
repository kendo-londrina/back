using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using KenLo.Application.Exceptions;
using KenLo.Domain.Entity;
using KenLo.Domain.SeedWork.SearchableRepository;
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

    [Fact(DisplayName = nameof(ListByIds))]
    public async Task ListByIds()
    {
        // Arrange
        var quantityToGenerate = 15;
        var dbContext = _fixture.CreateDbContext();
        var graduacoes = _fixture.GetGraduacaoList(quantityToGenerate);
        await dbContext.AddRangeAsync(graduacoes);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        // Act
        var graduacaoRepository = new GraduacaoRepository(_fixture.CreateDbContext(true));
        var graduacaoList = await graduacaoRepository.ListarGraduacoesPorIds(
            graduacoes.Select(g => g.Id).ToList(),
            CancellationToken.None);

        // Assert
        graduacaoList.Should().NotBeNull();
        graduacaoList.Should().HaveCount(quantityToGenerate);

        foreach (var item in graduacaoList)
        {
            var graduacao = graduacoes.Find(i => i.Id == item.Id);
            item.Nome.Should().Be(graduacao!.Nome);
            item.Id.Should().Be(graduacao.Id);
            item.Descricao.Should().Be(graduacao.Descricao);
            item.Ativo.Should().Be(graduacao.Ativo);
            item.CriadoEm.Should().Be(graduacao.CriadoEm);
        }
    }

    [Fact(DisplayName = nameof(SearchRetursEmptyWhenPersistenceIsEmpty))]
    public async Task SearchRetursEmptyWhenPersistenceIsEmpty()
    {
        KendoLondrinaDbContext dbContext = _fixture.CreateDbContext();
        var graduacaoRepository = new GraduacaoRepository(dbContext);
        var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

        var output = await graduacaoRepository.Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);
    }
    
    [Fact(DisplayName = nameof(SearchRetursListAndTotal))]
    public async Task SearchRetursListAndTotal()
    {
        // Arrange
        KendoLondrinaDbContext dbContext = _fixture.CreateDbContext();
        var graduacoes = _fixture.GetGraduacaoList(15);
        await dbContext.AddRangeAsync(graduacoes);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var graduacaoRepository = new GraduacaoRepository(
            _fixture.CreateDbContext(true)
        );
        var input = new SearchInput(1, 20, "", "", SearchOrder.Asc);

        // Act
        var output = await graduacaoRepository.Search(
            input,
            CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(graduacoes.Count);
        output.Items.Should().HaveCount(graduacoes.Count);

        foreach (Graduacao outputItem in output.Items)
        {
            var graduacao = graduacoes.Find(i => i.Id == outputItem.Id);
            outputItem.Nome.Should().Be(graduacao!.Nome);
            outputItem.Id.Should().Be(graduacao.Id);
            outputItem.Descricao.Should().Be(graduacao.Descricao);
            outputItem.Ativo.Should().Be(graduacao.Ativo);
            outputItem.CriadoEm.Should().Be(graduacao.CriadoEm);
        }
    }

    [Theory(DisplayName = nameof(SearchRetursPaginated))]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]
    public async Task SearchRetursPaginated(
        int quantityToGenerate,
        int page,
        int perPage,
        int expectedQuantityItems
    )
    {
        // Arrange
        KendoLondrinaDbContext dbContext = _fixture.CreateDbContext();
        var graduacoes = _fixture.GetGraduacaoList(quantityToGenerate);
        await dbContext.AddRangeAsync(graduacoes);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var graduacaoRepository = new GraduacaoRepository(
            _fixture.CreateDbContext(true)
        );
        var input = new SearchInput(page, perPage, "", "", SearchOrder.Asc);

        // Act
        var output = await graduacaoRepository.Search(
            input,
            CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(quantityToGenerate);
        output.Items.Should().HaveCount(expectedQuantityItems);

        foreach (Graduacao outputItem in output.Items)
        {
            var graduacao = graduacoes.Find(i => i.Id == outputItem.Id);
            outputItem.Nome.Should().Be(graduacao!.Nome);
            outputItem.Id.Should().Be(graduacao.Id);
            outputItem.Descricao.Should().Be(graduacao.Descricao);
            outputItem.Ativo.Should().Be(graduacao.Ativo);
            outputItem.CriadoEm.Should().Be(graduacao.CriadoEm);
        }
    }

    [Theory(DisplayName = nameof(SearchByText))]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 3, 3)]
    [InlineData("Horror", 2, 5, 0, 3)]
    [InlineData("Sci-fi", 1, 5, 4, 4)]
    [InlineData("Sci-fi", 1, 2, 2, 4)]
    [InlineData("Sci-fi", 2, 3, 1, 4)]
    [InlineData("Sci-fi Other", 1, 3, 0, 0)]
    [InlineData("Robots", 1, 5, 2, 2)]
    public async Task SearchByText(
        string search,
        int page,
        int perPage,
        int expectedQuantityItems,
        int expectedQuantityTotal
    ) {
        // Arrange
        var dbContext = _fixture.CreateDbContext();
        var graduacoes = _fixture.GetGraduacaoListComNomes(
            new List<string>() { 
                "Action",
                "Horror",
                "Horror - Robots",
                "Horror - Based on Real Facts",
                "Drama",
                "Sci-fi IA",
                "Sci-fi Space",
                "Sci-fi Robots",
                "Sci-fi Future"
            });
        await dbContext.AddRangeAsync(graduacoes);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var graduacaoRepository = new GraduacaoRepository(dbContext);
        var input = new SearchInput(page, perPage, search, "", SearchOrder.Asc);

        // Act
        var output = await graduacaoRepository.Search(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(expectedQuantityTotal);
        output.Items.Should().HaveCount(expectedQuantityItems);

        foreach (Graduacao outputItem in output.Items)
        {
            var graduacao = graduacoes.Find(i => i.Id == outputItem.Id);
            outputItem.Nome.Should().Be(graduacao!.Nome);
            outputItem.Id.Should().Be(graduacao.Id);
            outputItem.Descricao.Should().Be(graduacao.Descricao);
            outputItem.Ativo.Should().Be(graduacao.Ativo);
            outputItem.CriadoEm.Should().Be(graduacao.CriadoEm);
        }        
    }

    [Theory(DisplayName = nameof(SearchOrdered))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("createdAt", "asc")]
    [InlineData("createdAt", "desc")]
    [InlineData("", "asc")]
    public async Task SearchOrdered(
        string orderBy,
        string order
    ){
        // Arrange
        KendoLondrinaDbContext dbContext = _fixture.CreateDbContext();
        var graduacoes = _fixture.GetGraduacaoList(10);
        await dbContext.AddRangeAsync(graduacoes);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var graduacaoRepository = new GraduacaoRepository(
            _fixture.CreateDbContext(true)
        );
        var searchOrder = order.ToLower() == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var input = new SearchInput(1, 20, "", orderBy, searchOrder);
        var expectedOrderedList = _fixture.CloneGraduacaoListOrdered(
            graduacoes,
            orderBy,
            searchOrder
        );

        // Act
        var output = await graduacaoRepository.Search(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(graduacoes.Count);
        output.Items.Should().HaveCount(graduacoes.Count);
        for(int indice = 0; indice < expectedOrderedList.Count; indice++)
        {
            var expectedItem = expectedOrderedList[indice];
            var outputItem = output.Items[indice];
            expectedItem.Should().NotBeNull();
            outputItem.Should().NotBeNull();
            outputItem.Nome.Should().Be(expectedItem!.Nome);
            outputItem.Id.Should().Be(expectedItem.Id);
            outputItem.Descricao.Should().Be(expectedItem.Descricao);
            outputItem.Ativo.Should().Be(expectedItem.Ativo);
            outputItem.CriadoEm.Should().Be(expectedItem.CriadoEm);
        }
    }
}
