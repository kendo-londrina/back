using domain = KenLo.Domain.Entity;
using KenLo.Domain.Repository;
using KenLo.Application.Interfaces;

namespace KenLo.Application.UseCases.Graduacao;
public class DeleteGraduacao
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGraduacaoRepository _graduacaoRepository;

    public DeleteGraduacao(IUnitOfWork unitOfWork, IGraduacaoRepository graduacaoRepository)
    {
        _unitOfWork = unitOfWork;
        _graduacaoRepository = graduacaoRepository;
    }

    public async Task Handle(
        DeleteGraduacaoInput input,
        CancellationToken cancellationToken)
    {
        var graduacao = await _graduacaoRepository.Read(input.Id, cancellationToken);
        await _graduacaoRepository.Delete(graduacao, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);
    }
}