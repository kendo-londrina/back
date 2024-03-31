
using KenLo.Domain.Entity;
using KenLo.UnitTests.Common;

namespace KenLo.UnitTests.Domain.Entity;

public class GraduacaoFixture : BaseFixture
{
    public GraduacaoFixture()
        : base() {}

    public string ObterGraduacaoNomeValido()
    {
        var nome = "";
        while (nome.Length < 3)
            nome = Faker.Commerce.Categories(1)[0];
        if (nome.Length > 255)
            nome = nome[..255];
        return nome;
    }

    public string ObterGraduacaoDescricaoValida()
    {
        var descricao = Faker.Commerce.ProductDescription();
        if (descricao.Length > 10_000)
            descricao = descricao[..10_000];
        return descricao;
    }

    public Graduacao ObterGraduacaoValida()
        => new(
            ObterGraduacaoNomeValido(),
            ObterGraduacaoDescricaoValida(),
            true
        );
}

[CollectionDefinition(nameof(GraduacaoFixture))]
public class CategoryTestFixtureCollection 
    : ICollectionFixture<GraduacaoFixture>
{ }
