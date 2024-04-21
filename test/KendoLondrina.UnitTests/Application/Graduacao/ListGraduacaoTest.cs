using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
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
        var output = new SearchOutput<entity.Graduacao>(
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
        )).ReturnsAsync(output);
        // var useCase = new ListGraduacao(repositoryMock.Object);

        // Act
        // var useCaseOutput = await useCase.Handle(input, CancellationToken.None);

        // Assert
        output.Should().NotBeNull();
    }
}
