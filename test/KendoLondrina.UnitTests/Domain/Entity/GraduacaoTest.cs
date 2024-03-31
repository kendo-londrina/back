using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using KenLo.Domain.Entity;
using KenLo.Domain.Exceptions;
using KenLo.UnitTests.Domain.Entity;

namespace KenLo.UnitTests.Domain.Entity;

[Collection(nameof(GraduacaoFixture))]
public class GraduacaoTest
{
    private readonly GraduacaoFixture _graduacaoFixture;

    public GraduacaoTest(GraduacaoFixture graduacaoFixture) 
        => _graduacaoFixture = graduacaoFixture;

    [Fact(DisplayName = nameof(Instanciar))]
    [Trait("Domain", "Graduacao - Aggregates")]
    public void Instanciar()
    {
        var graducaoValida = _graduacaoFixture.ObterGraduacaoValida();
        var datetimeBefore = DateTime.Now;

        var graduacao = new Graduacao(graducaoValida.Nome, graducaoValida.Descricao);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        graduacao.Should().NotBeNull();
        graduacao.Nome.Should().Be(graducaoValida.Nome);
        graduacao.Descricao.Should().Be(graducaoValida.Descricao);
        graduacao.Id.Should().NotBeEmpty();
        graduacao.CriadoEm.Should().NotBeSameDateAs(default(DateTime));
        (graduacao.CriadoEm >= datetimeBefore).Should().BeTrue();
        (graduacao.CriadoEm <= datetimeAfter).Should().BeTrue();
        graduacao.Ativo.Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstanciarComAtivo))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstanciarComAtivo(bool ativo)
    {
        var graduacaoValida = _graduacaoFixture.ObterGraduacaoValida();
        var datetimeBefore = DateTime.Now;

        var graduacao = new Graduacao(graduacaoValida.Nome, graduacaoValida.Descricao, ativo);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        graduacao.Should().NotBeNull();
        graduacao.Nome.Should().Be(graduacaoValida.Nome);
        graduacao.Descricao.Should().Be(graduacaoValida.Descricao);
        graduacao.Id.Should().NotBeEmpty();
        graduacao.CriadoEm.Should().NotBeSameDateAs(default(DateTime));
        (graduacao.CriadoEm >= datetimeBefore).Should().BeTrue();
        (graduacao.CriadoEm <= datetimeAfter).Should().BeTrue();
        (graduacao.Ativo).Should().Be(ativo);
    }

    [Theory(DisplayName = nameof(ErroAoInstanciarComNomeVazio))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void ErroAoInstanciarComNomeVazio(string? nome)
    {
        var graduacaoValida = _graduacaoFixture.ObterGraduacaoValida();

        Action action =
            () => new Graduacao(nome!, graduacaoValida.Descricao);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Nome should not be empty or null");
    }

    [Fact(DisplayName = nameof(ErroAoInstanciarComDescricaoNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void ErroAoInstanciarComDescricaoNull()
    {
        var graduacaoValida = _graduacaoFixture.ObterGraduacaoValida();

        Action action =
            () => new Graduacao(graduacaoValida.Nome, null!);
        
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Descricao should not be null");
    }

    [Theory(DisplayName = nameof(ErroAoInstanciarComNomeMenorQue3Caracteres))]
    [Trait("Domain", "Category - Aggregates")]
    [MemberData(nameof(ObterNomesComMenosDe3Caracteres), parameters: 10)]
    public void ErroAoInstanciarComNomeMenorQue3Caracteres(string nomeInvalido)
    {
        var graduacaoValida = _graduacaoFixture.ObterGraduacaoValida();

        Action action =
            () => new Graduacao(nomeInvalido, graduacaoValida.Descricao);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Nome should be at least 3 characters long");
    }

    public static IEnumerable<object[]> ObterNomesComMenosDe3Caracteres(int numberOfTests = 6)
    {
        var fixture = new GraduacaoFixture();
        for (int i = 0; i < numberOfTests; i++)
        { 
            var isOdd = i % 2 == 1;
            yield return new object[] {
                fixture.ObterGraduacaoNomeValido()[..(isOdd ? 1 : 2)]
            };
        }
    }

    [Fact(DisplayName = nameof(ErroAoInstanciarComNomeMaiorQue255Caracteres))]
    [Trait("Domain", "Category - Aggregates")]
    public void ErroAoInstanciarComNomeMaiorQue255Caracteres()
    {
        var graduacaoValida = _graduacaoFixture.ObterGraduacaoValida();
        var nomeInvalido = String.Join(null,Enumerable.Range(1, 256).Select(_ => "a").ToArray());

        Action action =
            () => new Graduacao(nomeInvalido, graduacaoValida.Descricao);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Nome should be less or equal 255 characters long");
    }

    [Fact(DisplayName = nameof(ErroAoInstanciarComDescricaoMaiorQue10_000Caracteres))]
    [Trait("Domain", "Category - Aggregates")]
    public void ErroAoInstanciarComDescricaoMaiorQue10_000Caracteres()
    {
        var descricaoInvalida = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        var graduacaoValida = _graduacaoFixture.ObterGraduacaoValida();

        Action action =
            () => new Graduacao(graduacaoValida.Nome, descricaoInvalida);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Descricao should be less or equal 10000 characters long");
    }

    [Fact(DisplayName = nameof(Ativar))]
    [Trait("Domain", "Category - Aggregates")]
    public void Ativar()
    {
        var graduacaoValida = _graduacaoFixture.ObterGraduacaoValida();

        var graduacao = new Graduacao(graduacaoValida.Nome, graduacaoValida.Descricao, false);
        graduacao.Ativar();

        graduacao.Ativo.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Desativar))]
    [Trait("Domain", "Category - Aggregates")]
    public void Desativar()
    {
        var graduacaoValida = _graduacaoFixture.ObterGraduacaoValida();

        var graduacao = new Graduacao(graduacaoValida.Nome, graduacaoValida.Descricao, true);
        graduacao.Desativar();

        graduacao.Ativo.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Atualizar))]
    [Trait("Domain", "Category - Aggregates")]
    public void Atualizar()
    {
        var umaGraduacao = _graduacaoFixture.ObterGraduacaoValida();
        var outraGraduacao = _graduacaoFixture.ObterGraduacaoValida();

        umaGraduacao.Atualizar(outraGraduacao.Nome, outraGraduacao.Descricao);

        umaGraduacao.Nome.Should().Be(outraGraduacao.Nome);
        umaGraduacao.Descricao.Should().Be(outraGraduacao.Descricao);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        var category = _graduacaoFixture.ObterGraduacaoValida();
        var newName = _graduacaoFixture.ObterGraduacaoNomeValido();
        var currentDescription = category.Descricao;

        category.Atualizar(newName);

        category.Nome.Should().Be(newName);
        category.Descricao.Should().Be(currentDescription);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var category = _graduacaoFixture.ObterGraduacaoValida();
        Action action =
            () => category.Atualizar(name!);
        
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Nome should not be empty or null");
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ca")]
    public void UpdateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        var category = _graduacaoFixture.ObterGraduacaoValida();

        Action action =
            () => category.Atualizar(invalidName);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Nome should be at least 3 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters()
    {
        var category = _graduacaoFixture.ObterGraduacaoValida();
        var invalidName = _graduacaoFixture.Faker.Lorem.Letter(256);

        Action action =
            () => category.Atualizar(invalidName);
        
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Nome should be less or equal 255 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var category = _graduacaoFixture.ObterGraduacaoValida();
        var invalidDescription = 
            _graduacaoFixture.Faker.Commerce.ProductDescription();
        while (invalidDescription.Length <= 10_000)
            invalidDescription = $"{invalidDescription} {_graduacaoFixture.Faker.Commerce.ProductDescription()}";                            
        
        Action action =
            () => category.Atualizar("Category New Name", invalidDescription);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Descricao should be less or equal 10000 characters long");
    }
}