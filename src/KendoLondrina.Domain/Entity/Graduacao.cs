using KenLo.Domain.SeedWork;
using KenLo.Domain.Validation;

namespace KenLo.Domain.Entity;

public class Graduacao : AggregateRoot
{
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public string? ProximaGraduacao { get; private set; }
    public int? CarenciaEmAnos { get; private set; }
    public bool Ativo { get; private set; }
    public DateTime CriadoEm { get; private set; }

    public Graduacao(
        string nome,
        string descricao,
        bool ativo = true,
        string? proximaGraduacao = null,
        int? carenciaEmAnos = null
    ) : base()
    {
        Nome = nome;
        Descricao = descricao;
        Ativo = ativo;
        ProximaGraduacao = proximaGraduacao;
        CarenciaEmAnos = carenciaEmAnos;
        CriadoEm = DateTime.Now;

        Validate();
    }

    public void Ativar()
    {
        Ativo = true;
        Validate();
    }
    public void Desativar()
    {
        Ativo = false;
        Validate();
    }

    public void Update(
        string nome,
        string? descricao = null,
        string? proximaGraduacao = null,
        int? carenciaEmAnos = null
    )
    {
        Nome = nome;
        Descricao = descricao ?? Descricao;
        ProximaGraduacao = proximaGraduacao ?? ProximaGraduacao;
        CarenciaEmAnos = carenciaEmAnos ?? CarenciaEmAnos;

        Validate();
    }

    private void Validate()
    {
        DomainValidation.NotNullOrEmpty(Nome, nameof(Nome));
        DomainValidation.MinLength(Nome, 3, nameof(Nome));
        DomainValidation.MaxLength(Nome, 255, nameof(Nome));

        DomainValidation.NotNull(Descricao, nameof(Descricao));
        DomainValidation.MaxLength(Descricao, 10_000, nameof(Descricao));
    }    
}
