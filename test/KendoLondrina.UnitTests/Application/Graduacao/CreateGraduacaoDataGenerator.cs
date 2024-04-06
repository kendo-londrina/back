using System.Collections.Generic;

namespace KenLo.UnitTests.Application.Graduacao;

public static class CreateGraduacaoDataGenerator
{
   public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
    {
        var fixture = new CreateGraduacaoFixture();
        var invalidInputsList = new List<object[]>();
        var totalInvalidCases = 4;

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
                        fixture.GetInvalidInputDescricaoNull(),
                        "Descricao should not be null"
                    });
                    break;
                case 3:
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
