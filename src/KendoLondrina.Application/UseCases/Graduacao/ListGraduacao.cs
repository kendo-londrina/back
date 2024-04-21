using KenLo.Domain.Repository;

namespace KenLo.Application.UseCases.Graduacao;

public class ListGraduacao
{
    private readonly IGraduacaoRepository _graduacaoRepository;
    public ListGraduacao(IGraduacaoRepository graduacaoRepository)
    {
        _graduacaoRepository = graduacaoRepository;
    }

    public async Task<ListGraduacaoOutput> Handle(
        ListGraduacaoInput input,
        CancellationToken cancellationToken)
    {
        var searchOutput = await _graduacaoRepository.List(
            new(
                input.Page, 
                input.PerPage, 
                input.Search, 
                input.Sort, 
                input.Dir
            ),
            cancellationToken
        );
        return new ListGraduacaoOutput(
            searchOutput.CurrentPage,
            searchOutput.PerPage,
            searchOutput.Total,
            searchOutput.Items
                .Select(GraduacaoModelOutput.FromGraduacao)
                .ToList()
        );        
    }
}
