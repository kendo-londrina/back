using KenLo.EndToEndTests.Base;
using entity = KenLo.Domain.Entity;

namespace KenLo.EndToEndTests.Api.Graduacao;

public class GraduacaoBaseFixture: BaseFixture
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

    public entity.Graduacao GetGraduacao()
        => new(
            GetNomeValido(),
            GetDescricaoValida(),
            GetRandomBoolean()
        );

    public string GetInvalidNameTooShort()
        => Faker.Commerce.ProductName().Substring(0, 2);

    public string GetInvalidNameTooLong()
    {
        var tooLongNameForCategory = Faker.Commerce.ProductName();
        while (tooLongNameForCategory.Length <= 255)
            tooLongNameForCategory = $"{tooLongNameForCategory} {Faker.Commerce.ProductName()}";
        return tooLongNameForCategory;
    }

    public string GetInvalidDescriptionTooLong()
    {
        var tooLongDescriptionForCategory = Faker.Commerce.ProductDescription();
        while (tooLongDescriptionForCategory.Length <= 10_000)
            tooLongDescriptionForCategory = $"{tooLongDescriptionForCategory} {Faker.Commerce.ProductDescription()}";
        return tooLongDescriptionForCategory;
    }        
}