using System.Threading.Tasks;

namespace KenLo.UnitTests.Application.Graduacao;

[Collection(nameof(ListGraduacaoFixture))]
public class ListGraduacaoTest
{
    private readonly ListGraduacaoFixture _fixture;
    public ListGraduacaoTest(ListGraduacaoFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(List))]
    public async Task List()
    {
    }
}
