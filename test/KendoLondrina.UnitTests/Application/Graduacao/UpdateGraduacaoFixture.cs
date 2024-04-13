using System;
using KenLo.Application.UseCases.Graduacao;

namespace KenLo.UnitTests.Application.Graduacao;

[CollectionDefinition(nameof(UpdateGraduacaoFixture))]
public class UpdateGraduacaoFixtureCollection
    : ICollectionFixture<UpdateGraduacaoFixture>
{ }

public class UpdateGraduacaoFixture : GraduacaoUseCasesBaseFixture
{
    public UpdateGraduacaoInput GetInput(Guid? id = null)
    => new(
        id ?? Guid.NewGuid(),
        ObterNomeValido(),
        ObterDescricaoValida(),
        GetRandomBoolean()
    );

    public UpdateGraduacaoInput GetInvalidInputNomeCurto()
    {
        var invalidInput = GetInput();
        invalidInput.Nome =
            invalidInput.Nome.Substring(0, 2);
        return invalidInput;
    }

    public UpdateGraduacaoInput GetInvalidInputNomeLongo()
    {
        var invalidInput = GetInput();
        var nomeLongo = Faker.Commerce.ProductName();
        while (nomeLongo.Length <= 255)
            nomeLongo = $"{nomeLongo} {Faker.Commerce.ProductName()}";
        invalidInput.Nome = nomeLongo;
        return invalidInput;
    }

    public UpdateGraduacaoInput GetInvalidInputDescricaoNull()
    {
        var invalidInput = GetInput();
        invalidInput.Descricao = null!;
        return invalidInput;
    }

    public UpdateGraduacaoInput GetInvalidInputDescricaoLonga()
    {
        var invalidInput = GetInput();
        var descricaoLonga = Faker.Commerce.ProductDescription();
        while (descricaoLonga.Length <= 10_000)
            descricaoLonga = $"{descricaoLonga} {Faker.Commerce.ProductDescription()}";
        invalidInput.Descricao = descricaoLonga;
        return invalidInput;
    }
}
