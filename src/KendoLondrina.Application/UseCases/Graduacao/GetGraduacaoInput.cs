namespace KenLo.Application.UseCases.Graduacao;
public class GetGraduacaoInput
{
    public Guid Id { get; set; }
    public GetGraduacaoInput(Guid id)
    {
        Id = id;
    }
}
