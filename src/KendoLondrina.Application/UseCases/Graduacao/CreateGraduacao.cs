using KenLo.Application.Interfaces;
using KenLo.Domain.Repository;
using domain = KenLo.Domain.Entity;

namespace KenLo.Application.UseCases.Graduacao;

public interface ICreateGraduacao
{
    Task<GraduacaoModelOutput> Handle(
        CreateGraduacaoInput input, 
        CancellationToken cancellationToken
    );
}

public class CreateGraduacao: ICreateGraduacao
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

        await _graduacaoRepository.Create(graduacao, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);
        
        return GraduacaoModelOutput.FromGraduacao(graduacao);
    }    
}
