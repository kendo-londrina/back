using KenLo.Application.Interfaces;
using KenLo.Domain.Repository;
using KenLo.UnitTests.Common;
using Moq;
using entity = KenLo.Domain.Entity;

namespace KenLo.UnitTests.Application.Graduacao;

public abstract class GraduacaoUseCasesBaseFixture
    : BaseFixture
{

    public Mock<IGraduacaoRepository> GetRepositoryMock()
        => new();

    public Mock<IUnitOfWork> GetUnitOfWorkMock()
        => new();

    public string GetNomeValido()
    {
        var nome = "";
        while (nome.Length < 3)
            nome = Faker.Commerce.Categories(1)[0];
        if (nome.Length > 255)
            nome = nome[..255];
        return nome;
    }

    public string GetDescricaoValida()
    {
        var descricao = Faker.Commerce.ProductDescription();
        if (descricao.Length > 10_000)
            descricao = descricao[..10_000];
        return descricao;
    }

    public entity.Graduacao GetGraduacao()
        => new(
            GetNomeValido(),
            GetDescricaoValida(),
            GetRandomBoolean()
        );
}