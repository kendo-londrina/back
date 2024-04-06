using KenLo.Application.UseCases.Graduacao;

namespace KenLo.UnitTests.Application.Graduacao;

[CollectionDefinition(nameof(CreateGraduacaoFixture))]
public class CreateGraduacaoFixtureCollection
    : ICollectionFixture<CreateGraduacaoFixture>
{ }

public class CreateGraduacaoFixture : GraduacaoUseCasesBaseFixture
{
    public CreateGraduacaoInput GetInput()
        => new(
            ObterNomeValido(),
            ObterDescricaoValida(),
            GetRandomBoolean()
        );

 public CreateGraduacaoInput GetInvalidInputNomeCurto()
    {
        var invalidInput = GetInput();
        invalidInput.Nome =
            invalidInput.Nome.Substring(0, 2);
        return invalidInput;
    }

    public CreateGraduacaoInput GetInvalidInputNomeLongo()
    {
        var invalidInput = GetInput();
        var nomeLongo = Faker.Commerce.ProductName();
        while (nomeLongo.Length <= 255)
            nomeLongo = $"{nomeLongo} {Faker.Commerce.ProductName()}";
        invalidInput.Nome = nomeLongo;
        return invalidInput;
    }

    public CreateGraduacaoInput GetInvalidInputDescricaoNull()
    {
        var invalidInput = GetInput();
        invalidInput.Descricao = null!;
        return invalidInput;
    }

    public CreateGraduacaoInput GetInvalidInputDescricaoLonga()
    {
        var invalidInput = GetInput();
        var descricaoLonga = Faker.Commerce.ProductDescription();
        while (descricaoLonga.Length <= 10_000)
            descricaoLonga = $"{descricaoLonga} {Faker.Commerce.ProductDescription()}";
        invalidInput.Descricao = descricaoLonga;
        return invalidInput;
    }
}
