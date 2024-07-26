using KenLo.Application.Interfaces;
using KenLo.Domain.Repository;
using domain = KenLo.Domain.Entity;

namespace KenLo.Application.UseCases.Graduacao;

public interface IUpdateGraduacao
{
    Task<GraduacaoModelOutput> Handle(
        UpdateGraduacaoInput input, 
        CancellationToken cancellationToken
    );
}

public class UpdateGraduacao: IUpdateGraduacao
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGraduacaoRepository _repository;

    public UpdateGraduacao(IUnitOfWork unitOfWork, IGraduacaoRepository repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task<GraduacaoModelOutput> Handle(
        UpdateGraduacaoInput input, 
        CancellationToken cancellationToken)
    {
        var graduacao = await _repository.Read(input.Id, cancellationToken);
        graduacao.Update(
            input.Nome,
            input.Descricao
        );

        if (input.Ativo != null && input.Ativo != graduacao.Ativo) {
            if (input.Ativo.Value) graduacao.Ativar();
            else graduacao.Desativar();
        }

        await _repository.Update(graduacao, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return GraduacaoModelOutput.FromGraduacao(graduacao);        
    }


    
}