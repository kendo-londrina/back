using System.Collections.Generic;
using System.Linq;
using KenLo.Domain.Entity;
using Xunit;

namespace KenLo.IntegrationTests.Infra.Data.EF.Repositories;

[CollectionDefinition(nameof(GraduacaoRepositoryFixture))]
public class GraduacaoRepositoryFixtureCollection : ICollectionFixture<GraduacaoRepositoryFixture>
{
    // esta classe com a Anotation acima serve para que a fixture (classe) possa ser injetada no teste
}

public class GraduacaoRepositoryFixture : BaseFixture
{
    public string GetNomeValido()
    {
        var nome = "";
        while (nome.Length < 3)
            nome = Faker.Commerce.Categories(1)[0];
        if (nome.Length > 255)
            nome = nome[..255];
        return nome;
    }

    public string GetDescricaoValida()
    {
        var descricao = Faker.Commerce.ProductDescription();
        if (descricao.Length > 10_000)
            descricao = descricao[..10_000];
        return descricao;
    }

    public Graduacao GetGraduacaoValida()
        => new(
            GetNomeValido(),
            GetDescricaoValida(),
            true
        );
    
    public List<Graduacao> GetGraduacaoList(int length = 10)
        => Enumerable.Range(1, length)
            .Select(_ => GetGraduacaoValida()).ToList();
}