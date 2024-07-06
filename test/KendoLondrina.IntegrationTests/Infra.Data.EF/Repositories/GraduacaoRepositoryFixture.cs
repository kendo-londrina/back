using System.Collections.Generic;
using System.Linq;
using KenLo.Domain.Entity;
using KenLo.Domain.SeedWork.SearchableRepository;

namespace KenLo.IntegrationTests.Infra.Data.EF.Repositories;

[CollectionDefinition(nameof(GraduacaoRepositoryFixture))]
public class GraduacaoRepositoryFixtureCollection : ICollectionFixture<GraduacaoRepositoryFixture>
{
    // esta classe com a Anotation acima serve para que a fixture (classe) possa ser injetada no teste
}

public class GraduacaoRepositoryFixture : BaseFixture
{
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

    public Graduacao GetGraduacaoValida()
        => new(
            GetNomeValido(),
            GetDescricaoValida(),
            true
        );
    
    public List<Graduacao> GetGraduacaoList(int length = 10)
        => Enumerable.Range(1, length)
            .Select(_ => GetGraduacaoValida()).ToList();

    public List<Graduacao> GetGraduacaoListComNomes(List<string> nomes)
        => nomes.Select(nome =>
        {
            var graduacao = GetGraduacaoValida();
            graduacao.Update(nome);
            return graduacao;
        }).ToList();

    public List<Graduacao> CloneGraduacaoListOrdered(
        List<Graduacao> graduacaoList,
        string orderBy,
        SearchOrder order
    )
    {
        var listClone = new List<Graduacao>(graduacaoList);
        var orderedEnumerable = (orderBy.ToLower(), order) switch
        {
            ("nome", SearchOrder.Asc) => listClone.OrderBy(x => x.Nome)
                .ThenBy(x => x.Id),
            ("nome", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Nome)
                .ThenByDescending(x => x.Id),
            ("id", SearchOrder.Asc) => listClone.OrderBy(x => x.Id),
            ("id", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Id),
            ("criadoem", SearchOrder.Asc) => listClone.OrderBy(x => x.CriadoEm),
            ("criadoem", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CriadoEm),
            _ => listClone.OrderBy(x => x.Nome).ThenBy(x => x.Id),
        };
        return orderedEnumerable.ToList();
    }        
}