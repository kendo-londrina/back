namespace KenLo.Application.UseCases.Graduacao;
public class DeleteGraduacaoInput
{
    public Guid Id { get; set; }
    public DeleteGraduacaoInput(Guid id)
    {
        Id = id;
    }
}
