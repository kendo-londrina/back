namespace KenLo.EndToEndTests.Api.Graduacao;

[CollectionDefinition(nameof(ReadGraduacaoFixture))]
public class ReadGraduacaoFixtureCollection: ICollectionFixture<ReadGraduacaoFixture>
{ }

public class ReadGraduacaoFixture: GraduacaoBaseFixture
{ }
