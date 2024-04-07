namespace KenLo.Application.UseCases.Graduacao;
public class ReadGraduacaoInput
{
    public Guid Id { get; set; }
    public ReadGraduacaoInput(Guid id)
    {
        Id = id;
    }
}
