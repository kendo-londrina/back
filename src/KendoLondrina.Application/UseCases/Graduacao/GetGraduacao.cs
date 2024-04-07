using domain = KenLo.Domain.Entity;
using KenLo.Domain.Repository;

namespace KenLo.Application.UseCases.Graduacao;
public class GetGraduacao
{
    private readonly IGraduacaoRepository _graduacaoRepository;

    public GetGraduacao(IGraduacaoRepository graduacaoRepository)
    {
        _graduacaoRepository = graduacaoRepository;
    }

    public async Task<GraduacaoModelOutput> Handle(
        GetGraduacaoInput input,
        CancellationToken cancellationToken)
    {
        var graduacao = await _graduacaoRepository.Read(input.Id, cancellationToken);
        return GraduacaoModelOutput.FromGraduacao(graduacao);
    }
}