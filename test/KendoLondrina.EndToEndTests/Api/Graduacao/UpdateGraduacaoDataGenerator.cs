using System.Collections.Generic;

namespace KenLo.EndToEndTests.Api.Graduacao;

public class UpdateGraduacaoDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs()
    {
        var fixture = new UpdateGraduacaoFixture();
        var invalidInputsList = new List<object[]>();
        var totalInvalidCases = 3;

        for (int index = 0; index < totalInvalidCases; index++)
        {
            switch (index % totalInvalidCases)
            {
                case 0:
                    var input1 = fixture.getExampleInput();
                    input1.Nome = fixture.GetInvalidNameTooShort();
                    invalidInputsList.Add(new object[] {
                        input1,
                        "Nome should be at least 3 characters long"
                    });
                    break;
                case 1:
                    var input2 = fixture.getExampleInput();
                    input2.Nome = fixture.GetInvalidNameTooLong();
                    invalidInputsList.Add(new object[] {
                        input2,
                        "Nome should be less or equal 255 characters long"
                    });
                    break;
                case 2:
                    var input3 = fixture.getExampleInput();
                    input3.Descricao = fixture.GetInvalidDescriptionTooLong();
                    invalidInputsList.Add(new object[] {
                        input3,
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