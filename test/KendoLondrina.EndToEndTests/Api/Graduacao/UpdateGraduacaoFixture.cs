using System;
using KenLo.Application.UseCases.Graduacao;

namespace KenLo.EndToEndTests.Api.Graduacao;

[CollectionDefinition(nameof(UpdateGraduacaoFixture))]
public class UpdateGraduacaoFixtureCollection: ICollectionFixture<UpdateGraduacaoFixture>
{ }

public class UpdateGraduacaoFixture: GraduacaoBaseFixture
{
    public UpdateGraduacaoInput getExampleInput()
        => new(
            Guid.NewGuid(),
            GetNomeValido(),
            GetDescricaoValida(),
            GetRandomBoolean()
        );    

}
