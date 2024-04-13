using System;
using KenLo.Application.UseCases.Graduacao;

namespace KenLo.UnitTests.Application.Graduacao;

[CollectionDefinition(nameof(DeleteGraduacaoFixture))]
public class DeleteGraduacaoFixtureCollection
    : ICollectionFixture<DeleteGraduacaoFixture>
{ }

public class DeleteGraduacaoFixture : GraduacaoUseCasesBaseFixture
{

}
