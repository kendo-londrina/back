using System;
using System.Collections.Generic;
using KenLo.Application.UseCases.Graduacao;
using KenLo.Domain.SeedWork.SearchableRepository;
using entity = KenLo.Domain.Entity;

namespace KenLo.UnitTests.Application.Graduacao;

[CollectionDefinition(nameof(ListGraduacaoFixture))]
public class ListGraduacaoFixtureCollection : ICollectionFixture<ListGraduacaoFixture>
{
    // esta classe com a Anotation acima serve para que a fixture (classe) possa ser injetada no teste
}

public class ListGraduacaoFixture : GraduacaoUseCasesBaseFixture
{
    public List<entity.Graduacao> GetGraduacaoList(int length = 10)
    {
        var list = new List<entity.Graduacao>();
        for (int i = 0; i < length; i++)
            list.Add(GetGraduacao());
        return list;
    }

    public ListGraduacaoInput GetInput()
    {
        var random = new Random();
        return new ListGraduacaoInput(
            page: random.Next(1, 10),
            perPage: random.Next(15, 100),
            search: Faker.Commerce.ProductName(),
            sort: Faker.Commerce.ProductName(),
            dir: random.Next(0, 10) > 5 ?
                SearchOrder.Asc : SearchOrder.Desc
        );
    }
    
}
