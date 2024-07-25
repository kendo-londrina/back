namespace KenLo.EndToEndTests.Api.Graduacao;

[CollectionDefinition(nameof(DeleteGraduacaoFixture))]
public class DeleteGraduacaoFixtureCollection
    : ICollectionFixture<DeleteGraduacaoFixture>
{ }

public class DeleteGraduacaoFixture : GraduacaoBaseFixture
{ }
