using KenLo.Domain.Entity;
using Xunit;

namespace KenLo.IntegrationTests.Infra.Data.EF.Repositories;

[CollectionDefinition(nameof(GraduacaoRepositoryFixture))]
class GraduacaoRepositoryFixtureCollection : ICollectionFixture<GraduacaoRepositoryFixture>
{}

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
    
}