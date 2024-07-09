using KenLo.Application.UseCases.Graduacao;

namespace KenLo.EndToEndTests.Api.Graduacao;

[CollectionDefinition(nameof(CreateGraduacaoFixture))]
public class GraduacaoRepositoryFixtureCollection : ICollectionFixture<CreateGraduacaoFixture>
{
    // esta classe com a Anotation acima serve para que a fixture (classe) possa ser injetada no teste
}

public class CreateGraduacaoFixture: GraduacaoBaseFixture
{
    public CreateGraduacaoInput getExampleInput()
        => new(
            GetNomeValido(),
            GetDescricaoValida(),
            GetRandomBoolean()
        );

}
