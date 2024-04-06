using KenLo.Application.Interfaces;
using KenLo.Domain.Repository;
using domain = KenLo.Domain.Entity;

namespace KenLo.Application.UseCases.Graduacao;

public class CreateGraduacao
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGraduacaoRepository _graduacaoRepository;

    public CreateGraduacao(IUnitOfWork unitOfWork, IGraduacaoRepository graduacaoRepository)
    {
        _unitOfWork = unitOfWork;
        _graduacaoRepository = graduacaoRepository;
    }

    public async Task<GraduacaoModelOutput> Handle(
        CreateGraduacaoInput input, 
        CancellationToken cancellationToken)
    {
        var graduacao = new domain.Graduacao(
            input.Nome,
            input.Descricao,
            input.Ativo
        );

        await _graduacaoRepository.Insert(graduacao, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);
        
        return GraduacaoModelOutput.FromGraduacao(graduacao);
    }    
}
