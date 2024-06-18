using System.Collections.Generic;

namespace KenLo.IntegrationTests.Application.Usecases.Graduacao;

public static class UpdateGraduacaoDataGenerator
{
    public static IEnumerable<object[]> GetGraduacoes(int times = 10)
    {
        var fixture = new UpdateGraduacaoFixture();
        for (int i = 0; i < times; i++)
        {
            var graduacaoExemplo = fixture.GetGraduacao();
            var input = fixture.GetInput(graduacaoExemplo.Id);
            yield return new object[] {
                graduacaoExemplo, input
            };
        }
    }

    public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
    {
        var fixture = new UpdateGraduacaoFixture();
        var invalidInputsList = new List<object[]>();
        var totalInvalidCases = 3;

        for (int index = 0; index < times; index++)
        {
            switch (index % totalInvalidCases)
            {
                case 0:
                    invalidInputsList.Add(new object[] {
                        fixture.GetInvalidInputNomeCurto(),
                        "Nome should be at least 3 characters long"
                    });
                    break;
                case 1:
                    invalidInputsList.Add(new object[] {
                        fixture.GetInvalidInputNomeLongo(),
                        "Nome should be less or equal 255 characters long"
                    });
                    break;
                case 2:
                    invalidInputsList.Add(new object[] {
                        fixture.GetInvalidInputDescricaoLonga(),
                        "Descricao should be less or equal 10000 characters long"
                    });
                    break;
                default:
                    break;
            }
        }

        return invalidInputsList;
    }    
}
