namespace KenLo.Application.UseCases.Graduacao;
public class CreateGraduacaoInput
{
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public bool Ativo { get; set; }

    public CreateGraduacaoInput(
        string nome,
        string? descricao = null,
        bool ativo = true
    )
    {
        Nome = nome;
        Descricao = descricao ?? "";
        Ativo = ativo;
    }
}