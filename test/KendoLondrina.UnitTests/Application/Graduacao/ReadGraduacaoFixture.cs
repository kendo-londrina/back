namespace KenLo.UnitTests.Application.Graduacao;

[CollectionDefinition(nameof(ReadGraduacaoFixture))]
public class ReadGraduacaoFixtureCollection : ICollectionFixture<ReadGraduacaoFixture>
{ }

public class ReadGraduacaoFixture : GraduacaoUseCasesBaseFixture
{ }
