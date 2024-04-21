using System.Collections.Generic;
using KenLo.Application.UseCases.Graduacao;

namespace KenLo.UnitTests.Application.Graduacao;

public static class ListGraduacaoDataGenerator
{
    public static IEnumerable<object[]> GetInputsWithoutAllParameter(int times = 14)
    {
        var fixture = new ListGraduacaoFixture();
        var input = fixture.GetInput();
        for (int i = 0; i < times; i++)
        {
            switch (i % 7)
            {
                case 0:
                    yield return new object[] {
                        new ListGraduacaoInput()
                    };
                    break;
                case 1:
                    yield return new object[] {
                        new ListGraduacaoInput(input.Page)
                    };
                    break;
                case 3:
                    yield return new object[] {
                        new ListGraduacaoInput(
                            input.Page,
                            input.PerPage
                        )
                    };
                    break;
                case 4:
                    yield return new object[] {
                        new ListGraduacaoInput(
                            input.Page,
                            input.PerPage,
                            input.Search
                        )
                    };
                    break;
                case 5:
                    yield return new object[] {
                        new ListGraduacaoInput(
                            input.Page,
                            input.PerPage,
                            input.Search,
                            input.Sort
                        )
                    };
                    break;
                case 6:
                    yield return new object[] { input };
                    break;
                default:
                    yield return new object[] {
                        new ListGraduacaoInput()
                    };
                    break;
            }
        }
    }
    
}