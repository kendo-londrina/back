using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using KenLo.Application.UseCases.Graduacao;
using KenLo.Domain.SeedWork.SearchableRepository;
using Moq;
using entity = KenLo.Domain.Entity;

namespace KenLo.UnitTests.Application.Graduacao;

[Collection(nameof(ListGraduacaoFixture))]
public class ListGraduacaoTest
{
    private readonly ListGraduacaoFixture _fixture;
    public ListGraduacaoTest(ListGraduacaoFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(List))]
    public async Task List()
    {
        // Arrange
        var graduacoes = _fixture.GetGraduacaoList();
        var repositoryMock = _fixture.GetRepositoryMock();
        var input = _fixture.GetInput();
        var searchOutput = new SearchOutput<entity.Graduacao>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: (IReadOnlyList<entity.Graduacao>) graduacoes,
            total: new Random().Next(50, 200)
        );
        repositoryMock.Setup(x => x.List(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(searchOutput);
        var useCase = new ListGraduacao(repositoryMock.Object);

        // Act
        var output = await useCase.Handle(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Page.Should().Be(searchOutput.CurrentPage);
        output.PerPage.Should().Be(searchOutput.PerPage);
        output.Total.Should().Be(searchOutput.Total);
        output.Items.Should().HaveCount(searchOutput.Items.Count);
        ((List<GraduacaoModelOutput>)output.Items).ForEach(outputItem =>
        {
            var repositoryCategory = searchOutput.Items
                .FirstOrDefault(x => x.Id == outputItem.Id);
            outputItem.Should().NotBeNull();
            outputItem.Nome.Should().Be(repositoryCategory!.Nome);
            outputItem.Descricao.Should().Be(repositoryCategory!.Descricao);
            outputItem.Ativo.Should().Be(repositoryCategory!.Ativo);
            outputItem.CriadoEm.Should().Be(repositoryCategory!.CriadoEm);
        });
        repositoryMock.Verify(x => x.List(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }

    [Fact(DisplayName = nameof(ListVazio))]
    public async Task ListVazio()
    {
        // Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var input = _fixture.GetInput();
        var searchOutput = new SearchOutput<entity.Graduacao>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: new List<entity.Graduacao>(),
            total: 0
        );
        repositoryMock.Setup(x => x.List(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(searchOutput);
        var useCase = new ListGraduacao(repositoryMock.Object);

        // Act
        var output = await useCase.Handle(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Page.Should().Be(searchOutput.CurrentPage);
        output.PerPage.Should().Be(searchOutput.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);
        repositoryMock.Verify(x => x.List(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }

    [Theory(DisplayName = nameof(ListInputWithoutAllParameters))]
    [MemberData(
        nameof(ListGraduacaoDataGenerator.GetInputsWithoutAllParameter),
        parameters: 14,
        MemberType = typeof(ListGraduacaoDataGenerator)
    )]
    public async Task ListInputWithoutAllParameters(
        ListGraduacaoInput input
    )
    {
        // Arrange
        var graduacoes = _fixture.GetGraduacaoList();
        var repositoryMock = _fixture.GetRepositoryMock();
        var searchOutput = new SearchOutput<entity.Graduacao>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: (IReadOnlyList<entity.Graduacao>) graduacoes,
            total: new Random().Next(50, 200)
        );
        repositoryMock.Setup(x => x.List(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(searchOutput);
        var useCase = new ListGraduacao(repositoryMock.Object);

        // Act
        var output = await useCase.Handle(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
        output.Page.Should().Be(searchOutput.CurrentPage);
        output.PerPage.Should().Be(searchOutput.PerPage);
        output.Total.Should().Be(searchOutput.Total);
        output.Items.Should().HaveCount(searchOutput.Items.Count);
        ((List<GraduacaoModelOutput>)output.Items).ForEach(outputItem =>
        {
            var repositoryCategory = searchOutput.Items
                .FirstOrDefault(x => x.Id == outputItem.Id);
            outputItem.Should().NotBeNull();
            outputItem.Nome.Should().Be(repositoryCategory!.Nome);
            outputItem.Descricao.Should().Be(repositoryCategory!.Descricao);
            outputItem.Ativo.Should().Be(repositoryCategory!.Ativo);
            outputItem.CriadoEm.Should().Be(repositoryCategory!.CriadoEm);
        });
        repositoryMock.Verify(x => x.List(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        ), Times.Once);        
    }
}
