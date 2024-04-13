namespace KenLo.Application.UseCases.Graduacao;

public class UpdateGraduacaoInput
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string? Descricao { get; set; }
    public bool? Ativo { get; set; }

    public UpdateGraduacaoInput(
        Guid id,
        string nome,
        string? descricao = null,
        bool? ativo = null)
    {
        Id = id;
        Nome = nome;
        Descricao = descricao;
        Ativo = ativo;
    }
}
