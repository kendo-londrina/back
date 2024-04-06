using domain = KenLo.Domain.Entity;

namespace KenLo.Application.UseCases.Graduacao;
public class GraduacaoModelOutput
{
    public Guid Id { get; set;}
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public string? ProximaGraduacao { get; set; }
    public int? CarenciaEmAnos { get; set; }
    public bool Ativo { get; set; }
    public DateTime CriadoEm { get; set; }

    public GraduacaoModelOutput(
        Guid id,
        string nome,
        string descricao,
        string? proximaGraduacao,
        int? carenciaEmAnos,
        bool ativo,
        DateTime criadoEm
    )
    {
        Id = id;
        Nome = nome;
        Descricao = descricao;
        ProximaGraduacao = proximaGraduacao;
        CarenciaEmAnos = carenciaEmAnos;
        Ativo = ativo;
        CriadoEm = criadoEm;
    }

    public static GraduacaoModelOutput FromGraduacao(domain.Graduacao graduacao)
        => new GraduacaoModelOutput(
            graduacao.Id,
            graduacao.Nome,
            graduacao.Descricao,
            graduacao.ProximaGraduacao,
            graduacao.CarenciaEmAnos,
            graduacao.Ativo,
            graduacao.CriadoEm
        );
}