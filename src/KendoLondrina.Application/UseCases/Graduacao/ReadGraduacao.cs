using domain = KenLo.Domain.Entity;
using KenLo.Domain.Repository;

namespace KenLo.Application.UseCases.Graduacao;

public interface IReadGraduacao
{
    Task<GraduacaoModelOutput> Handle(
        ReadGraduacaoInput input,
        CancellationToken cancellationToken
    );
}

public class ReadGraduacao: IReadGraduacao
{
    private readonly IGraduacaoRepository _graduacaoRepository;

    public ReadGraduacao(IGraduacaoRepository graduacaoRepository)
    {
        _graduacaoRepository = graduacaoRepository;
    }

    public async Task<GraduacaoModelOutput> Handle(
        ReadGraduacaoInput input,
        CancellationToken cancellationToken)
    {
        var graduacao = await _graduacaoRepository.Read(input.Id, cancellationToken);
        return GraduacaoModelOutput.FromGraduacao(graduacao);
    }
}