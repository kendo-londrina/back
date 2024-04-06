namespace KenLo.UnitTests.Application.Graduacao;

[CollectionDefinition(nameof(GetGraduacaoFixture))]
public class GetGraduacaoFixtureCollection : ICollectionFixture<GetGraduacaoFixture>
{ }

public class GetGraduacaoFixture : GraduacaoUseCasesBaseFixture
{ }
