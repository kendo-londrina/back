using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using KenLo.Application.UseCases.Graduacao;
using KenLo.Domain.SeedWork.SearchableRepository;
using KenLo.Infra.Data.EF;
using KenLo.Infra.Data.EF.Repositories;

namespace KenLo.IntegrationTests.Application.Usecases.Graduacao;

[Collection(nameof(ListGraduacaoFixture))]
public class ListGraduacaoTest
{
    private readonly ListGraduacaoFixture _fixture;

    public ListGraduacaoTest(ListGraduacaoFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(SearchRetursEmptyWhenPersistenceIsEmpty))]
    public async Task SearchRetursEmptyWhenPersistenceIsEmpty()
    {
        KendoLondrinaDbContext dbContext = _fixture.CreateDbContext();
        var graduacaoRepository = new GraduacaoRepository(dbContext);
        var input = new ListGraduacaoInput(1, 20);

        var useCase = new ListGraduacao(graduacaoRepository);

        // Act
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
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
        var input = new ListGraduacaoInput();
        var useCase = new ListGraduacao(graduacaoRepository);

        // Act
        var output = await useCase.Handle(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(graduacoes.Count);
        output.Items.Should().HaveCount(graduacoes.Count);

        foreach (GraduacaoModelOutput outputItem in output.Items)
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
        var input = new ListGraduacaoInput(page, perPage, "", "", SearchOrder.Asc);
        var useCase = new ListGraduacao(graduacaoRepository);

        // Act
        var output = await useCase.Handle(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(quantityToGenerate);
        output.Items.Should().HaveCount(expectedQuantityItems);

        foreach (GraduacaoModelOutput outputItem in output.Items)
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
        var input = new ListGraduacaoInput(page, perPage, search, "", SearchOrder.Asc);
        var useCase = new ListGraduacao(graduacaoRepository);

        // Act
        var output = await useCase.Handle(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(expectedQuantityTotal);
        output.Items.Should().HaveCount(expectedQuantityItems);

        foreach (GraduacaoModelOutput outputItem in output.Items)
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
        var input = new ListGraduacaoInput(1, 20, "", orderBy, searchOrder);
        var expectedOrderedList = _fixture.CloneGraduacaoListOrdered(
            graduacoes,
            orderBy,
            searchOrder
        );
        var useCase = new ListGraduacao(graduacaoRepository);

        // Act
        var output = await useCase.Handle(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
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
